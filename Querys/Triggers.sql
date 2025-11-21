use db20232000983
go

CREATE OR ALTER TRIGGER trVentaActualizarStock
ON VentasDetalle
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE p
    SET p.stock_actual = p.stock_actual - i.cantidad
    FROM Productos p
    INNER JOIN inserted i ON p.id_producto = i.id_producto
    INNER JOIN Ventas v ON i.id_venta = v.id_venta
    WHERE v.estado != 'Cancelada';
END
GO

CREATE OR ALTER TRIGGER trVentaActualizarSaldoCliente
ON Ventas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE c
    SET c.saldo_actual = c.saldo_actual + i.total
    FROM Clientes c
    INNER JOIN inserted i ON c.id_cliente = i.id_cliente
    WHERE i.tipo_venta = 'Credito' AND i.estado != 'Cancelada';
END
GO

CREATE OR ALTER TRIGGER trVentaValidarLimiteCredito
ON Ventas
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si algún cliente excede su límite de crédito
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Clientes c ON i.id_cliente = c.id_cliente
        WHERE i.tipo_venta = 'Credito' 
        AND (c.saldo_actual + i.total) > c.limite_credito
    )
    BEGIN
        RAISERROR('El cliente ha excedido su límite de crédito', 16, 1);
        RETURN;
    END
    
    -- Si pasa la validación, insertar
    INSERT INTO Ventas (id_cliente, fecha_venta, tipo_venta, total, estado)
    SELECT id_cliente, fecha_venta, tipo_venta, total, estado
    FROM inserted;
END
GO

CREATE OR ALTER TRIGGER trOrdenCompraActualizarSaldoProveedor
ON OrdenesCompra
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE p
    SET p.saldo_actual = p.saldo_actual + i.total
    FROM Proveedores p
    INNER JOIN inserted i ON p.id_proveedor = i.id_proveedor
    WHERE i.estado != 'Cancelada';
END
GO

CREATE OR ALTER TRIGGER trOrdenCompraValidarLimiteProveedor
ON OrdenesCompra
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si algún proveedor excede su límite de crédito
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Proveedores p ON i.id_proveedor = p.id_proveedor
        WHERE (p.saldo_actual + i.total) > p.limite_credito
    )
    BEGIN
        RAISERROR('El proveedor ha excedido su límite de crédito', 16, 1);
        RETURN;
    END
    
    -- Si pasa la validación, insertar
    INSERT INTO OrdenesCompra (id_proveedor, fecha_orden, fecha_esperada, estado, total, observaciones)
    SELECT id_proveedor, fecha_orden, fecha_esperada, estado, total, observaciones
    FROM inserted;
END
GO

CREATE OR ALTER TRIGGER trRecepcionActualizarStock
ON Recepciones
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Solo actualizar stock cuando la recepción cambia a 'Aceptada'
    IF UPDATE(estado)
    BEGIN
        UPDATE p
        SET p.stock_actual = p.stock_actual + rd.cantidad_aceptada
        FROM Productos p
        INNER JOIN RecepcionesDetalle rd ON p.id_producto = rd.id_producto
        INNER JOIN inserted i ON rd.id_recepcion = i.id_recepcion
        INNER JOIN deleted d ON i.id_recepcion = d.id_recepcion
        WHERE i.estado = 'Aceptada' AND d.estado != 'Aceptada';
    END
END
GO

CREATE OR ALTER TRIGGER trRecepcionActualizarInventarioBodega
ON RecepcionesDetalle
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Actualizar o insertar en InventarioBodega
    MERGE InventarioBodega AS target
    USING (
        SELECT 
            i.id_recepcion,
            r.id_bodega,
            i.id_producto,
            i.cantidad_aceptada
        FROM inserted i
        INNER JOIN Recepciones r ON i.id_recepcion = r.id_recepcion
        WHERE r.estado = 'Aceptada'
    ) AS source (id_recepcion, id_bodega, id_producto, cantidad_aceptada)
    ON (target.id_bodega = source.id_bodega AND target.id_producto = source.id_producto)
    WHEN MATCHED THEN
        UPDATE SET target.cantidad = target.cantidad + source.cantidad_aceptada
    WHEN NOT MATCHED THEN
        INSERT (id_bodega, id_producto, cantidad)
        VALUES (source.id_bodega, source.id_producto, source.cantidad_aceptada);
