using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ComercializadoraMedicos
{
    public partial class frmVentas : Form
    {
        private DatabaseHelper dbHelper;
        private DataTable detalleVenta;
        private decimal totalVenta = 0;
        private int clienteIdActual = 0;
        private decimal limiteCreditoCliente = 0;
        private decimal saldoActualCliente = 0;

        public frmVentas()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarProductos();
            InicializarDetalleVenta();
            LimpiarFormulario();
        }

        private void CargarClientes()
        {
            try
            {
                string query = "SELECT id_cliente, nombre, limite_credito, saldo_actual FROM Clientes WHERE estado = 1 ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbCliente.DataSource = dt;
                cmbCliente.DisplayMember = "nombre";
                cmbCliente.ValueMember = "id_cliente";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                string query = @"SELECT id_producto, codigo, nombre, precio_venta, stock_actual 
                               FROM Productos 
                               WHERE estado = 1 AND stock_actual > 0 
                               ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbProducto.DataSource = dt;
                cmbProducto.DisplayMember = "nombre";
                cmbProducto.ValueMember = "id_producto";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarDetalleVenta()
        {
            detalleVenta = new DataTable();
            detalleVenta.Columns.Add("id_producto", typeof(int));
            detalleVenta.Columns.Add("codigo", typeof(string));
            detalleVenta.Columns.Add("nombre", typeof(string));
            detalleVenta.Columns.Add("precio_unitario", typeof(decimal));
            detalleVenta.Columns.Add("cantidad", typeof(int));
            detalleVenta.Columns.Add("subtotal", typeof(decimal));

            dgvDetalleVenta.DataSource = detalleVenta;
            ConfigurarGridDetalle();
        }

        private void ConfigurarGridDetalle()
        {
            if (dgvDetalleVenta.Columns.Count > 0)
            {
                dgvDetalleVenta.Columns["id_producto"].Visible = false;
                dgvDetalleVenta.Columns["codigo"].HeaderText = "Código";
                dgvDetalleVenta.Columns["codigo"].Width = 80;
                dgvDetalleVenta.Columns["nombre"].HeaderText = "Producto";
                dgvDetalleVenta.Columns["nombre"].Width = 200;
                dgvDetalleVenta.Columns["precio_unitario"].HeaderText = "Precio Unitario";
                dgvDetalleVenta.Columns["precio_unitario"].Width = 100;
                dgvDetalleVenta.Columns["cantidad"].HeaderText = "Cantidad";
                dgvDetalleVenta.Columns["cantidad"].Width = 70;
                dgvDetalleVenta.Columns["subtotal"].HeaderText = "Subtotal";
                dgvDetalleVenta.Columns["subtotal"].Width = 100;

                dgvDetalleVenta.Columns["precio_unitario"].DefaultCellStyle.Format = "C2";
                dgvDetalleVenta.Columns["subtotal"].DefaultCellStyle.Format = "C2";
            }
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue != null && cmbCliente.SelectedValue is int)
            {
                clienteIdActual = (int)cmbCliente.SelectedValue;
                ActualizarInfoCliente();
            }
        }

        private void ActualizarInfoCliente()
        {
            try
            {
                string query = $"SELECT limite_credito, saldo_actual FROM Clientes WHERE id_cliente = {clienteIdActual}";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    limiteCreditoCliente = Convert.ToDecimal(dt.Rows[0]["limite_credito"]);
                    saldoActualCliente = Convert.ToDecimal(dt.Rows[0]["saldo_actual"]);

                    lblLimiteCredito.Text = $"Límite Crédito: {limiteCreditoCliente:C2}";
                    lblSaldoActual.Text = $"Saldo Actual: {saldoActualCliente:C2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del cliente: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue != null)
            {
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                int stock = Convert.ToInt32(producto["stock_actual"]);
                lblStockDisponible.Text = $"Stock Disponible: {stock}";
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un producto.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            try
            {
                // Obtener datos del producto seleccionado
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                int idProducto = (int)producto["id_producto"];
                string codigo = producto["codigo"].ToString();
                string nombre = producto["nombre"].ToString();
                decimal precio = Convert.ToDecimal(producto["precio_venta"]);
                int stockActual = Convert.ToInt32(producto["stock_actual"]);

                // Verificar stock disponible
                if (cantidad > stockActual)
                {
                    MessageBox.Show($"Stock insuficiente. Solo hay {stockActual} unidades disponibles.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si el producto ya está en el detalle
                DataRow[] existingRows = detalleVenta.Select($"id_producto = {idProducto}");
                if (existingRows.Length > 0)
                {
                    // Actualizar cantidad si ya existe
                    int nuevaCantidad = (int)existingRows[0]["cantidad"] + cantidad;
                    if (nuevaCantidad > stockActual)
                    {
                        MessageBox.Show($"La cantidad total ({nuevaCantidad}) excede el stock disponible ({stockActual}).", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    existingRows[0]["cantidad"] = nuevaCantidad;
                    existingRows[0]["subtotal"] = nuevaCantidad * precio;
                }
                else
                {
                    // Agregar nuevo producto al detalle
                    DataRow newRow = detalleVenta.NewRow();
                    newRow["id_producto"] = idProducto;
                    newRow["codigo"] = codigo;
                    newRow["nombre"] = nombre;
                    newRow["precio_unitario"] = precio;
                    newRow["cantidad"] = cantidad;
                    newRow["subtotal"] = cantidad * precio;
                    detalleVenta.Rows.Add(newRow);
                }

                CalcularTotal();
                LimpiarControlesProducto();
                ActualizarStockDisponible();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDetalleVenta.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDetalleVenta.SelectedRows)
                {
                    dgvDetalleVenta.Rows.Remove(row);
                }
                CalcularTotal();
                ActualizarStockDisponible();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un producto para quitar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CalcularTotal()
        {
            totalVenta = 0;
            foreach (DataRow row in detalleVenta.Rows)
            {
                totalVenta += Convert.ToDecimal(row["subtotal"]);
            }
            lblTotalVenta.Text = totalVenta.ToString("C2");
        }

        private void ActualizarStockDisponible()
        {
            if (cmbProducto.SelectedValue != null)
            {
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                int stockOriginal = Convert.ToInt32(producto["stock_actual"]);

                // Calcular stock reservado en el detalle actual
                int stockReservado = 0;
                DataRow[] rowsInDetail = detalleVenta.Select($"id_producto = {producto["id_producto"]}");
                if (rowsInDetail.Length > 0)
                {
                    stockReservado = Convert.ToInt32(rowsInDetail[0]["cantidad"]);
                }

                lblStockDisponible.Text = $"Stock Disponible: {stockOriginal - stockReservado}";
            }
        }

        private void LimpiarControlesProducto()
        {
            txtCantidad.Text = "1";
            cmbProducto.Focus();
        }

        private void LimpiarFormulario()
        {
            cmbCliente.SelectedIndex = 0;
            cmbTipoVenta.SelectedIndex = 0;
            detalleVenta.Clear();
            totalVenta = 0;
            lblTotalVenta.Text = "L0.00";
            txtCantidad.Text = "1";
            ActualizarStockDisponible();
        }

        private void btnProcesarVenta_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un cliente.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (detalleVenta.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto a la venta.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbTipoVenta.Text))
            {
                MessageBox.Show("Por favor seleccione el tipo de venta.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar límite de crédito si es venta a crédito
            if (cmbTipoVenta.Text == "Credito")
            {
                if (!ValidarLimiteCredito())
                {
                    return;
                }
            }

            // Confirmar la venta
            var result = MessageBox.Show($"¿Está seguro de procesar la venta por {totalVenta:C2}?", "Confirmar Venta",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarVenta();
            }
        }

        private bool ValidarLimiteCredito()
        {
            decimal nuevoSaldo = saldoActualCliente + totalVenta;

            if (nuevoSaldo > limiteCreditoCliente)
            {
                MessageBox.Show($"El cliente ha excedido su límite de crédito.\n\n" +
                              $"Límite de crédito: {limiteCreditoCliente:C2}\n" +
                              $"Saldo actual: {saldoActualCliente:C2}\n" +
                              $"Nuevo saldo: {nuevoSaldo:C2}\n" +
                              $"Excedente: {nuevoSaldo - limiteCreditoCliente:C2}",
                              "Límite de Crédito Excedido",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ProcesarVenta()
        {
            try
            {
                // Preparar los detalles en formato JSON para el stored procedure
                string detallesJSON = GenerarJSONDetalles();

                // Llamar al procedimiento almacenado
                string query = $@"EXEC spTransaccionVentaCompleta 
                                {clienteIdActual}, 
                                '{cmbTipoVenta.Text}', 
                                '{detallesJSON}',
                                1";

                DataTable result = dbHelper.ExecuteQuery(query);

                if (result.Rows.Count > 0)
                {
                    string resultado = result.Rows[0]["resultado"].ToString();
                    if (resultado == "Éxito")
                    {
                        int idVenta = Convert.ToInt32(result.Rows[0]["id_venta"]);
                        decimal total = Convert.ToDecimal(result.Rows[0]["total"]);

                        MessageBox.Show($"✅ Venta procesada exitosamente!\n\n" +
                                      $"Número de venta: {idVenta}\n" +
                                      $"Total: {total:C2}\n" +
                                      $"Tipo: {cmbTipoVenta.Text}",
                                      "Venta Exitosa",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarFormulario();
                        CargarProductos(); // Recargar productos para actualizar stocks
                    }
                    else
                    {
                        string mensaje = result.Rows[0]["mensaje"].ToString();
                        MessageBox.Show($"❌ Error al procesar la venta: {mensaje}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar la venta: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarJSONDetalles()
        {
            // Crear JSON manualmente para los detalles
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            json.Append("[");

            for (int i = 0; i < detalleVenta.Rows.Count; i++)
            {
                DataRow row = detalleVenta.Rows[i];
                json.Append("{");
                json.Append($"\"id_producto\": {row["id_producto"]},");
                json.Append($"\"cantidad\": {row["cantidad"]},");
                json.Append($"\"precio_unitario\": {row["precio_unitario"]}");
                json.Append("}");

                if (i < detalleVenta.Rows.Count - 1)
                    json.Append(",");
            }

            json.Append("]");
            return json.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de cancelar la venta? Se perderán todos los datos ingresados.",
                                       "Cancelar Venta",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvDetalleVenta_SelectionChanged(object sender, EventArgs e)
        {
            // Actualizar stock disponible cuando se selecciona un producto en el grid
            if (dgvDetalleVenta.SelectedRows.Count > 0 && cmbProducto.SelectedValue != null)
            {
                ActualizarStockDisponible();
            }
        }
    }
}