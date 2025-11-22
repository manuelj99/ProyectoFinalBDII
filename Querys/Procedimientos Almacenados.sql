use db20232000983
go

-- INSERTAR Proveedor
CREATE OR ALTER PROCEDURE sp_Proveedores_Insertar
    @nombre VARCHAR(100),
    @direccion VARCHAR(200),
    @telefono VARCHAR(20),
    @email VARCHAR(100),
    @limite_credito DECIMAL(15,2)
AS
BEGIN
    INSERT INTO Proveedores (nombre, direccion, telefono, email, limite_credito)
    VALUES (@nombre, @direccion, @telefono, @email, @limite_credito)
    
    SELECT SCOPE_IDENTITY() AS id_proveedor
END
GO

-- ACTUALIZAR Proveedor
CREATE OR ALTER PROCEDURE sp_Proveedores_Actualizar
    @id_proveedor INT,
    @nombre VARCHAR(100),
    @direccion VARCHAR(200),
    @telefono VARCHAR(20),
    @email VARCHAR(100),
    @limite_credito DECIMAL(15,2)
AS
BEGIN
    UPDATE Proveedores 
    SET nombre = @nombre,
        direccion = @direccion,
        telefono = @telefono,
        email = @email,
        limite_credito = @limite_credito
    WHERE id_proveedor = @id_proveedor
END
GO

-- ELIMINAR Proveedor (eliminación lógica)
CREATE OR ALTER PROCEDURE sp_Proveedores_Eliminar
    @id_proveedor INT
AS
BEGIN
    UPDATE Proveedores 
    SET estado = 0
    WHERE id_proveedor = @id_proveedor
END
GO

-- OBTENER Proveedor por ID
CREATE OR ALTER PROCEDURE sp_Proveedores_ObtenerPorId
    @id_proveedor INT
AS
BEGIN
    SELECT * FROM Proveedores 
    WHERE id_proveedor = @id_proveedor AND estado = 1
END
GO

-- OBTENER TODOS los Proveedores
CREATE OR ALTER PROCEDURE sp_Proveedores_ObtenerTodos
AS
BEGIN
    SELECT * FROM Proveedores WHERE estado = 1
END
GO

-- INSERTAR Producto
CREATE OR ALTER PROCEDURE sp_Productos_Insertar
    @codigo VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(500),
    @id_categoria INT,
    @tipo VARCHAR(20),
    @precio_compra DECIMAL(10,2),
    @precio_venta DECIMAL(10,2),
    @stock_minimo INT,
    @es_materia_prima BIT
AS
BEGIN
    INSERT INTO Productos (codigo, nombre, descripcion, id_categoria, tipo, 
                          precio_compra, precio_venta, stock_minimo, es_materia_prima)
    VALUES (@codigo, @nombre, @descripcion, @id_categoria, @tipo, 
            @precio_compra, @precio_venta, @stock_minimo, @es_materia_prima)
    
    SELECT SCOPE_IDENTITY() AS id_producto
END
GO

-- ACTUALIZAR Producto
CREATE OR ALTER PROCEDURE sp_Productos_Actualizar
    @id_producto INT,
    @codigo VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(500),
    @id_categoria INT,
    @tipo VARCHAR(20),
    @precio_compra DECIMAL(10,2),
    @precio_venta DECIMAL(10,2),
    @stock_minimo INT,
    @es_materia_prima BIT
AS
BEGIN
    UPDATE Productos 
    SET codigo = @codigo,
        nombre = @nombre,
        descripcion = @descripcion,
        id_categoria = @id_categoria,
        tipo = @tipo,
        precio_compra = @precio_compra,
        precio_venta = @precio_venta,
        stock_minimo = @stock_minimo,
        es_materia_prima = @es_materia_prima
    WHERE id_producto = @id_producto
END
GO

-- ELIMINAR Producto (eliminacion logica)
CREATE OR ALTER PROCEDURE sp_Productos_Eliminar @id_producto INT
AS
BEGIN
    UPDATE Productos
    SET estado = 0
    WHERE id_producto = @id_producto
END
GO

-- OBTENER Productos con stock bajo
CREATE OR ALTER PROCEDURE sp_Productos_ObtenerStockBajo
AS
BEGIN
    SELECT * FROM Productos 
    WHERE stock_actual <= stock_minimo AND estado = 1
END
GO

