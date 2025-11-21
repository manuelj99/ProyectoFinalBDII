use db20232000983
go

CREATE OR ALTER FUNCTION dbo.fnCalcularEdadProducto(@id_producto INT)
RETURNS INT
AS
BEGIN
    DECLARE @edad_dias INT;
    
    SELECT @edad_dias = DATEDIFF(day, MIN(r.fecha_recepcion), GETDATE())
    FROM RecepcionesDetalle rd
    INNER JOIN Recepciones r ON rd.id_recepcion = r.id_recepcion
    WHERE rd.id_producto = @id_producto
    AND r.estado = 'Aceptada';
    
    RETURN ISNULL(@edad_dias, 0);
END
GO

CREATE OR ALTER FUNCTION dbo.fnCalcularMargenGanancia(@id_producto INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @margen DECIMAL(10,2);
    
    SELECT @margen = 
        CASE 
            WHEN precio_compra > 0 THEN ((precio_venta - precio_compra) / precio_compra) * 100
            ELSE 0
        END
    FROM Productos
    WHERE id_producto = @id_producto;
    
    RETURN ISNULL(@margen, 0);
END
GO

CREATE OR ALTER FUNCTION dbo.fnCalcularDiasInventario(@id_producto INT)
RETURNS INT
AS
BEGIN
    DECLARE @dias_restantes INT;
    DECLARE @stock_actual INT;
    DECLARE @venta_promedio_diaria DECIMAL(10,2);
    
    -- Obtener stock actual
    SELECT @stock_actual = stock_actual
    FROM Productos
    WHERE id_producto = @id_producto;
    
    -- Calcular venta promedio diaria (últimos 30 días)
    SELECT @venta_promedio_diaria = AVG(cantidad_diaria)
    FROM (
        SELECT vd.id_producto, v.fecha_venta, SUM(vd.cantidad) as cantidad_diaria
        FROM VentasDetalle vd
        INNER JOIN Ventas v ON vd.id_venta = v.id_venta
        WHERE vd.id_producto = @id_producto
        AND v.fecha_venta >= DATEADD(day, -30, GETDATE())
        AND v.estado = 'Pagada'
        GROUP BY vd.id_producto, v.fecha_venta
    ) AS ventas_diarias;
    
    -- Calcular días restantes
    IF @venta_promedio_diaria > 0
        SET @dias_restantes = @stock_actual / @venta_promedio_diaria;
    ELSE
        SET @dias_restantes = 999; -- Si no hay ventas, inventario dura "mucho"
    
    RETURN @dias_restantes;
END
GO

CREATE OR ALTER FUNCTION dbo.fnCalcularComisionVenta(@id_venta INT, @porcentaje_comision DECIMAL(5,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @comision DECIMAL(10,2);
    DECLARE @total_venta DECIMAL(15,2);
    
    SELECT @total_venta = total
    FROM Ventas
    WHERE id_venta = @id_venta
    AND estado = 'Pagada';
    
    SET @comision = @total_venta * (@porcentaje_comision / 100);
    
    RETURN ISNULL(@comision, 0);
END
GO

CREATE OR ALTER FUNCTION dbo.fnProductoProximoVencer(@id_producto INT, @dias_alerta INT)
RETURNS BIT
AS
BEGIN
    DECLARE @esta_proximo_vencer BIT = 0;
    DECLARE @fecha_vencimiento_mas_proxima DATE;
    
    -- En un sistema real, aquí buscaríamos en una tabla de lotes con fechas de vencimiento
    -- Por ahora simulamos con fecha de recepción + 365 días
    SELECT TOP 1 @fecha_vencimiento_mas_proxima = DATEADD(day, 365, r.fecha_recepcion)
    FROM RecepcionesDetalle rd
    INNER JOIN Recepciones r ON rd.id_recepcion = r.id_recepcion
    WHERE rd.id_producto = @id_producto
    AND r.estado = 'Aceptada'
    ORDER BY r.fecha_recepcion DESC;
    
    IF @fecha_vencimiento_mas_proxima IS NOT NULL 
       AND @fecha_vencimiento_mas_proxima <= DATEADD(day, @dias_alerta, GETDATE())
    BEGIN
        SET @esta_proximo_vencer = 1;
    END
    
    RETURN @esta_proximo_vencer;
END
GO

CREATE OR ALTER FUNCTION dbo.fnObtenerProductosPorCategoria(@id_categoria INT)
RETURNS TABLE
AS
RETURN (
    SELECT 
        p.id_producto,
        p.codigo,
        p.nombre,
        p.descripcion,
        c.nombre AS categoria,
        p.stock_actual,
        p.stock_minimo,
        p.precio_compra,
        p.precio_venta,
        dbo.fnCalcularMargenGanancia(p.id_producto) AS margen_ganancia,
        CASE 
            WHEN p.stock_actual <= p.stock_minimo THEN 'STOCK BAJO'
            WHEN p.stock_actual = 0 THEN 'AGOTADO'
            ELSE 'DISPONIBLE'
        END AS estado_stock
    FROM Productos p
    INNER JOIN Categorias c ON p.id_categoria = c.id_categoria
    WHERE p.estado = 1
    AND (@id_categoria = 0 OR p.id_categoria = @id_categoria)
)
GO

CREATE OR ALTER FUNCTION dbo.fnObtenerHistorialProducto(@id_producto INT, @fecha_inicio DATE, @fecha_fin DATE)
RETURNS TABLE
AS
RETURN (
    -- Entradas por recepciones
    SELECT 
        'ENTRADA' AS tipo_movimiento,
        r.fecha_recepcion AS fecha,
        'RECEPCIÓN' AS origen,
        rd.cantidad_aceptada AS cantidad,
        rd.cantidad_aceptada * p.precio_compra AS valor,
        CONCAT('Recepción #', r.id_recepcion) AS referencia
    FROM RecepcionesDetalle rd
    INNER JOIN Recepciones r ON rd.id_recepcion = r.id_recepcion
    INNER JOIN Productos p ON rd.id_producto = p.id_producto
    WHERE rd.id_producto = @id_producto
    AND r.estado = 'Aceptada'
    AND r.fecha_recepcion BETWEEN @fecha_inicio AND @fecha_fin
    
    UNION ALL
    
    -- Salidas por ventas
    SELECT 
        'SALIDA' AS tipo_movimiento,
        v.fecha_venta AS fecha,
        'VENTA' AS origen,
        vd.cantidad AS cantidad,
        vd.cantidad * vd.precio_unitario AS valor,
        CONCAT('Venta #', v.id_venta) AS referencia
    FROM VentasDetalle vd
    INNER JOIN Ventas v ON vd.id_venta = v.id_venta
    WHERE vd.id_producto = @id_producto
    AND v.estado = 'Pagada'
    AND v.fecha_venta BETWEEN @fecha_inicio AND @fecha_fin
    
    UNION ALL
    
    -- Salidas por elaboración (como materia prima)
    SELECT 
        'SALIDA' AS tipo_movimiento,
        ep.fecha_elaboracion AS fecha,
        'ELABORACIÓN' AS origen,
        ed.cantidad_utilizada AS cantidad,
        ed.cantidad_utilizada * p.precio_compra AS valor,
        CONCAT('Elaboración #', ep.id_elaboracion) AS referencia
    FROM ElaboracionDetalle ed
    INNER JOIN ElaboracionProductos ep ON ed.id_elaboracion = ep.id_elaboracion
    INNER JOIN Productos p ON ed.id_materia_prima = p.id_producto
    WHERE ed.id_materia_prima = @id_producto
    AND ep.estado = 'Completado'
    AND ep.fecha_elaboracion BETWEEN @fecha_inicio AND @fecha_fin
    
    UNION ALL
    
    -- Entradas por elaboración (como producto elaborado)
    SELECT 
        'ENTRADA' AS tipo_movimiento,
        ep.fecha_elaboracion AS fecha,
        'ELABORACIÓN' AS origen,
        ep.cantidad_elaborada AS cantidad,
        ep.cantidad_elaborada * p.precio_compra AS valor,
        CONCAT('Elaboración #', ep.id_elaboracion) AS referencia
    FROM ElaboracionProductos ep
    INNER JOIN Productos p ON ep.id_producto_elaborado = p.id_producto
    WHERE ep.id_producto_elaborado = @id_producto
    AND ep.estado = 'Completado'
    AND ep.fecha_elaboracion BETWEEN @fecha_inicio AND @fecha_fin
)
GO

CREATE OR ALTER FUNCTION dbo.fnObtenerVentasPorVendedor(@fecha_inicio DATE, @fecha_fin DATE)
RETURNS TABLE
AS
RETURN (
    SELECT 
        -- En un sistema real, aquí se uniría con la tabla de usuarios/vendedores
        'VENDEDOR_DEFAULT' AS vendedor,
        v.fecha_venta,
        v.id_venta,
        c.nombre AS cliente,
        v.tipo_venta,
        v.total,
        COUNT(vd.id_detalle_venta) AS cantidad_productos,
        SUM(vd.cantidad) AS total_unidades,
        dbo.fnCalcularComisionVenta(v.id_venta, 2.5) AS comision -- 2.5% de comisión
    FROM Ventas v
    INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
    INNER JOIN VentasDetalle vd ON v.id_venta = vd.id_venta
    WHERE v.estado = 'Pagada'
    AND v.fecha_venta BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY v.id_venta, v.fecha_venta, c.nombre, v.tipo_venta, v.total
)
GO

CREATE OR ALTER FUNCTION dbo.fnObtenerProductosReorden()
RETURNS TABLE
AS
RETURN (
    SELECT 
        p.id_producto,
        p.codigo,
        p.nombre,
        c.nombre AS categoria,
        p.stock_actual,
        p.stock_minimo,
        (p.stock_minimo - p.stock_actual) AS cantidad_faltante,
        p.precio_compra,
        p.precio_venta,
        dbo.fnCalcularMargenGanancia(p.id_producto) AS margen_ganancia,
        dbo.fnCalcularDiasInventario(p.id_producto) AS dias_inventario_restantes,
        CASE 
            WHEN p.stock_actual = 0 THEN 'URGENTE'
            WHEN p.stock_actual <= p.stock_minimo THEN 'ALERTA'
            ELSE 'NORMAL'
        END AS prioridad_reorden
    FROM Productos p
    INNER JOIN Categorias c ON p.id_categoria = c.id_categoria
    WHERE p.estado = 1
    AND p.stock_actual <= p.stock_minimo
)
GO

CREATE OR ALTER FUNCTION dbo.fnObtenerAnalisisABC(@fecha_inicio DATE, @fecha_fin DATE)
RETURNS TABLE
AS
RETURN (
    WITH VentasProducto AS (
        SELECT 
            vd.id_producto,
            p.nombre AS producto,
            SUM(vd.cantidad) AS cantidad_vendida,
            SUM(vd.subtotal) AS valor_ventas
        FROM VentasDetalle vd
        INNER JOIN Ventas v ON vd.id_venta = v.id_venta
        INNER JOIN Productos p ON vd.id_producto = p.id_producto
        WHERE v.estado = 'Pagada'
        AND v.fecha_venta BETWEEN @fecha_inicio AND @fecha_fin
        GROUP BY vd.id_producto, p.nombre
    ),
    AnalisisABC AS (
        SELECT 
            *,
            SUM(valor_ventas) OVER (ORDER BY valor_ventas DESC) * 1.0 / SUM(valor_ventas) OVER () AS porcentaje_acumulado,
            CASE 
                WHEN SUM(valor_ventas) OVER (ORDER BY valor_ventas DESC) * 1.0 / SUM(valor_ventas) OVER () <= 0.8 THEN 'A'
                WHEN SUM(valor_ventas) OVER (ORDER BY valor_ventas DESC) * 1.0 / SUM(valor_ventas) OVER () <= 0.95 THEN 'B'
                ELSE 'C'
            END AS categoria_abc
        FROM VentasProducto
    )
    SELECT 
        id_producto,
        producto,
        cantidad_vendida,
        valor_ventas,
        ROUND(porcentaje_acumulado * 100, 2) AS porcentaje_acumulado,
        categoria_abc
    FROM AnalisisABC
)
GO

