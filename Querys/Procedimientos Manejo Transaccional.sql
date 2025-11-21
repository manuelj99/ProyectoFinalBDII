use db20232000983
go

CREATE OR ALTER PROCEDURE spTransaccionVentaCompleta
    @id_cliente INT,
    @tipo_venta VARCHAR(20),
    @detalles NVARCHAR(MAX), -- JSON con detalles de venta
    @id_usuario INT = 1 -- Usuario que realiza la venta
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id_venta INT;
    DECLARE @total_venta DECIMAL(15,2) = 0;
    DECLARE @error_message NVARCHAR(4000);
    DECLARE @error_severity INT;
    DECLARE @error_state INT;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. VALIDACIONES INICIALES
        -- Verificar que el cliente existe y está activo
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE id_cliente = @id_cliente AND estado = 1)
        BEGIN
            RAISERROR('El cliente no existe o está inactivo.', 16, 1);
        END

        -- Verificar límite de crédito si es venta a crédito
        IF @tipo_venta = 'Credito'
        BEGIN
            DECLARE @saldo_actual DECIMAL(15,2);
            DECLARE @limite_credito DECIMAL(15,2);
            
            SELECT @saldo_actual = saldo_actual, @limite_credito = limite_credito
            FROM Clientes 
            WHERE id_cliente = @id_cliente;
            
            -- Calcular total temporal de la venta
            SELECT @total_venta = SUM(cantidad * precio_unitario)
            FROM OPENJSON(@detalles)
            WITH (
                id_producto INT '$.id_producto',
                cantidad INT '$.cantidad',
                precio_unitario DECIMAL(10,2) '$.precio_unitario'
            );
            
            IF (@saldo_actual + @total_venta) > @limite_credito
            BEGIN
                RAISERROR('El cliente ha excedido su límite de crédito.', 16, 1);
            END
        END

        -- 2. INSERTAR CABECERA DE VENTA
        INSERT INTO Ventas (id_cliente, fecha_venta, tipo_venta, estado)
        VALUES (@id_cliente, GETDATE(), @tipo_venta, 
                CASE WHEN @tipo_venta = 'Contado' THEN 'Pagada' ELSE 'Pendiente' END);
        
        SET @id_venta = SCOPE_IDENTITY();

        -- 3. INSERTAR DETALLES DE VENTA Y ACTUALIZAR STOCK
        INSERT INTO VentasDetalle (id_venta, id_producto, cantidad, precio_unitario, subtotal)
        SELECT 
            @id_venta,
            id_producto,
            cantidad,
            precio_unitario,
            cantidad * precio_unitario
        FROM OPENJSON(@detalles)
        WITH (
            id_producto INT '$.id_producto',
            cantidad INT '$.cantidad',
            precio_unitario DECIMAL(10,2) '$.precio_unitario'
        );

        -- 4. ACTUALIZAR STOCK DE PRODUCTOS
        UPDATE p
        SET p.stock_actual = p.stock_actual - d.cantidad
        FROM Productos p
        INNER JOIN (
            SELECT id_producto, cantidad
            FROM OPENJSON(@detalles)
            WITH (
                id_producto INT '$.id_producto',
                cantidad INT '$.cantidad'
            )
        ) d ON p.id_producto = d.id_producto;

        -- 5. ACTUALIZAR SALDO DEL CLIENTE SI ES VENTA A CRÉDITO
        IF @tipo_venta = 'Credito'
        BEGIN
            UPDATE Clientes 
            SET saldo_actual = saldo_actual + @total_venta
            WHERE id_cliente = @id_cliente;
        END

        -- 6. CALCULAR Y ACTUALIZAR TOTAL DE VENTA
        SELECT @total_venta = SUM(subtotal) 
        FROM VentasDetalle 
        WHERE id_venta = @id_venta;
        
        UPDATE Ventas SET total = @total_venta WHERE id_venta = @id_venta;

        -- 7. CONFIRMAR TRANSACCIÓN
        COMMIT TRANSACTION;

        -- RETORNAR RESULTADO
        SELECT 
            'Éxito' AS resultado,
            @id_venta AS id_venta,
            @total_venta AS total,
            'Venta registrada correctamente' AS mensaje;

    END TRY
    BEGIN CATCH
        -- DESHACER TRANSACCIÓN EN CASO DE ERROR
        ROLLBACK TRANSACTION;
        
        -- OBTENER INFORMACIÓN DEL ERROR
        SELECT 
            @error_message = ERROR_MESSAGE(),
            @error_severity = ERROR_SEVERITY(),
            @error_state = ERROR_STATE();
        
        -- RETORNAR ERROR
        SELECT 
            'Error' AS resultado,
            NULL AS id_venta,
            0 AS total,
            @error_message AS mensaje;
        
        -- RELANZAR ERROR PARA REGISTRO EN LOGS
        RAISERROR(@error_message, @error_severity, @error_state);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE spTransaccionRecepcionCompleta
    @id_orden_compra INT,
    @id_bodega INT,
    @observaciones VARCHAR(500),
    @detalles NVARCHAR(MAX) -- JSON con detalles de recepción
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id_recepcion INT;
    DECLARE @id_proveedor INT;
    DECLARE @error_message NVARCHAR(4000);
    DECLARE @error_severity INT;
    DECLARE @error_state INT;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. VALIDACIONES INICIALES
        -- Verificar que la orden de compra existe y está aprobada
        IF NOT EXISTS (SELECT 1 FROM OrdenesCompra WHERE id_orden_compra = @id_orden_compra AND estado = 'Aprobada')
        BEGIN
            RAISERROR('La orden de compra no existe o no está aprobada.', 16, 1);
        END

        -- Verificar que la bodega existe
        IF NOT EXISTS (SELECT 1 FROM Bodegas WHERE id_bodega = @id_bodega AND estado = 1)
        BEGIN
            RAISERROR('La bodega no existe o está inactiva.', 16, 1);
        END

        -- Obtener proveedor de la orden de compra
        SELECT @id_proveedor = id_proveedor 
        FROM OrdenesCompra 
        WHERE id_orden_compra = @id_orden_compra;

        -- 2. INSERTAR CABECERA DE RECEPCIÓN
        INSERT INTO Recepciones (id_orden_compra, fecha_recepcion, id_proveedor, id_bodega, observaciones, estado)
        VALUES (@id_orden_compra, GETDATE(), @id_proveedor, @id_bodega, @observaciones, 'Aceptada');
        
        SET @id_recepcion = SCOPE_IDENTITY();

        -- 3. INSERTAR DETALLES DE RECEPCIÓN
        INSERT INTO RecepcionesDetalle (id_recepcion, id_producto, cantidad_solicitada, cantidad_recibida, cantidad_aceptada, cantidad_rechazada)
        SELECT 
            @id_recepcion,
            id_producto,
            cantidad_solicitada,
            cantidad_recibida,
            -- Lógica de aceptación: aceptar hasta la cantidad solicitada
            CASE WHEN cantidad_recibida <= cantidad_solicitada THEN cantidad_recibida ELSE cantidad_solicitada END,
            -- Rechazar el excedente
            CASE WHEN cantidad_recibida > cantidad_solicitada THEN cantidad_recibida - cantidad_solicitada ELSE 0 END
        FROM OPENJSON(@detalles)
        WITH (
            id_producto INT '$.id_producto',
            cantidad_solicitada INT '$.cantidad_solicitada',
            cantidad_recibida INT '$.cantidad_recibida'
        );

        -- 4. ACTUALIZAR STOCK DE PRODUCTOS
        UPDATE p
        SET p.stock_actual = p.stock_actual + rd.cantidad_aceptada
        FROM Productos p
        INNER JOIN RecepcionesDetalle rd ON p.id_producto = rd.id_producto
        WHERE rd.id_recepcion = @id_recepcion;

        -- 5. ACTUALIZAR INVENTARIO POR BODEGA
        MERGE InventarioBodega AS target
        USING (
            SELECT 
                @id_bodega AS id_bodega,
                rd.id_producto,
                rd.cantidad_aceptada
            FROM RecepcionesDetalle rd
            WHERE rd.id_recepcion = @id_recepcion
        ) AS source (id_bodega, id_producto, cantidad_aceptada)
        ON (target.id_bodega = source.id_bodega AND target.id_producto = source.id_producto)
        WHEN MATCHED THEN
            UPDATE SET target.cantidad = target.cantidad + source.cantidad_aceptada
        WHEN NOT MATCHED THEN
            INSERT (id_bodega, id_producto, cantidad)
            VALUES (source.id_bodega, source.id_producto, source.cantidad_aceptada);

        -- 6. ACTUALIZAR ESTADO DE ORDEN DE COMPRA
        UPDATE OrdenesCompra 
        SET estado = 'Recibida' 
        WHERE id_orden_compra = @id_orden_compra;

        -- 7. CONFIRMAR TRANSACCIÓN
        COMMIT TRANSACTION;

        -- RETORNAR RESULTADO
        SELECT 
            'Éxito' AS resultado,
            @id_recepcion AS id_recepcion,
            'Recepción completada correctamente' AS mensaje;

    END TRY
    BEGIN CATCH
        -- DESHACER TRANSACCIÓN EN CASO DE ERROR
        ROLLBACK TRANSACTION;
        
        -- OBTENER INFORMACIÓN DEL ERROR
        SELECT 
            @error_message = ERROR_MESSAGE(),
            @error_severity = ERROR_SEVERITY(),
            @error_state = ERROR_STATE();
        
        -- RETORNAR ERROR
        SELECT 
            'Error' AS resultado,
            NULL AS id_recepcion,
            @error_message AS mensaje;
        
        -- RELANZAR ERROR
        RAISERROR(@error_message, @error_severity, @error_state);
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE spTransaccionPagoProveedores
    @id_proveedor INT,
    @id_banco INT,
    @monto DECIMAL(15,2),
    @tipo_pago VARCHAR(20),
    @numero_documento VARCHAR(50),
    @facturas NVARCHAR(MAX) = NULL -- JSON opcional con facturas a pagar
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id_pago_proveedor INT;
    DECLARE @saldo_actual_proveedor DECIMAL(15,2);
    DECLARE @saldo_banco DECIMAL(15,2);
    DECLARE @error_message NVARCHAR(4000);
    DECLARE @error_severity INT;
    DECLARE @error_state INT;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. VALIDACIONES INICIALES
        -- Verificar que el proveedor existe
        IF NOT EXISTS (SELECT 1 FROM Proveedores WHERE id_proveedor = @id_proveedor AND estado = 1)
        BEGIN
            RAISERROR('El proveedor no existe o está inactivo.', 16, 1);
        END

        -- Verificar que el banco existe
        IF NOT EXISTS (SELECT 1 FROM Bancos WHERE id_banco = @id_banco AND estado = 1)
        BEGIN
            RAISERROR('El banco no existe o está inactivo.', 16, 1);
        END

        -- Verificar saldo del proveedor
        SELECT @saldo_actual_proveedor = saldo_actual 
        FROM Proveedores 
        WHERE id_proveedor = @id_proveedor;

        IF @saldo_actual_proveedor < @monto
        BEGIN
            RAISERROR('El saldo del proveedor es insuficiente para realizar el pago.', 16, 1);
        END

        -- Verificar fondos del banco
        SELECT @saldo_banco = saldo 
        FROM Bancos 
        WHERE id_banco = @id_banco;

        IF @saldo_banco < @monto
        BEGIN
            RAISERROR('El banco no tiene fondos suficientes para realizar el pago.', 16, 1);
        END

        -- 2. INSERTAR REGISTRO DE PAGO
        INSERT INTO PagosProveedores (id_proveedor, id_banco, fecha_pago, monto, tipo_pago, numero_documento, estado)
        VALUES (@id_proveedor, @id_banco, GETDATE(), @monto, @tipo_pago, @numero_documento, 'Aplicado');
        
        SET @id_pago_proveedor = SCOPE_IDENTITY();

        -- 3. ACTUALIZAR SALDO DEL PROVEEDOR
        UPDATE Proveedores 
        SET saldo_actual = saldo_actual - @monto 
        WHERE id_proveedor = @id_proveedor;

        -- 4. ACTUALIZAR SALDO DEL BANCO
        UPDATE Bancos 
        SET saldo = saldo - @monto 
        WHERE id_banco = @id_banco;

        -- 5. PROCESAR FACTURAS ESPECÍFICAS SI SE PROPORCIONAN
        IF @facturas IS NOT NULL
        BEGIN
            -- Aquí se podría implementar lógica para aplicar el pago a facturas específicas
            -- Actualizar estado de facturas pagadas, etc.
            PRINT 'Procesando aplicación de pago a facturas específicas...';
        END

        -- 6. CONFIRMAR TRANSACCIÓN
        COMMIT TRANSACTION;

        -- RETORNAR RESULTADO
        SELECT 
            'Éxito' AS resultado,
            @id_pago_proveedor AS id_pago_proveedor,
            @monto AS monto_pagado,
            'Pago registrado correctamente' AS mensaje;

    END TRY
    BEGIN CATCH
        -- DESHACER TRANSACCIÓN EN CASO DE ERROR
        ROLLBACK TRANSACTION;
        
        -- OBTENER INFORMACIÓN DEL ERROR
        SELECT 
            @error_message = ERROR_MESSAGE(),
            @error_severity = ERROR_SEVERITY(),
            @error_state = ERROR_STATE();
        
        -- RETORNAR ERROR
        SELECT 
            'Error' AS resultado,
            NULL AS id_pago_proveedor,
            0 AS monto_pagado,
            @error_message AS mensaje;
        
        -- RELANZAR ERROR
        RAISERROR(@error_message, @error_severity, @error_state);
    END CATCH
END
GO