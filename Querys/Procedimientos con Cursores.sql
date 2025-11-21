use db20232000983
go

CREATE OR ALTER PROCEDURE spGenerarEstadoCuentaCliente
    @id_cliente INT,
    @fecha_inicio DATE,
    @fecha_fin DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tabla temporal para almacenar el estado de cuenta
    CREATE TABLE #EstadoCuenta (
        fecha DATE,
        tipo_movimiento VARCHAR(50),
        descripcion VARCHAR(200),
        debito DECIMAL(15,2),
        credito DECIMAL(15,2),
        saldo_acumulado DECIMAL(15,2)
    );

    DECLARE @fecha DATE;
    DECLARE @tipo_movimiento VARCHAR(50);
    DECLARE @descripcion VARCHAR(200);
    DECLARE @debito DECIMAL(15,2);
    DECLARE @credito DECIMAL(15,2);
    DECLARE @saldo_acumulado DECIMAL(15,2) = 0;

    CREATE TABLE #TransaccionesTemp (
        fecha DATE,
        tipo_movimiento VARCHAR(50),
        descripcion VARCHAR(200),
        debito DECIMAL(15,2),
        credito DECIMAL(15,2)
    );

    -- Insertar ventas a crédito
    INSERT INTO #TransaccionesTemp (fecha, tipo_movimiento, descripcion, debito, credito)
    SELECT 
        v.fecha_venta,
        'VENTA A CRÉDITO',
        'Factura #' + CAST(v.id_venta AS VARCHAR(10)) + ' - ' + CAST(COUNT(vd.id_detalle_venta) AS VARCHAR(5)) + ' productos',
        v.total,
        0
    FROM Ventas v
    INNER JOIN VentasDetalle vd ON v.id_venta = vd.id_venta
    WHERE v.id_cliente = @id_cliente
    AND v.tipo_venta = 'Credito'
    AND v.fecha_venta BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY v.id_venta, v.fecha_venta, v.total;
    
    -- Insertar pagos del cliente
    INSERT INTO #TransaccionesTemp (fecha, tipo_movimiento, descripcion, debito, credito)
    SELECT 
        pc.fecha_pago,
        'PAGO',
        'Recibo #' + CAST(pc.id_pago_cliente AS VARCHAR(10)),
        0,
        pc.monto
    FROM PagosClientes pc
    WHERE pc.id_cliente = @id_cliente
    AND pc.estado = 'Aplicado'
    AND pc.fecha_pago BETWEEN @fecha_inicio AND @fecha_fin;

    -- Cursor sobre la tabla temporal ordenada
    DECLARE curTransacciones CURSOR FOR
    SELECT fecha, tipo_movimiento, descripcion, debito, credito
    FROM #TransaccionesTemp
    ORDER BY fecha;

    OPEN curTransacciones;
    FETCH NEXT FROM curTransacciones INTO @fecha, @tipo_movimiento, @descripcion, @debito, @credito;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular saldo acumulado
        SET @saldo_acumulado = @saldo_acumulado + @debito - @credito;
        
        -- Insertar en tabla temporal
        INSERT INTO #EstadoCuenta (fecha, tipo_movimiento, descripcion, debito, credito, saldo_acumulado)
        VALUES (@fecha, @tipo_movimiento, @descripcion, @debito, @credito, @saldo_acumulado);
        
        FETCH NEXT FROM curTransacciones INTO @fecha, @tipo_movimiento, @descripcion, @debito, @credito;
    END

    CLOSE curTransacciones;
    DEALLOCATE curTransacciones;

    -- Retornar el estado de cuenta
    SELECT * FROM #EstadoCuenta ORDER BY fecha;

    -- Limpiar tablas temporales
    DROP TABLE #EstadoCuenta;
    DROP TABLE #TransaccionesTemp;
END
GO