-- INSERTAR Orden de Compra (con manejo transaccional)
CREATE OR ALTER PROCEDURE sp_OrdenesCompra_Insertar
    @id_proveedor INT,
    @fecha_esperada DATE,
    @detalles NVARCHAR(MAX) -- JSON con los detalles: [{"id_producto":1, "cantidad":10, "precio_unitario":5.50}]
AS
BEGIN
    DECLARE @id_orden_compra INT
    DECLARE @total DECIMAL(15,2) = 0
    
    BEGIN TRANSACTION
    BEGIN TRY
        -- Insertar cabecera
        INSERT INTO OrdenesCompra (id_proveedor, fecha_orden, fecha_esperada, estado)
        VALUES (@id_proveedor, GETDATE(), @fecha_esperada, 'Pendiente')
        
        SET @id_orden_compra = SCOPE_IDENTITY()
        
        -- Procesar detalles desde JSON
        INSERT INTO OrdenesCompraDetalle (id_orden_compra, id_producto, cantidad, precio_unitario, subtotal)
        SELECT 
            @id_orden_compra,
            id_producto,
            cantidad,
            precio_unitario,
            cantidad * precio_unitario
        FROM OPENJSON(@detalles)
        WITH (
            id_producto INT '$.id_producto',
            cantidad INT '$.cantidad',
            precio_unitario DECIMAL(10,2) '$.precio_unitario'
        )
        
        -- Calcular total
        SELECT @total = SUM(subtotal) 
        FROM OrdenesCompraDetalle 
        WHERE id_orden_compra = @id_orden_compra
        
        -- Actualizar total en cabecera
        UPDATE OrdenesCompra SET total = @total 
        WHERE id_orden_compra = @id_orden_compra
        
        COMMIT TRANSACTION
        
        SELECT @id_orden_compra AS id_orden_compra, @total AS total
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al crear orden de compra: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Venta
CREATE OR ALTER PROCEDURE sp_Ventas_Insertar
    @id_cliente INT,
    @tipo_venta VARCHAR(20),
    @detalles NVARCHAR(MAX) -- JSON con detalles
AS
BEGIN
    DECLARE @id_venta INT
    DECLARE @total DECIMAL(15,2) = 0
    
    BEGIN TRANSACTION
    BEGIN TRY
        -- Verificar stock antes de insertar
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@detalles)
            WITH (
                id_producto INT '$.id_producto',
                cantidad INT '$.cantidad'
            ) d
            JOIN Productos p ON d.id_producto = p.id_producto
            WHERE p.stock_actual < d.cantidad
        )
        BEGIN
            RAISERROR('Stock insuficiente para uno o más productos', 16, 1)
            RETURN
        END
        
        -- Insertar cabecera
        INSERT INTO Ventas (id_cliente, fecha_venta, tipo_venta, estado)
        VALUES (@id_cliente, GETDATE(), @tipo_venta, 'Pendiente')
        
        SET @id_venta = SCOPE_IDENTITY()
        
        -- Insertar detalles
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
        )
        
        -- Actualizar stock de productos
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
        ) d ON p.id_producto = d.id_producto
        
        -- Calcular total
        SELECT @total = SUM(subtotal) 
        FROM VentasDetalle 
        WHERE id_venta = @id_venta
        
        -- Actualizar total en cabecera
        UPDATE Ventas SET total = @total 
        WHERE id_venta = @id_venta
        
        -- Si es venta al contado, actualizar estado
        IF @tipo_venta = 'Contado'
        BEGIN
            UPDATE Ventas SET estado = 'Pagada' WHERE id_venta = @id_venta
        END
        
        COMMIT TRANSACTION
        
        SELECT @id_venta AS id_venta, @total AS total
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al crear venta: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Cliente
CREATE OR ALTER PROCEDURE sp_Clientes_Insertar
    @nombre VARCHAR(100),
    @direccion VARCHAR(200),
    @telefono VARCHAR(20),
    @email VARCHAR(100),
    @limite_credito DECIMAL(15,2),
    @tipo_cliente VARCHAR(20)
AS
BEGIN
    INSERT INTO Clientes (nombre, direccion, telefono, email, limite_credito, tipo_cliente)
    VALUES (@nombre, @direccion, @telefono, @email, @limite_credito, @tipo_cliente)
    
    SELECT SCOPE_IDENTITY() AS id_cliente
