use db20232000983
go

CREATE OR ALTER VIEW vSaldoProveedores AS
SELECT 
    p.id_proveedor,
    p.nombre AS proveedor,
    p.limite_credito,
    p.saldo_actual,
    (p.limite_credito - p.saldo_actual) AS credito_disponible,
    CASE 
        WHEN p.saldo_actual > (p.limite_credito * 0.8) THEN 'Alto'
        WHEN p.saldo_actual > (p.limite_credito * 0.5) THEN 'Medio'
        ELSE 'Bajo'
    END AS nivel_riesgo
FROM Proveedores p
WHERE p.estado = 1;
GO

CREATE OR ALTER VIEW vExistenciasPorBodega AS
SELECT 
    b.nombre AS bodega,
    p.codigo,
    p.nombre AS producto,
    cat.nombre AS categoria,
    ib.cantidad AS existencia,
    p.stock_minimo,
    CASE 
        WHEN ib.cantidad <= p.stock_minimo THEN 'STOCK BAJO'
        WHEN ib.cantidad = 0 THEN 'AGOTADO'
        ELSE 'DISPONIBLE'
    END AS estado_stock,
    p.precio_compra,
    p.precio_venta,
    (ib.cantidad * p.precio_compra) AS valor_inventario
FROM InventarioBodega ib
INNER JOIN Bodegas b ON ib.id_bodega = b.id_bodega
INNER JOIN Productos p ON ib.id_producto = p.id_producto
LEFT JOIN Categorias cat ON p.id_categoria = cat.id_categoria
WHERE p.estado = 1 AND b.estado = 1;
GO

CREATE OR ALTER VIEW vOrdenesEstado AS
SELECT 
    oc.id_orden_compra,
    p.nombre AS proveedor,
    oc.fecha_orden,
    oc.fecha_esperada,
    oc.estado,
    oc.total,
    COUNT(ocd.id_producto) AS cantidad_productos,
    SUM(ocd.cantidad) AS total_unidades,
    DATEDIFF(day, oc.fecha_orden, GETDATE()) AS dias_transcurridos,
    CASE 
        WHEN oc.estado = 'Pendiente' AND DATEDIFF(day, oc.fecha_orden, GETDATE()) > 7 THEN 'ATRASADA'
        ELSE 'NORMAL'
    END AS situacion_orden
FROM OrdenesCompra oc
INNER JOIN Proveedores p ON oc.id_proveedor = p.id_proveedor
INNER JOIN OrdenesCompraDetalle ocd ON oc.id_orden_compra = ocd.id_orden_compra
GROUP BY oc.id_orden_compra, p.nombre, oc.fecha_orden, oc.fecha_esperada, oc.estado, oc.total;
GO

CREATE OR ALTER VIEW vMovimientosDia AS
-- Ventas del día
SELECT 
    v.fecha_venta AS fecha,
    'VENTA' AS tipo_movimiento,
    c.nombre AS cliente_proveedor,
    v.total AS monto,
    v.tipo_venta AS detalle,
    'INGRESO' AS tipo_flujo
FROM Ventas v
INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
WHERE v.estado = 'Pagada'

UNION ALL

-- Pagos de clientes
SELECT 
    pc.fecha_pago AS fecha,
    'PAGO CLIENTE' AS tipo_movimiento,
    c.nombre AS cliente_proveedor,
    pc.monto AS monto,
    'ABONO A CUENTA' AS detalle,
    'INGRESO' AS tipo_flujo
FROM PagosClientes pc
INNER JOIN Clientes c ON pc.id_cliente = c.id_cliente
WHERE pc.estado = 'Aplicado'

UNION ALL

-- Pagos a proveedores
SELECT 
    pp.fecha_pago AS fecha,
    'PAGO PROVEEDOR' AS tipo_movimiento,
    p.nombre AS cliente_proveedor,
    pp.monto AS monto,
    pp.tipo_pago AS detalle,
    'EGRESO' AS tipo_flujo
FROM PagosProveedores pp
INNER JOIN Proveedores p ON pp.id_proveedor = p.id_proveedor
WHERE pp.estado = 'Aplicado';
GO

CREATE OR ALTER VIEW vSaldoCuentasBancarias AS
SELECT 
    b.id_banco,
    b.nombre_banco,
    b.numero_cuenta,
    b.saldo,
    
    -- Últimos movimientos
    (SELECT TOP 1 fecha_pago 
     FROM PagosProveedores 
     WHERE id_banco = b.id_banco 
     ORDER BY fecha_pago DESC) AS ultimo_egreso,
     
    (SELECT TOP 1 fecha_deposito 
     FROM Depositos 
     WHERE id_banco = b.id_banco 
     ORDER BY fecha_deposito DESC) AS ultimo_ingreso,
    
    -- Total de depósitos del mes actual
    ISNULL((SELECT SUM(monto) 
            FROM Depositos 
            WHERE id_banco = b.id_banco 
            AND MONTH(fecha_deposito) = MONTH(GETDATE()) 
            AND YEAR(fecha_deposito) = YEAR(GETDATE())), 0) AS depositos_mes_actual,
    
    -- Total de pagos del mes actual
    ISNULL((SELECT SUM(monto) 
            FROM PagosProveedores 
            WHERE id_banco = b.id_banco 
            AND MONTH(fecha_pago) = MONTH(GETDATE()) 
            AND YEAR(fecha_pago) = YEAR(GETDATE())), 0) AS pagos_mes_actual

FROM Bancos b
WHERE b.estado = 1;
GO

CREATE OR ALTER VIEW vProductosStockBajo AS
SELECT 
    p.id_producto,
    p.codigo,
    p.nombre,
    p.stock_actual,
    p.stock_minimo,
    (p.stock_minimo - p.stock_actual) AS faltante,
    p.precio_compra,
    p.precio_venta,
    cat.nombre AS categoria,
    CASE 
        WHEN p.stock_actual = 0 THEN 'AGOTADO'
        WHEN p.stock_actual <= p.stock_minimo THEN 'STOCK BAJO'
        ELSE 'SUFICIENTE'
    END AS estado_inventario
FROM Productos p
LEFT JOIN Categorias cat ON p.id_categoria = cat.id_categoria
WHERE p.estado = 1 
AND p.stock_actual <= p.stock_minimo;
GO

CREATE OR ALTER VIEW vEstadoCuentaClientes AS
SELECT 
    c.id_cliente,
    c.nombre AS cliente,
    c.tipo_cliente,
    c.limite_credito,
    c.saldo_actual,
    (c.limite_credito - c.saldo_actual) AS credito_disponible,
    
    -- Ventas pendientes de pago
    ISNULL((SELECT COUNT(*) 
            FROM Ventas 
            WHERE id_cliente = c.id_cliente 
            AND estado = 'Pendiente'), 0) AS ventas_pendientes,
    
    -- Total ventas del mes
    ISNULL((SELECT SUM(total) 
            FROM Ventas 
            WHERE id_cliente = c.id_cliente 
            AND MONTH(fecha_venta) = MONTH(GETDATE()) 
            AND YEAR(fecha_venta) = YEAR(GETDATE())), 0) AS ventas_mes_actual,
    
    CASE 
        WHEN c.saldo_actual > (c.limite_credito * 0.8) THEN 'ALTO RIESGO'
        WHEN c.saldo_actual > (c.limite_credito * 0.5) THEN 'RIESGO MODERADO'
        ELSE 'BAJO RIESGO'
    END AS nivel_riesgo_credito

FROM Clientes c
WHERE c.estado = 1;
GO