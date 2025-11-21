using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ComercializadoraMedicos
{
    public partial class frmOrdenesCompra : Form
    {
        private DatabaseHelper dbHelper;
        private DataTable detalleOrden;
        private decimal totalOrden = 0;
        private int proveedorIdActual = 0;
        private decimal limiteCreditoProveedor = 0;
        private decimal saldoActualProveedor = 0;

        public frmOrdenesCompra()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmOrdenesCompra_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarProductos();
            InicializarDetalleOrden();
            LimpiarFormulario();

            // Configurar fecha esperada (7 días a partir de hoy)
            dtpFechaEsperada.Value = DateTime.Now.AddDays(7);
        }

        private void CargarProveedores()
        {
            try
            {
                string query = "SELECT id_proveedor, nombre, limite_credito, saldo_actual FROM Proveedores WHERE estado = 1 ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id_proveedor";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                string query = @"SELECT id_producto, codigo, nombre, precio_compra, stock_actual 
                               FROM Productos 
                               WHERE estado = 1 
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

        private void InicializarDetalleOrden()
        {
            detalleOrden = new DataTable();
            detalleOrden.Columns.Add("id_producto", typeof(int));
            detalleOrden.Columns.Add("codigo", typeof(string));
            detalleOrden.Columns.Add("nombre", typeof(string));
            detalleOrden.Columns.Add("precio_unitario", typeof(decimal));
            detalleOrden.Columns.Add("cantidad", typeof(int));
            detalleOrden.Columns.Add("subtotal", typeof(decimal));

            dgvDetalleOrden.DataSource = detalleOrden;
            ConfigurarGridDetalle();
        }

        private void ConfigurarGridDetalle()
        {
            if (dgvDetalleOrden.Columns.Count > 0)
            {
                dgvDetalleOrden.Columns["id_producto"].Visible = false;
                dgvDetalleOrden.Columns["codigo"].HeaderText = "Código";
                dgvDetalleOrden.Columns["codigo"].Width = 80;
                dgvDetalleOrden.Columns["nombre"].HeaderText = "Producto";
                dgvDetalleOrden.Columns["nombre"].Width = 200;
                dgvDetalleOrden.Columns["precio_unitario"].HeaderText = "Precio Compra";
                dgvDetalleOrden.Columns["precio_unitario"].Width = 100;
                dgvDetalleOrden.Columns["cantidad"].HeaderText = "Cantidad";
                dgvDetalleOrden.Columns["cantidad"].Width = 70;
                dgvDetalleOrden.Columns["subtotal"].HeaderText = "Subtotal";
                dgvDetalleOrden.Columns["subtotal"].Width = 100;

                dgvDetalleOrden.Columns["precio_unitario"].DefaultCellStyle.Format = "C2";
                dgvDetalleOrden.Columns["subtotal"].DefaultCellStyle.Format = "C2";
            }
        }

        private void cmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue != null && cmbProveedor.SelectedValue is int)
            {
                proveedorIdActual = (int)cmbProveedor.SelectedValue;
                ActualizarInfoProveedor();
            }
        }

        private void ActualizarInfoProveedor()
        {
            try
            {
                string query = $"SELECT limite_credito, saldo_actual FROM Proveedores WHERE id_proveedor = {proveedorIdActual}";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    limiteCreditoProveedor = Convert.ToDecimal(dt.Rows[0]["limite_credito"]);
                    saldoActualProveedor = Convert.ToDecimal(dt.Rows[0]["saldo_actual"]);

                    lblLimiteCredito.Text = $"Límite Crédito: {limiteCreditoProveedor:C2}";
                    lblSaldoActual.Text = $"Saldo Actual: {saldoActualProveedor:C2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del proveedor: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue != null)
            {
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                decimal precioCompra = Convert.ToDecimal(producto["precio_compra"]);
                txtPrecioCompra.Text = precioCompra.ToString("F2");
                lblStockActual.Text = $"Stock Actual: {producto["stock_actual"]}";
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

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio de compra debe ser un número mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return;
            }

            try
            {
                // Obtener datos del producto seleccionado
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                int idProducto = (int)producto["id_producto"];
                string codigo = producto["codigo"].ToString();
                string nombre = producto["nombre"].ToString();

                // Verificar si el producto ya está en el detalle
                DataRow[] existingRows = detalleOrden.Select($"id_producto = {idProducto}");
                if (existingRows.Length > 0)
                {
                    // Actualizar cantidad si ya existe
                    int nuevaCantidad = (int)existingRows[0]["cantidad"] + cantidad;
                    existingRows[0]["cantidad"] = nuevaCantidad;
                    existingRows[0]["precio_unitario"] = precio;
                    existingRows[0]["subtotal"] = nuevaCantidad * precio;
                }
                else
                {
                    // Agregar nuevo producto al detalle
                    DataRow newRow = detalleOrden.NewRow();
                    newRow["id_producto"] = idProducto;
                    newRow["codigo"] = codigo;
                    newRow["nombre"] = nombre;
                    newRow["precio_unitario"] = precio;
                    newRow["cantidad"] = cantidad;
                    newRow["subtotal"] = cantidad * precio;
                    detalleOrden.Rows.Add(newRow);
                }

                CalcularTotal();
                LimpiarControlesProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDetalleOrden.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDetalleOrden.SelectedRows)
                {
                    dgvDetalleOrden.Rows.Remove(row);
                }
                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Por favor seleccione un producto para quitar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CalcularTotal()
        {
            totalOrden = 0;
            foreach (DataRow row in detalleOrden.Rows)
            {
                totalOrden += Convert.ToDecimal(row["subtotal"]);
            }
            lblTotalOrden.Text = totalOrden.ToString("C2");

            // Validar límite de crédito en tiempo real
            ValidarLimiteCreditoTiempoReal();
        }

        private void ValidarLimiteCreditoTiempoReal()
        {
            decimal nuevoSaldo = saldoActualProveedor + totalOrden;

            if (nuevoSaldo > limiteCreditoProveedor)
            {
                lblTotalOrden.ForeColor = System.Drawing.Color.Red;
                lblMensajeCredito.Text = $"⚠️ Límite excedido por: {(nuevoSaldo - limiteCreditoProveedor):C2}";
                lblMensajeCredito.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblTotalOrden.ForeColor = System.Drawing.Color.Green;
                lblMensajeCredito.Text = $"✅ Crédito disponible: {(limiteCreditoProveedor - nuevoSaldo):C2}";
                lblMensajeCredito.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void LimpiarControlesProducto()
        {
            txtCantidad.Text = "1";
            if (cmbProducto.SelectedValue != null)
            {
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                decimal precioCompra = Convert.ToDecimal(producto["precio_compra"]);
                txtPrecioCompra.Text = precioCompra.ToString("F2");
            }
            cmbProducto.Focus();
        }

        private void LimpiarFormulario()
        {
            cmbProveedor.SelectedIndex = 0;
            dtpFechaEsperada.Value = DateTime.Now.AddDays(7);
            detalleOrden.Clear();
            totalOrden = 0;
            lblTotalOrden.Text = "L0.00";
            txtCantidad.Text = "1";
            lblMensajeCredito.Text = "";
            lblTotalOrden.ForeColor = System.Drawing.Color.Black;
        }

        private void btnProcesarOrden_Click(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un proveedor.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (detalleOrden.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto a la orden.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar límite de crédito del proveedor
            if (!ValidarLimiteCredito())
            {
                return;
            }

            // Confirmar la orden
            var result = MessageBox.Show($"¿Está seguro de procesar la orden de compra por {totalOrden:C2}?", "Confirmar Orden",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarOrden();
            }
        }

        private bool ValidarLimiteCredito()
        {
            decimal nuevoSaldo = saldoActualProveedor + totalOrden;

            if (nuevoSaldo > limiteCreditoProveedor)
            {
                MessageBox.Show($"El proveedor ha excedido su límite de crédito.\n\n" +
                              $"Límite de crédito: {limiteCreditoProveedor:C2}\n" +
                              $"Saldo actual: {saldoActualProveedor:C2}\n" +
                              $"Nuevo saldo: {nuevoSaldo:C2}\n" +
                              $"Excedente: {nuevoSaldo - limiteCreditoProveedor:C2}",
                              "Límite de Crédito Excedido",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ProcesarOrden()
        {
            try
            {
                // Preparar los detalles en formato JSON para el stored procedure
                string detallesJSON = GenerarJSONDetalles();

                // Llamar al procedimiento almacenado
                string query = $@"EXEC sp_OrdenesCompra_Insertar 
                                {proveedorIdActual}, 
                                '{dtpFechaEsperada.Value:yyyy-MM-dd}', 
                                '{detallesJSON}'";

                DataTable result = dbHelper.ExecuteQuery(query);

                if (result.Rows.Count > 0)
                {
                    int idOrden = Convert.ToInt32(result.Rows[0]["id_orden_compra"]);
                    decimal total = Convert.ToDecimal(result.Rows[0]["total"]);

                    MessageBox.Show($"✅ Orden de compra procesada exitosamente!\n\n" +
                                  $"Número de orden: {idOrden}\n" +
                                  $"Total: {total:C2}",
                                  "Orden Exitosa",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarProveedores(); // Recargar proveedores para actualizar saldos
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar la orden: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarJSONDetalles()
        {
            // Crear JSON manualmente para los detalles
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            json.Append("[");

            for (int i = 0; i < detalleOrden.Rows.Count; i++)
            {
                DataRow row = detalleOrden.Rows[i];
                json.Append("{");
                json.Append($"\"id_producto\": {row["id_producto"]},");
                json.Append($"\"cantidad\": {row["cantidad"]},");
                json.Append($"\"precio_unitario\": {row["precio_unitario"]}");
                json.Append("}");

                if (i < detalleOrden.Rows.Count - 1)
                    json.Append(",");
            }

            json.Append("]");
            return json.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de cancelar la orden? Se perderán todos los datos ingresados.",
                                       "Cancelar Orden",
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

        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, punto decimal y control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Permitir solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnGenerarAutomaticas_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("¿Desea generar órdenes de compra automáticas para productos con stock bajo?",
                                           "Órdenes Automáticas",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DataTable resultAuto = dbHelper.ExecuteQuery("EXEC spGenerarOrdenesCompraAutomaticas");

                    if (resultAuto.Rows.Count > 0 || resultAuto.Rows.Count == 0)
                    {
                        MessageBox.Show("Proceso de órdenes automáticas completado.", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Recargar datos
                        CargarProveedores();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar órdenes automáticas: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