END
GO

--Si no se ha creado, ejecutar la creacion de la tabla
--CREATE TABLE AuditoriaProductos (
--    id_auditoria INT IDENTITY(1,1) PRIMARY KEY,
--    id_producto INT NOT NULL,
--    usuario VARCHAR(100),
--    fecha_cambio DATETIME DEFAULT GETDATE(),
--    campo_afectado VARCHAR(100),
--    valor_anterior VARCHAR(500),
--    valor_nuevo VARCHAR(500),
--    operacion VARCHAR(10)
--)
--GO

CREATE OR ALTER TRIGGER trProductosAuditoria
ON Productos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Auditoría para precio_compra
    IF UPDATE(precio_compra)
    BEGIN
        INSERT INTO AuditoriaProductos (id_producto, usuario, campo_afectado, valor_anterior, valor_nuevo, operacion)
        SELECT 
            i.id_producto,
            SUSER_NAME(),
            'precio_compra',
            CAST(d.precio_compra AS VARCHAR(500)),
            CAST(i.precio_compra AS VARCHAR(500)),
            'UPDATE'
        FROM inserted i
        INNER JOIN deleted d ON i.id_producto = d.id_producto
        WHERE i.precio_compra != d.precio_compra;
    END
    
    -- Auditoría para precio_venta
    IF UPDATE(precio_venta)
    BEGIN
        INSERT INTO AuditoriaProductos (id_producto, usuario, campo_afectado, valor_anterior, valor_nuevo, operacion)
        SELECT 
            i.id_producto,
            SUSER_NAME(),
            'precio_venta',
            CAST(d.precio_venta AS VARCHAR(500)),
            CAST(i.precio_venta AS VARCHAR(500)),
            'UPDATE'
        FROM inserted i
        INNER JOIN deleted d ON i.id_producto = d.id_producto
        WHERE i.precio_venta != d.precio_venta;
    END
END
GO

CREATE OR ALTER TRIGGER trVentaValidarStock
ON VentasDetalle
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar stock suficiente
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        INNER JOIN Productos p ON i.id_producto = p.id_producto
        WHERE p.stock_actual < i.cantidad
    )
    BEGIN
        RAISERROR('Stock insuficiente para uno o más productos', 16, 1);
        RETURN;
    END
    
    -- Si hay stock suficiente, insertar
    INSERT INTO VentasDetalle (id_venta, id_producto, cantidad, precio_unitario, subtotal)
    SELECT id_venta, id_producto, cantidad, precio_unitario, subtotal
    FROM inserted;
END
GO

CREATE OR ALTER TRIGGER trRecepcionActualizarEstadoOrden
ON Recepciones
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cuando una recepción es aceptada, marcar la orden como recibida
    IF UPDATE(estado)
    BEGIN
        UPDATE oc
        SET oc.estado = 'Recibida'
        FROM OrdenesCompra oc
        INNER JOIN inserted i ON oc.id_orden_compra = i.id_orden_compra
        WHERE i.estado = 'Aceptada';
    END
END
GO

CREATE OR ALTER TRIGGER trPagoClienteActualizarSaldo
ON PagosClientes
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE c
    SET c.saldo_actual = c.saldo_actual - i.monto
    FROM Clientes c
    INNER JOIN inserted i ON c.id_cliente = i.id_cliente
    WHERE i.estado = 'Aplicado';
END
GO

CREATE OR ALTER TRIGGER trPagoProveedorActualizarSaldo
ON PagosProveedores
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE p
    SET p.saldo_actual = p.saldo_actual - i.monto
    FROM Proveedores p
    INNER JOIN inserted i ON p.id_proveedor = i.id_proveedor
    WHERE i.estado = 'Aplicado';
END
GO