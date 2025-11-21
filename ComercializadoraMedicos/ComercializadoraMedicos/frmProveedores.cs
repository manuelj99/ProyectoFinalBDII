using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace ComercializadoraMedicos
{
    public partial class frmProveedores : Form
    {
        private DatabaseHelper dbHelper;
        private int proveedorIdActual = 0;
        private bool modoEdicion = false;
        private Timer timerBusqueda;

        public frmProveedores()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void CargarProveedores()
        {
            string query = "SELECT id_proveedor, nombre, direccion, telefono, email, limite_credito, saldo_actual FROM Proveedores WHERE estado = 1";
            DataTable dt = dbHelper.ExecuteQuery(query);
            dgvProveedores.DataSource = dt;

            // Configurar columnas del DataGridView
            if (dgvProveedores.Columns.Count > 0)
            {
                dgvProveedores.Columns["id_proveedor"].HeaderText = "ID";
                dgvProveedores.Columns["id_proveedor"].Width = 50;
                dgvProveedores.Columns["nombre"].HeaderText = "Nombre";
                dgvProveedores.Columns["nombre"].Width = 150;
                dgvProveedores.Columns["direccion"].HeaderText = "Dirección";
                dgvProveedores.Columns["direccion"].Width = 200;
                dgvProveedores.Columns["telefono"].HeaderText = "Teléfono";
                dgvProveedores.Columns["telefono"].Width = 100;
                dgvProveedores.Columns["email"].HeaderText = "Email";
                dgvProveedores.Columns["email"].Width = 150;
                dgvProveedores.Columns["limite_credito"].HeaderText = "Límite Crédito";
                dgvProveedores.Columns["limite_credito"].Width = 100;
                dgvProveedores.Columns["saldo_actual"].HeaderText = "Saldo Actual";
                dgvProveedores.Columns["saldo_actual"].Width = 100;

                // Formato de moneda para las columnas numéricas
                dgvProveedores.Columns["limite_credito"].DefaultCellStyle.Format = "C2";
                dgvProveedores.Columns["saldo_actual"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            proveedorIdActual = 0;
            LimpiarFormulario();
            HabilitarControles(true);
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un proveedor para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            modoEdicion = true;
            DataGridViewRow row = dgvProveedores.SelectedRows[0];
            proveedorIdActual = Convert.ToInt32(row.Cells["id_proveedor"].Value);

            // Cargar datos en el formulario
            txtNombre.Text = row.Cells["nombre"].Value.ToString();
            txtDireccion.Text = row.Cells["direccion"].Value?.ToString() ?? "";
            txtTelefono.Text = row.Cells["telefono"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
            txtLimiteCredito.Text = Convert.ToDecimal(row.Cells["limite_credito"].Value).ToString("F2");

            HabilitarControles(true);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un proveedor para eliminar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar este proveedor? Esta acción no se puede deshacer.",
                                       "Confirmar Eliminación",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataGridViewRow row = dgvProveedores.SelectedRows[0];
                int idProveedor = Convert.ToInt32(row.Cells["id_proveedor"].Value);

                // Usar parámetros para evitar SQL injection
                string query = "UPDATE Proveedores SET estado = 0 WHERE id_proveedor = @idProveedor";
                SqlParameter[] parameters = { new SqlParameter("@idProveedor", idProveedor) };

                int affected = dbHelper.ExecuteNonQueryWithParameters(query, parameters);

                if (affected > 0)
                {
                    MessageBox.Show("Proveedor eliminado correctamente.", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProveedores();
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el proveedor.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                if (modoEdicion)
                {
                    // Actualizar proveedor existente
                    ActualizarProveedor();
                }
                else
                {
                    // Insertar nuevo proveedor
                    InsertarProveedor();
                }

                CargarProveedores();
                LimpiarFormulario();
                HabilitarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el proveedor: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertarProveedor()
        {
            string query = @"INSERT INTO Proveedores (nombre, direccion, telefono, email, limite_credito) 
                            VALUES (@nombre, @direccion, @telefono, @email, @limite_credito)";

            SqlParameter[] parameters = {
                new SqlParameter("@nombre", txtNombre.Text),
                new SqlParameter("@direccion", txtDireccion.Text),
                new SqlParameter("@telefono", txtTelefono.Text),
                new SqlParameter("@email", txtEmail.Text),
                new SqlParameter("@limite_credito", decimal.Parse(txtLimiteCredito.Text))
            };

            int affected = dbHelper.ExecuteNonQueryWithParameters(query, parameters);

            if (affected > 0)
            {
                MessageBox.Show("Proveedor agregado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarProveedor()
        {
            string query = @"UPDATE Proveedores 
                            SET nombre = @nombre,
                                direccion = @direccion,
                                telefono = @telefono,
                                email = @email,
                                limite_credito = @limite_credito
                            WHERE id_proveedor = @id_proveedor";

            SqlParameter[] parameters = {
                new SqlParameter("@nombre", txtNombre.Text),
                new SqlParameter("@direccion", txtDireccion.Text),
                new SqlParameter("@telefono", txtTelefono.Text),
                new SqlParameter("@email", txtEmail.Text),
                new SqlParameter("@limite_credito", decimal.Parse(txtLimiteCredito.Text)),
                new SqlParameter("@id_proveedor", proveedorIdActual)
            };

            int affected = dbHelper.ExecuteNonQueryWithParameters(query, parameters);

            if (affected > 0)
            {
                MessageBox.Show("Proveedor actualizado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                // Si no hay filtro, cargar todos los proveedores
                CargarProveedores();
                return;
            }

            try
            {
                // Consulta directa con LIKE - MÁS SIMPLE
                string query = $@"SELECT id_proveedor, nombre, direccion, telefono, email, limite_credito, saldo_actual 
                         FROM Proveedores 
                         WHERE estado = 1 
                         AND (nombre LIKE '%{filtro}%' 
                              OR direccion LIKE '%{filtro}%' 
                              OR telefono LIKE '%{filtro}%' 
                              OR email LIKE '%{filtro}%')";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvProveedores.DataSource = dt;

                // Actualizar el contador de resultados
                //lblResultados.Text = $"Se encontraron {dt.Rows.Count} proveedores";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Búsqueda automática cuando el usuario deja de escribir por 1 segundo
            if (timerBusqueda == null)
            {
                timerBusqueda = new Timer();
                timerBusqueda.Interval = 1000; // 1 segundo
                timerBusqueda.Tick += (s, args) =>
                {
                    timerBusqueda.Stop();
                    btnBuscar_Click(null, null);
                };
            }

            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar.PerformClick();
                e.Handled = true;
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLimiteCredito.Text) || !decimal.TryParse(txtLimiteCredito.Text, out decimal limite))
            {
                MessageBox.Show("El límite de crédito debe ser un valor numérico válido.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLimiteCredito.Focus();
                return false;
            }

            if (limite <= 0)
            {
                MessageBox.Show("El límite de crédito debe ser mayor a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLimiteCredito.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtLimiteCredito.Clear();
            proveedorIdActual = 0;
            modoEdicion = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtNombre.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtLimiteCredito.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;

            btnNuevo.Enabled = !habilitar;
            btnEditar.Enabled = !habilitar;
            btnEliminar.Enabled = !habilitar;
            btnBuscar.Enabled = !habilitar;
            txtBuscar.Enabled = !habilitar;
        }

        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            // Cargar datos automáticamente al seleccionar una fila
            if (dgvProveedores.SelectedRows.Count > 0 && !modoEdicion)
            {
                DataGridViewRow row = dgvProveedores.SelectedRows[0];
                txtNombre.Text = row.Cells["nombre"].Value.ToString();
                txtDireccion.Text = row.Cells["direccion"].Value?.ToString() ?? "";
                txtTelefono.Text = row.Cells["telefono"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
                txtLimiteCredito.Text = Convert.ToDecimal(row.Cells["limite_credito"].Value).ToString("F2");
            }
        }
    }
}