END
GO

-- OBTENER saldo de cliente
CREATE OR ALTER PROCEDURE sp_Clientes_ObtenerSaldo
    @id_cliente INT
AS
BEGIN
    SELECT saldo_actual FROM Clientes WHERE id_cliente = @id_cliente
END
GO

-- ACTUALIZAR inventario después de recepción
CREATE OR ALTER PROCEDURE sp_Inventario_ActualizarStock
    @id_producto INT,
    @cantidad INT,
    @operacion VARCHAR(10) -- 'INCREMENTAR' o 'DECREMENTAR'
AS
BEGIN
    IF @operacion = 'INCREMENTAR'
    BEGIN
        UPDATE Productos 
        SET stock_actual = stock_actual + @cantidad 
        WHERE id_producto = @id_producto
    END
    ELSE IF @operacion = 'DECREMENTAR'
    BEGIN
        UPDATE Productos 
        SET stock_actual = stock_actual - @cantidad 
        WHERE id_producto = @id_producto
    END
END
GO

-- INSERTAR Cliente
CREATE OR ALTER PROCEDURE sp_Clientes_Insertar
    @nombre VARCHAR(100),
    @direccion VARCHAR(200),
    @telefono VARCHAR(20),
    @email VARCHAR(100),
    @limite_credito DECIMAL(15,2),
    @tipo_cliente VARCHAR(20)
AS
BEGIN
    INSERT INTO Clientes (nombre, direccion, telefono, email, limite_credito, tipo_cliente)
    VALUES (@nombre, @direccion, @telefono, @email, @limite_credito, @tipo_cliente)
    
    SELECT SCOPE_IDENTITY() AS id_cliente
END
GO

-- ACTUALIZAR Cliente
CREATE OR ALTER PROCEDURE sp_Clientes_Actualizar
    @id_cliente INT,
    @nombre VARCHAR(100),
    @direccion VARCHAR(200),
    @telefono VARCHAR(20),
    @email VARCHAR(100),
    @limite_credito DECIMAL(15,2),
    @tipo_cliente VARCHAR(20)
AS
BEGIN
    UPDATE Clientes 
    SET nombre = @nombre,
        direccion = @direccion,
        telefono = @telefono,
        email = @email,
        limite_credito = @limite_credito,
        tipo_cliente = @tipo_cliente
    WHERE id_cliente = @id_cliente
END
GO

-- ELIMINAR Cliente (eliminación lógica)
CREATE OR ALTER PROCEDURE sp_Clientes_Eliminar
    @id_cliente INT
AS
BEGIN
    UPDATE Clientes 
    SET estado = 0
    WHERE id_cliente = @id_cliente
END
GO

-- OBTENER Cliente por ID
CREATE OR ALTER PROCEDURE sp_Clientes_ObtenerPorId
    @id_cliente INT
AS
BEGIN
    SELECT * FROM Clientes 
    WHERE id_cliente = @id_cliente AND estado = 1
END
GO

-- OBTENER TODOS los Clientes
CREATE OR ALTER PROCEDURE sp_Clientes_ObtenerTodos
AS
BEGIN
    SELECT * FROM Clientes WHERE estado = 1
END
GO

-- OBTENER saldo de cliente
CREATE OR ALTER PROCEDURE sp_Clientes_ObtenerSaldo
    @id_cliente INT
AS
BEGIN
    SELECT saldo_actual FROM Clientes WHERE id_cliente = @id_cliente
END
GO

-- INSERTAR Banco
CREATE OR ALTER PROCEDURE sp_Bancos_Insertar
    @nombre_banco VARCHAR(100),
    @numero_cuenta VARCHAR(50),
    @saldo DECIMAL(15,2)
AS
BEGIN
    INSERT INTO Bancos (nombre_banco, numero_cuenta, saldo)
    VALUES (@nombre_banco, @numero_cuenta, @saldo)
    
    SELECT SCOPE_IDENTITY() AS id_banco
END
GO

-- ACTUALIZAR Banco
CREATE OR ALTER PROCEDURE sp_Bancos_Actualizar
    @id_banco INT,
    @nombre_banco VARCHAR(100),
    @numero_cuenta VARCHAR(50),
    @saldo DECIMAL(15,2)
AS
BEGIN
    UPDATE Bancos 
    SET nombre_banco = @nombre_banco,
        numero_cuenta = @numero_cuenta,
        saldo = @saldo
    WHERE id_banco = @id_banco