CREATE OR ALTER PROCEDURE spGenerarOrdenesCompraAutomaticas
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_producto INT;
    DECLARE @nombre_producto VARCHAR(100);
    DECLARE @stock_actual INT;
    DECLARE @stock_minimo INT;
    DECLARE @id_proveedor_principal INT;
    DECLARE @precio_compra DECIMAL(10,2);
    DECLARE @cantidad_reorden INT;
    DECLARE @detalles_orden NVARCHAR(MAX) = '';
    DECLARE @proveedor_temp INT;

    -- Cursor para productos con stock bajo
    DECLARE curProductosStockBajo CURSOR FOR
    SELECT 
        p.id_producto,
        p.nombre,
        p.stock_actual,
        p.stock_minimo,
        p.precio_compra,
        (p.stock_minimo * 2 - p.stock_actual) AS cantidad_reorden -- Fórmula básica de reorden
    FROM Productos p
    WHERE p.estado = 1
    AND p.stock_actual <= p.stock_minimo
    AND p.stock_actual >= 0; -- Excluir productos con stock negativo

    OPEN curProductosStockBajo;
    FETCH NEXT FROM curProductosStockBajo INTO @id_producto, @nombre_producto, @stock_actual, @stock_minimo, @precio_compra, @cantidad_reorden;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT TOP 1 @proveedor_temp = id_proveedor 
        FROM Proveedores 
        WHERE estado = 1 
        ORDER BY NEWID();

        -- Asegurar que la cantidad de reorden sea positiva y razonable
        IF @cantidad_reorden > 0 AND @cantidad_reorden <= 1000
        BEGIN
            -- Agregar producto a los detalles de la orden (formato JSON)
            IF @detalles_orden != ''
                SET @detalles_orden = @detalles_orden + ',';
            
            SET @detalles_orden = @detalles_orden + '{"id_producto":' + CAST(@id_producto AS VARCHAR(10)) + 
                                 ',"cantidad":' + CAST(@cantidad_reorden AS VARCHAR(10)) + 
                                 ',"precio_unitario":' + CAST(@precio_compra AS VARCHAR(20)) + '}';
            
            PRINT 'Producto agregado a orden: ' + @nombre_producto + ' - Cantidad: ' + CAST(@cantidad_reorden AS VARCHAR(10));
            
            SET @id_proveedor_principal = @proveedor_temp;
        END
        
        FETCH NEXT FROM curProductosStockBajo INTO @id_producto, @nombre_producto, @stock_actual, @stock_minimo, @precio_compra, @cantidad_reorden;
    END

    CLOSE curProductosStockBajo;
    DEALLOCATE curProductosStockBajo;

    -- Si hay productos para ordenar, crear la orden de compra
    IF @detalles_orden != '' AND @id_proveedor_principal IS NOT NULL
    BEGIN
        SET @detalles_orden = '[' + @detalles_orden + ']';
        
        DECLARE @fecha_esperada DATE = DATEADD(DAY, 7, GETDATE());
        
        -- Usar el procedimiento existente para crear la orden
        EXEC sp_OrdenesCompra_Insertar 
            @id_proveedor = @id_proveedor_principal,
            @fecha_esperada = @fecha_esperada,
            @detalles = @detalles_orden;
            
        PRINT 'Orden de compra automática generada exitosamente';
    END
    ELSE
    BEGIN
        PRINT 'No hay productos que requieran orden de compra automática';
    END
END
GO

CREATE OR ALTER PROCEDURE spConsolidarMovimientosDiarios
    @fecha DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tipo_movimiento VARCHAR(50);
    DECLARE @categoria VARCHAR(100);
    DECLARE @total_monto DECIMAL(15,2);
    DECLARE @total_cantidad INT;

    -- Tabla temporal para resultados consolidados
    CREATE TABLE #Consolidado (
        tipo_movimiento VARCHAR(50),
        categoria VARCHAR(100),
        total_monto DECIMAL(15,2),
        total_cantidad INT,
        porcentaje_del_total DECIMAL(5,2)
    );

    DECLARE @total_general DECIMAL(15,2) = 0;

    -- Cursor para ventas del día por categoría
    DECLARE curVentas CURSOR FOR
    SELECT 
        'VENTA',
        c.nombre,
        SUM(vd.subtotal),
        SUM(vd.cantidad)
    FROM VentasDetalle vd
    INNER JOIN Ventas v ON vd.id_venta = v.id_venta
    INNER JOIN Productos p ON vd.id_producto = p.id_producto
    INNER JOIN Categorias c ON p.id_categoria = c.id_categoria
    WHERE v.fecha_venta = @fecha
    AND v.estado = 'Pagada'
    GROUP BY c.nombre;

    OPEN curVentas;
    FETCH NEXT FROM curVentas INTO @tipo_movimiento, @categoria, @total_monto, @total_cantidad;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #Consolidado (tipo_movimiento, categoria, total_monto, total_cantidad)
        VALUES (@tipo_movimiento, @categoria, @total_monto, @total_cantidad);
        
        SET @total_general = @total_general + @total_monto;
        
        FETCH NEXT FROM curVentas INTO @tipo_movimiento, @categoria, @total_monto, @total_cantidad;
    END

    CLOSE curVentas;
    DEALLOCATE curVentas;

    -- Cursor para compras del día por categoría
    DECLARE curCompras CURSOR FOR
    SELECT 
        'COMPRA',
        c.nombre,
        SUM(ocd.subtotal),
        SUM(ocd.cantidad)
    FROM OrdenesCompraDetalle ocd
    INNER JOIN OrdenesCompra oc ON ocd.id_orden_compra = oc.id_orden_compra
    INNER JOIN Productos p ON ocd.id_producto = p.id_producto
    INNER JOIN Categorias c ON p.id_categoria = c.id_categoria
    WHERE oc.fecha_orden = @fecha
    AND oc.estado = 'Aprobada'
    GROUP BY c.nombre;

    OPEN curCompras;
    FETCH NEXT FROM curCompras INTO @tipo_movimiento, @categoria, @total_monto, @total_cantidad;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #Consolidado (tipo_movimiento, categoria, total_monto, total_cantidad)
        VALUES (@tipo_movimiento, @categoria, @total_monto, @total_cantidad);
        
        SET @total_general = @total_general - @total_monto; -- Las compras son egresos
        
        FETCH NEXT FROM curCompras INTO @tipo_movimiento, @categoria, @total_monto, @total_cantidad;
    END

    CLOSE curCompras;
    DEALLOCATE curCompras;

    -- Calcular porcentajes
    IF @total_general > 0
    BEGIN
        UPDATE #Consolidado 
        SET porcentaje_del_total = (total_monto / @total_general) * 100;
    END

    -- Retornar el consolidado
    SELECT 
        tipo_movimiento,
        categoria,
        total_monto,
        total_cantidad,
        porcentaje_del_total
    FROM #Consolidado
    ORDER BY tipo_movimiento, total_monto DESC;

    -- Limpiar tabla temporal
    DROP TABLE #Consolidado;
END
GO