use db20232000983
go

-- Tabla de Proveedores
CREATE TABLE Proveedores (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(200),
    telefono VARCHAR(20),
    email VARCHAR(100),
    limite_credito DECIMAL(15,2) DEFAULT 0,
    saldo_actual DECIMAL(15,2) DEFAULT 0,
    estado BIT DEFAULT 1,
    fecha_registro DATETIME DEFAULT GETDATE()
);

-- Tabla de Categorías de Productos
CREATE TABLE Categorias (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200)
);

-- Tabla de Productos
CREATE TABLE Productos (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(50) UNIQUE NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500),
    id_categoria INT,
    tipo VARCHAR(20) CHECK (tipo IN ('Medicamento', 'Material', 'Producto_Elaborado')),
    precio_compra DECIMAL(10,2),
    precio_venta DECIMAL(10,2),
    stock_minimo INT DEFAULT 0,
    stock_actual INT DEFAULT 0,
    es_materia_prima BIT DEFAULT 0,
    estado BIT DEFAULT 1,
    FOREIGN KEY (id_categoria) REFERENCES Categorias(id_categoria)
);

-- Tabla de Bodegas/Almacenes
CREATE TABLE Bodegas (
    id_bodega INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    ubicacion VARCHAR(200),
    capacidad INT,
    estado BIT DEFAULT 1
);

-- Tabla de Inventario por Bodega
CREATE TABLE InventarioBodega (
    id_inventario INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    id_bodega INT NOT NULL,
    cantidad INT NOT NULL,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_bodega) REFERENCES Bodegas(id_bodega)
);

-- Tabla de Clientes
CREATE TABLE Clientes (
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(200),
    telefono VARCHAR(20),
    email VARCHAR(100),
    limite_credito DECIMAL(15,2) DEFAULT 0,
    saldo_actual DECIMAL(15,2) DEFAULT 0,
    tipo_cliente VARCHAR(20) CHECK (tipo_cliente IN ('Mayorista', 'Detalle')),
    estado BIT DEFAULT 1,
    fecha_registro DATETIME DEFAULT GETDATE()
);