END
GO

-- ELIMINAR Banco (eliminación lógica)
CREATE OR ALTER PROCEDURE sp_Bancos_Eliminar
    @id_banco INT
AS
BEGIN
    UPDATE Bancos 
    SET estado = 0
    WHERE id_banco = @id_banco
END
GO

-- OBTENER Banco por ID
CREATE OR ALTER PROCEDURE sp_Bancos_ObtenerPorId
    @id_banco INT
AS
BEGIN
    SELECT * FROM Bancos 
    WHERE id_banco = @id_banco AND estado = 1
END
GO

-- OBTENER TODOS los Bancos
CREATE OR ALTER PROCEDURE sp_Bancos_ObtenerTodos
AS
BEGIN
    SELECT * FROM Bancos WHERE estado = 1
END
GO

-- INSERTAR Categoría
CREATE OR ALTER PROCEDURE sp_Categorias_Insertar
    @nombre VARCHAR(100),
    @descripcion VARCHAR(200)
AS
BEGIN
    INSERT INTO Categorias (nombre, descripcion)
    VALUES (@nombre, @descripcion)
    
    SELECT SCOPE_IDENTITY() AS id_categoria
END
GO

-- ACTUALIZAR Categoría
CREATE OR ALTER PROCEDURE sp_Categorias_Actualizar
    @id_categoria INT,
    @nombre VARCHAR(100),
    @descripcion VARCHAR(200)
AS
BEGIN
    UPDATE Categorias 
    SET nombre = @nombre,
        descripcion = @descripcion
    WHERE id_categoria = @id_categoria
END
GO

-- ELIMINAR Categoría
CREATE OR ALTER PROCEDURE sp_Categorias_Eliminar
    @id_categoria INT
AS
BEGIN
    DELETE FROM Categorias WHERE id_categoria = @id_categoria
END
GO

-- OBTENER Categoría por ID
CREATE OR ALTER PROCEDURE sp_Categorias_ObtenerPorId
    @id_categoria INT
AS
BEGIN
    SELECT * FROM Categorias WHERE id_categoria = @id_categoria
END
GO

-- OBTENER TODAS las Categorías
CREATE OR ALTER PROCEDURE sp_Categorias_ObtenerTodos
AS
BEGIN
    SELECT * FROM Categorias
END
GO

-- INSERTAR Bodega
CREATE OR ALTER PROCEDURE sp_Bodegas_Insertar
    @nombre VARCHAR(100),
    @ubicacion VARCHAR(200),
    @capacidad INT
AS
BEGIN
    INSERT INTO Bodegas (nombre, ubicacion, capacidad)
    VALUES (@nombre, @ubicacion, @capacidad)
    
    SELECT SCOPE_IDENTITY() AS id_bodega
END
GO

-- ACTUALIZAR Bodega
CREATE OR ALTER PROCEDURE sp_Bodegas_Actualizar
    @id_bodega INT,
    @nombre VARCHAR(100),
    @ubicacion VARCHAR(200),
    @capacidad INT
AS
BEGIN
    UPDATE Bodegas 
    SET nombre = @nombre,
        ubicacion = @ubicacion,
        capacidad = @capacidad
    WHERE id_bodega = @id_bodega
END
GO

-- ELIMINAR Bodega (eliminación lógica)
CREATE OR ALTER PROCEDURE sp_Bodegas_Eliminar
    @id_bodega INT
AS
BEGIN
    UPDATE Bodegas 
    SET estado = 0
    WHERE id_bodega = @id_bodega
END
GO

-- OBTENER Bodega por ID
CREATE OR ALTER PROCEDURE sp_Bodegas_ObtenerPorId
    @id_bodega INT
AS
BEGIN
    SELECT * FROM Bodegas 
    WHERE id_bodega = @id_bodega AND estado = 1
END
GO

-- OBTENER TODAS las Bodegas
CREATE OR ALTER PROCEDURE sp_Bodegas_ObtenerTodos
AS
BEGIN
    SELECT * FROM Bodegas WHERE estado = 1
END
GO

-- INSERTAR Recepción (con manejo transaccional)
CREATE OR ALTER PROCEDURE sp_Recepciones_Insertar
    @id_orden_compra INT,
    @id_proveedor INT,
    @id_bodega INT,
    @observaciones VARCHAR(500),
    @detalles NVARCHAR(MAX) -- JSON con detalles de recepción
