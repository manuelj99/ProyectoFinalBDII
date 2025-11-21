using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmDevolucionProveedores : Form
    {
        private DatabaseHelper dbHelper;

        public frmDevolucionProveedores()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmDevolucionProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarProductos();
            dtpFechaDevolucion.Value = DateTime.Now;
        }

        private void CargarProveedores()
        {
            try
            {
                string query = "SELECT id_proveedor, nombre FROM Proveedores WHERE estado = 1 ORDER BY nombre";
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
                string query = @"SELECT id_producto, codigo, nombre, stock_actual 
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

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue != null)
            {
                DataRowView producto = (DataRowView)cmbProducto.SelectedItem;
                int stock = Convert.ToInt32(producto["stock_actual"]);
                lblStockActual.Text = $"Stock Actual: {stock}";

                // Establecer máximo en el NumericUpDown
                numCantidad.Maximum = stock;
                if (numCantidad.Value > stock)
                    numCantidad.Value = stock;
            }
        }

        private void btnProcesarDevolucion_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                int idProveedor = (int)cmbProveedor.SelectedValue;
                int idProducto = (int)cmbProducto.SelectedValue;
                int cantidad = (int)numCantidad.Value;
                string motivo = txtMotivo.Text.Trim();

                // Usar el procedimiento almacenado existente
                string query = $@"EXEC sp_DevolucionesProveedores_Insertar 
                                {idProveedor}, 
                                {idProducto}, 
                                {cantidad}, 
                                '{motivo}'";

                int result = dbHelper.ExecuteNonQuery(query);

                if (result > 0)
                {
                    MessageBox.Show("✅ Devolución registrada correctamente.\n\n" +
                                  $"Producto: {cmbProducto.Text}\n" +
                                  $"Cantidad: {cantidad}\n" +
                                  $"Proveedor: {cmbProveedor.Text}",
                                  "Devolución Exitosa",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarProductos(); // Recargar para actualizar stock
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la devolución.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar la devolución: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (cmbProveedor.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un proveedor.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProveedor.Focus();
                return false;
            }

            if (cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un producto.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProducto.Focus();
                return false;
            }

            if (numCantidad.Value <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numCantidad.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Por favor ingrese el motivo de la devolución.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            cmbProveedor.SelectedIndex = 0;
            cmbProducto.SelectedIndex = 0;
            numCantidad.Value = 1;
            txtMotivo.Clear();
            lblStockActual.Text = "Stock Actual: 0";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