-- Tabla de Órdenes de Compra
CREATE TABLE OrdenesCompra (
    id_orden_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    fecha_orden DATE NOT NULL,
    fecha_esperada DATE,
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Aprobada', 'Recibida', 'Cancelada')) DEFAULT 'Pendiente',
    total DECIMAL(15,2) DEFAULT 0,
    observaciones VARCHAR(500),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- Tabla de Detalle de Órdenes de Compra
CREATE TABLE OrdenesCompraDetalle (
    id_detalle INT IDENTITY(1,1) PRIMARY KEY,
    id_orden_compra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(15,2) NOT NULL,
    FOREIGN KEY (id_orden_compra) REFERENCES OrdenesCompra(id_orden_compra),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- Tabla de Recepción de Mercadería
CREATE TABLE Recepciones (
    id_recepcion INT IDENTITY(1,1) PRIMARY KEY,
    id_orden_compra INT NOT NULL,
    fecha_recepcion DATE NOT NULL,
    id_proveedor INT NOT NULL,
    id_bodega INT NOT NULL,
    observaciones VARCHAR(500),
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Aceptada', 'Rechazada')) DEFAULT 'Pendiente',
    FOREIGN KEY (id_orden_compra) REFERENCES OrdenesCompra(id_orden_compra),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_bodega) REFERENCES Bodegas(id_bodega)
);

-- Tabla de Detalle de Recepción
CREATE TABLE RecepcionesDetalle (
    id_detalle_recepcion INT IDENTITY(1,1) PRIMARY KEY,
    id_recepcion INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad_solicitada INT NOT NULL,
    cantidad_recibida INT NOT NULL,
    cantidad_aceptada INT NOT NULL,
    cantidad_rechazada INT NOT NULL,
    motivo_rechazo VARCHAR(200),
    FOREIGN KEY (id_recepcion) REFERENCES Recepciones(id_recepcion),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- Tabla de Ventas
CREATE TABLE Ventas (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    fecha_venta DATE NOT NULL,
    tipo_venta VARCHAR(20) CHECK (tipo_venta IN ('Contado', 'Credito')),
    total DECIMAL(15,2) DEFAULT 0,
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Pagada', 'Cancelada')) DEFAULT 'Pendiente',
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- Tabla de Detalle de Ventas
CREATE TABLE VentasDetalle (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(15,2) NOT NULL,
    FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- Tabla de Bancos
CREATE TABLE Bancos (
    id_banco INT IDENTITY(1,1) PRIMARY KEY,
    nombre_banco VARCHAR(100) NOT NULL,
    numero_cuenta VARCHAR(50) UNIQUE NOT NULL,
    saldo DECIMAL(15,2) DEFAULT 0,
    estado BIT DEFAULT 1
);

-- Tabla de Pagos a Proveedores
CREATE TABLE PagosProveedores (
    id_pago_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    id_banco INT NOT NULL,
    fecha_pago DATE NOT NULL,
    monto DECIMAL(15,2) NOT NULL,
    tipo_pago VARCHAR(20) CHECK (tipo_pago IN ('Cheque', 'Transferencia')),
    numero_documento VARCHAR(50),
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Aplicado', 'Cancelado')) DEFAULT 'Pendiente',
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_banco) REFERENCES Bancos(id_banco)
);

-- Tabla de Pagos de Clientes
CREATE TABLE PagosClientes (
    id_pago_cliente INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    fecha_pago DATE NOT NULL,
    monto DECIMAL(15,2) NOT NULL,
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Aplicado', 'Cancelado')) DEFAULT 'Pendiente',
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- Tabla de Depósitos
CREATE TABLE Depositos (
    id_deposito INT IDENTITY(1,1) PRIMARY KEY,
    id_banco INT NOT NULL,
    fecha_deposito DATE NOT NULL,
    monto DECIMAL(15,2) NOT NULL,
    descripcion VARCHAR(200),
    FOREIGN KEY (id_banco) REFERENCES Bancos(id_banco)
);

-- Tabla de Elaboración de Productos
CREATE TABLE ElaboracionProductos (
    id_elaboracion INT IDENTITY(1,1) PRIMARY KEY,
    fecha_elaboracion DATE NOT NULL,
    id_producto_elaborado INT NOT NULL,
    cantidad_elaborada INT NOT NULL,
    estado VARCHAR(20) CHECK (estado IN ('Pendiente', 'Completado', 'Cancelado')) DEFAULT 'Pendiente',
    FOREIGN KEY (id_producto_elaborado) REFERENCES Productos(id_producto)
);

-- Tabla de Detalle de Elaboración
CREATE TABLE ElaboracionDetalle (
    id_detalle_elaboracion INT IDENTITY(1,1) PRIMARY KEY,
    id_elaboracion INT NOT NULL,
    id_materia_prima INT NOT NULL,
    cantidad_utilizada INT NOT NULL,
    FOREIGN KEY (id_elaboracion) REFERENCES ElaboracionProductos(id_elaboracion),
    FOREIGN KEY (id_materia_prima) REFERENCES Productos(id_producto)
);

-- Tabla de Devoluciones a Proveedores
CREATE TABLE DevolucionesProveedores (
    id_devolucion INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    id_producto INT NOT NULL,
    fecha_devolucion DATE NOT NULL,
    cantidad INT NOT NULL,
    motivo VARCHAR(200),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- Tabla de Arqueos de Caja
CREATE TABLE ArqueosCaja (
    id_arqueo INT IDENTITY(1,1) PRIMARY KEY,
    fecha_arqueo DATE NOT NULL,
    total_ventas_contado DECIMAL(15,2) DEFAULT 0,
    total_pagos_recibidos DECIMAL(15,2) DEFAULT 0,
    total_depositado DECIMAL(15,2) DEFAULT 0,
    diferencia DECIMAL(15,2) DEFAULT 0,
    observaciones VARCHAR(200)
);