AS
BEGIN
    DECLARE @id_recepcion INT
    
    BEGIN TRANSACTION
    BEGIN TRY
        -- Insertar cabecera de recepción
        INSERT INTO Recepciones (id_orden_compra, fecha_recepcion, id_proveedor, id_bodega, observaciones, estado)
        VALUES (@id_orden_compra, GETDATE(), @id_proveedor, @id_bodega, @observaciones, 'Pendiente')
        
        SET @id_recepcion = SCOPE_IDENTITY()
        
        -- Insertar detalles de recepción
        INSERT INTO RecepcionesDetalle (id_recepcion, id_producto, cantidad_solicitada, cantidad_recibida, cantidad_aceptada, cantidad_rechazada)
        SELECT 
            @id_recepcion,
            id_producto,
            cantidad_solicitada,
            cantidad_recibida,
            CASE WHEN cantidad_recibida <= cantidad_solicitada THEN cantidad_recibida ELSE cantidad_solicitada END,
            CASE WHEN cantidad_recibida > cantidad_solicitada THEN cantidad_recibida - cantidad_solicitada ELSE 0 END
        FROM OPENJSON(@detalles)
        WITH (
            id_producto INT '$.id_producto',
            cantidad_solicitada INT '$.cantidad_solicitada',
            cantidad_recibida INT '$.cantidad_recibida'
        )
        
        -- Actualizar estado de orden de compra si toda la mercadería fue recibida
        UPDATE OrdenesCompra 
        SET estado = 'Recibida'
        WHERE id_orden_compra = @id_orden_compra
        
        COMMIT TRANSACTION
        
        SELECT @id_recepcion AS id_recepcion
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al crear recepción: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- ACEPTAR Recepción y actualizar inventario
CREATE OR ALTER PROCEDURE sp_Recepciones_Aceptar
    @id_recepcion INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Actualizar stock de productos
        UPDATE p
        SET p.stock_actual = p.stock_actual + rd.cantidad_aceptada
        FROM Productos p
        INNER JOIN RecepcionesDetalle rd ON p.id_producto = rd.id_producto
        WHERE rd.id_recepcion = @id_recepcion
        
        -- Actualizar estado de recepción
        UPDATE Recepciones SET estado = 'Aceptada' WHERE id_recepcion = @id_recepcion
        
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al aceptar recepción: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Pago a Proveedor
CREATE OR ALTER PROCEDURE sp_PagosProveedores_Insertar
    @id_proveedor INT,
    @id_banco INT,
    @monto DECIMAL(15,2),
    @tipo_pago VARCHAR(20),
    @numero_documento VARCHAR(50)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Insertar pago
        INSERT INTO PagosProveedores (id_proveedor, id_banco, fecha_pago, monto, tipo_pago, numero_documento, estado)
        VALUES (@id_proveedor, @id_banco, GETDATE(), @monto, @tipo_pago, @numero_documento, 'Aplicado')
        
        -- Actualizar saldo del proveedor
        UPDATE Proveedores 
        SET saldo_actual = saldo_actual - @monto
        WHERE id_proveedor = @id_proveedor
        
        -- Actualizar saldo del banco
        UPDATE Bancos 
        SET saldo = saldo - @monto
        WHERE id_banco = @id_banco
        
        COMMIT TRANSACTION
        
        SELECT SCOPE_IDENTITY() AS id_pago_proveedor
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al registrar pago: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Pago de Cliente
CREATE OR ALTER PROCEDURE sp_PagosClientes_Insertar
    @id_cliente INT,
    @monto DECIMAL(15,2)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Insertar pago
        INSERT INTO PagosClientes (id_cliente, fecha_pago, monto, estado)
        VALUES (@id_cliente, GETDATE(), @monto, 'Aplicado')
        
        -- Actualizar saldo del cliente
        UPDATE Clientes 
        SET saldo_actual = saldo_actual - @monto
        WHERE id_cliente = @id_cliente
        
        COMMIT TRANSACTION
        
        SELECT SCOPE_IDENTITY() AS id_pago_cliente
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al registrar pago de cliente: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Elaboración de Producto
CREATE OR ALTER PROCEDURE sp_ElaboracionProductos_Insertar
    @id_producto_elaborado INT,
    @cantidad_elaborada INT,
    @detalles NVARCHAR(MAX) -- JSON con materias primas
