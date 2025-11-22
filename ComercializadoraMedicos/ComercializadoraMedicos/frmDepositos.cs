using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmDepositos : Form
    {
        private DatabaseHelper dbHelper;

        public frmDepositos()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmDepositos_Load(object sender, EventArgs e)
        {
            CargarBancos();
            dtpFechaDeposito.Value = DateTime.Now;
        }

        private void CargarBancos()
        {
            try
            {
                string query = "SELECT id_banco, nombre_banco, numero_cuenta, saldo FROM Bancos WHERE estado = 1 ORDER BY nombre_banco";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbBanco.DataSource = dt;
                cmbBanco.DisplayMember = "nombre_banco";
                cmbBanco.ValueMember = "id_banco";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar bancos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarDeposito_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos()) return;

            try
            {
                SqlParameter[] parameters = {
            new SqlParameter("@id_banco", cmbBanco.SelectedValue),
            new SqlParameter("@monto", decimal.Parse(txtMonto.Text)),
            new SqlParameter("@descripcion", txtDescripcion.Text.Trim())
        };

                // Llamada al nuevo SP transaccional
                dbHelper.ExecuteStoredProcedure("sp_Depositos_Insertar", parameters);

                MessageBox.Show("Depósito registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar depósito: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (cmbBanco.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un banco.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("El monto debe ser un número mayor a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonto.Focus();
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMonto_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}