using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmClientes : Form
    {
        private DatabaseHelper dbHelper;
        private int clienteIdActual = 0;
        private bool modoEdicion = false;
        private Timer timerBusqueda;

        public frmClientes()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void CargarClientes()
        {
            string query = "SELECT id_cliente, nombre, direccion, telefono, email, limite_credito, saldo_actual, tipo_cliente FROM Clientes WHERE estado = 1";
            DataTable dt = dbHelper.ExecuteQuery(query);
            dgvClientes.DataSource = dt;

            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            if (dgvClientes.Columns.Count > 0)
            {
                dgvClientes.Columns["id_cliente"].HeaderText = "ID";
                dgvClientes.Columns["id_cliente"].Width = 50;
                dgvClientes.Columns["nombre"].HeaderText = "Nombre";
                dgvClientes.Columns["nombre"].Width = 150;
                dgvClientes.Columns["direccion"].HeaderText = "Dirección";
                dgvClientes.Columns["direccion"].Width = 200;
                dgvClientes.Columns["telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["telefono"].Width = 100;
                dgvClientes.Columns["email"].HeaderText = "Email";
                dgvClientes.Columns["email"].Width = 150;
                dgvClientes.Columns["limite_credito"].HeaderText = "Límite Crédito";
                dgvClientes.Columns["limite_credito"].Width = 100;
                dgvClientes.Columns["saldo_actual"].HeaderText = "Saldo Actual";
                dgvClientes.Columns["saldo_actual"].Width = 100;
                dgvClientes.Columns["tipo_cliente"].HeaderText = "Tipo Cliente";
                dgvClientes.Columns["tipo_cliente"].Width = 80;

                // Formato de moneda
                dgvClientes.Columns["limite_credito"].DefaultCellStyle.Format = "C2";
                dgvClientes.Columns["saldo_actual"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            RealizarBusqueda();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (timerBusqueda == null)
            {
                timerBusqueda = new Timer();
                timerBusqueda.Interval = 1000;
                timerBusqueda.Tick += (s, args) =>
                {
                    timerBusqueda.Stop();
                    RealizarBusqueda();
                };
            }

            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void RealizarBusqueda()
        {
            string filtro = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                CargarClientes();
                return;
            }

            try
            {
                string query = $@"SELECT id_cliente, nombre, direccion, telefono, email, limite_credito, saldo_actual, tipo_cliente
                                FROM Clientes 
                                WHERE estado = 1 
                                AND (nombre LIKE '%{filtro}%' 
                                     OR direccion LIKE '%{filtro}%' 
                                     OR telefono LIKE '%{filtro}%' 
                                     OR email LIKE '%{filtro}%'
                                     OR tipo_cliente LIKE '%{filtro}%')";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvClientes.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarClientes();
            txtBuscar.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            clienteIdActual = 0;
            LimpiarFormulario();
            HabilitarControles(true);
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un cliente para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            modoEdicion = true;
            DataGridViewRow row = dgvClientes.SelectedRows[0];
            clienteIdActual = Convert.ToInt32(row.Cells["id_cliente"].Value);

            CargarDatosDesdeFila(row);
            HabilitarControles(true);
        }

        private void CargarDatosDesdeFila(DataGridViewRow row)
        {
            txtNombre.Text = row.Cells["nombre"].Value.ToString();
            txtDireccion.Text = row.Cells["direccion"].Value?.ToString() ?? "";
            txtTelefono.Text = row.Cells["telefono"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
            txtLimiteCredito.Text = Convert.ToDecimal(row.Cells["limite_credito"].Value).ToString("F2");
            cmbTipoCliente.Text = row.Cells["tipo_cliente"].Value.ToString();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                if (modoEdicion)
                {
                    ActualizarCliente();
                }
                else
                {
                    InsertarCliente();
                }

                CargarClientes();
                LimpiarFormulario();
                HabilitarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertarCliente()
        {
            string query = $@"INSERT INTO Clientes 
                            (nombre, direccion, telefono, email, limite_credito, tipo_cliente) 
                            VALUES 
                            ('{txtNombre.Text}', '{txtDireccion.Text}', '{txtTelefono.Text}', 
                             '{txtEmail.Text}', {decimal.Parse(txtLimiteCredito.Text)}, 
                             '{cmbTipoCliente.Text}')";

            int affected = dbHelper.ExecuteNonQuery(query);

            if (affected > 0)
            {
                MessageBox.Show("Cliente agregado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarCliente()
        {
            string query = $@"UPDATE Clientes 
                            SET nombre = '{txtNombre.Text}',
                                direccion = '{txtDireccion.Text}',
                                telefono = '{txtTelefono.Text}',
                                email = '{txtEmail.Text}',
                                limite_credito = {decimal.Parse(txtLimiteCredito.Text)},
                                tipo_cliente = '{cmbTipoCliente.Text}'
                            WHERE id_cliente = {clienteIdActual}";

            int affected = dbHelper.ExecuteNonQuery(query);

            if (affected > 0)
            {
                MessageBox.Show("Cliente actualizado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un cliente para eliminar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar este cliente?", "Confirmar",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataGridViewRow row = dgvClientes.SelectedRows[0];
                int idCliente = Convert.ToInt32(row.Cells["id_cliente"].Value);

                string query = $"UPDATE Clientes SET estado = 0 WHERE id_cliente = {idCliente}";
                int affected = dbHelper.ExecuteNonQuery(query);

                if (affected > 0)
                {
                    MessageBox.Show("Cliente eliminado correctamente.", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                    LimpiarFormulario();
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del cliente es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbTipoCliente.Text))
            {
                MessageBox.Show("Debe seleccionar un tipo de cliente.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoCliente.Focus();
                return false;
            }

            if (!decimal.TryParse(txtLimiteCredito.Text, out decimal limite) || limite < 0)
            {
                MessageBox.Show("El límite de crédito debe ser un número mayor o igual a cero.", "Validación",
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
            cmbTipoCliente.SelectedIndex = -1;
            clienteIdActual = 0;
            modoEdicion = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtNombre.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtLimiteCredito.Enabled = habilitar;
            cmbTipoCliente.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;

            btnNuevo.Enabled = !habilitar;
            btnEditar.Enabled = !habilitar;
            btnEliminar.Enabled = !habilitar;
            btnBuscar.Enabled = !habilitar;
            txtBuscar.Enabled = !habilitar;
            btnLimpiar.Enabled = !habilitar;
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0 && !modoEdicion)
            {
                DataGridViewRow row = dgvClientes.SelectedRows[0];
                CargarDatosDesdeFila(row);
            }
        }
    }
}