AS
BEGIN
    DECLARE @id_elaboracion INT
    
    BEGIN TRANSACTION
    BEGIN TRY
        -- Verificar stock de materias primas
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@detalles)
            WITH (
                id_materia_prima INT '$.id_materia_prima',
                cantidad_utilizada INT '$.cantidad_utilizada'
            ) d
            JOIN Productos p ON d.id_materia_prima = p.id_producto
            WHERE p.stock_actual < d.cantidad_utilizada
        )
        BEGIN
            RAISERROR('Stock insuficiente de materias primas', 16, 1)
            RETURN
        END
        
        -- Insertar cabecera de elaboración
        INSERT INTO ElaboracionProductos (fecha_elaboracion, id_producto_elaborado, cantidad_elaborada, estado)
        VALUES (GETDATE(), @id_producto_elaborado, @cantidad_elaborada, 'Completado')
        
        SET @id_elaboracion = SCOPE_IDENTITY()
        
        -- Insertar detalles de elaboración
        INSERT INTO ElaboracionDetalle (id_elaboracion, id_materia_prima, cantidad_utilizada)
        SELECT 
            @id_elaboracion,
            id_materia_prima,
            cantidad_utilizada
        FROM OPENJSON(@detalles)
        WITH (
            id_materia_prima INT '$.id_materia_prima',
            cantidad_utilizada INT '$.cantidad_utilizada'
        )
        
        -- Descontar materias primas del inventario
        UPDATE p
        SET p.stock_actual = p.stock_actual - d.cantidad_utilizada
        FROM Productos p
        INNER JOIN (
            SELECT id_materia_prima, cantidad_utilizada
            FROM OPENJSON(@detalles)
            WITH (
                id_materia_prima INT '$.id_materia_prima',
                cantidad_utilizada INT '$.cantidad_utilizada'
            )
        ) d ON p.id_producto = d.id_materia_prima
        
        -- Aumentar stock del producto elaborado
        UPDATE Productos 
        SET stock_actual = stock_actual + @cantidad_elaborada
        WHERE id_producto = @id_producto_elaborado
        
        COMMIT TRANSACTION
        
        SELECT @id_elaboracion AS id_elaboracion
        
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error en elaboración de producto: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Devolución a Proveedor
CREATE OR ALTER PROCEDURE sp_DevolucionesProveedores_Insertar
    @id_proveedor INT,
    @id_producto INT,
    @cantidad INT,
    @motivo VARCHAR(200)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        -- Verificar stock disponible
        IF (SELECT stock_actual FROM Productos WHERE id_producto = @id_producto) < @cantidad
        BEGIN
            RAISERROR('Stock insuficiente para devolución', 16, 1)
            RETURN
        END
        
        -- Insertar devolución
        INSERT INTO DevolucionesProveedores (id_proveedor, id_producto, fecha_devolucion, cantidad, motivo)
        VALUES (@id_proveedor, @id_producto, GETDATE(), @cantidad, @motivo)
        
        -- Actualizar stock
        UPDATE Productos 
        SET stock_actual = stock_actual - @cantidad
        WHERE id_producto = @id_producto
        
        COMMIT TRANSACTION
        
        SELECT SCOPE_IDENTITY() AS id_devolucion
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR('Error al registrar devolución: %s', 16, 1, @ErrorMessage)
    END CATCH
END
GO

-- INSERTAR Arqueo de Caja
CREATE OR ALTER PROCEDURE sp_ArqueosCaja_Insertar
    @total_ventas_contado DECIMAL(15,2),
    @total_pagos_recibidos DECIMAL(15,2),
    @total_depositado DECIMAL(15,2),
    @observaciones VARCHAR(200)
AS
BEGIN
    DECLARE @diferencia DECIMAL(15,2)
    SET @diferencia = (@total_ventas_contado + @total_pagos_recibidos) - @total_depositado
    
    INSERT INTO ArqueosCaja (fecha_arqueo, total_ventas_contado, total_pagos_recibidos, total_depositado, diferencia, observaciones)
    VALUES (GETDATE(), @total_ventas_contado, @total_pagos_recibidos, @total_depositado, @diferencia, @observaciones)
    
    SELECT SCOPE_IDENTITY() AS id_arqueo
END
GO