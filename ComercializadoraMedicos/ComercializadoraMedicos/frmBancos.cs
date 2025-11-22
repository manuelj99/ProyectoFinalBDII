using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmBancos : Form
    {
        private DatabaseHelper dbHelper;
        private int bancoIdActual = 0;
        private bool modoEdicion = false;
        private Timer timerBusqueda;

        public frmBancos()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmBancos_Load(object sender, EventArgs e)
        {
            CargarBancos();
            CargarMovimientos();
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void CargarBancos()
        {
            try
            {
                string query = "SELECT id_banco, nombre_banco, numero_cuenta, saldo FROM Bancos WHERE estado = 1 ORDER BY nombre_banco";
                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvBancos.DataSource = dt;

                ConfigurarGridBancos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar bancos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGridBancos()
        {
            if (dgvBancos.Columns.Count > 0)
            {
                dgvBancos.Columns["id_banco"].HeaderText = "ID";
                dgvBancos.Columns["id_banco"].Width = 50;
                dgvBancos.Columns["nombre_banco"].HeaderText = "Banco";
                dgvBancos.Columns["nombre_banco"].Width = 150;
                dgvBancos.Columns["numero_cuenta"].HeaderText = "Número de Cuenta";
                dgvBancos.Columns["numero_cuenta"].Width = 120;
                dgvBancos.Columns["saldo"].HeaderText = "Saldo Actual";
                dgvBancos.Columns["saldo"].Width = 100;

                dgvBancos.Columns["saldo"].DefaultCellStyle.Format = "C2";
                dgvBancos.Columns["saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void CargarMovimientos()
        {
            try
            {
                string query = @"SELECT 
                                CASE 
                                    WHEN pp.id_pago_proveedor IS NOT NULL THEN 'PAGO PROVEEDOR'
                                    WHEN d.id_deposito IS NOT NULL THEN 'DEPÓSITO'
                                    ELSE 'OTRO'
                                END AS tipo_movimiento,
                                COALESCE(pp.fecha_pago, d.fecha_deposito) AS fecha,
                                b.nombre_banco,
                                b.numero_cuenta,
                                COALESCE(pp.monto, d.monto) AS monto,
                                COALESCE(pp.tipo_pago, 'DEPÓSITO') AS descripcion
                            FROM Bancos b
                            LEFT JOIN PagosProveedores pp ON b.id_banco = pp.id_banco
                            LEFT JOIN Depositos d ON b.id_banco = d.id_banco
                            WHERE (pp.id_pago_proveedor IS NOT NULL OR d.id_deposito IS NOT NULL)
                            AND COALESCE(pp.fecha_pago, d.fecha_deposito) >= DATEADD(month, -1, GETDATE())
                            ORDER BY fecha DESC";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvMovimientos.DataSource = dt;

                ConfigurarGridMovimientos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar movimientos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGridMovimientos()
        {
            if (dgvMovimientos.Columns.Count > 0)
            {
                dgvMovimientos.Columns["tipo_movimiento"].HeaderText = "Tipo";
                dgvMovimientos.Columns["tipo_movimiento"].Width = 120;
                dgvMovimientos.Columns["fecha"].HeaderText = "Fecha";
                dgvMovimientos.Columns["fecha"].Width = 80;
                dgvMovimientos.Columns["nombre_banco"].HeaderText = "Banco";
                dgvMovimientos.Columns["nombre_banco"].Width = 120;
                dgvMovimientos.Columns["numero_cuenta"].HeaderText = "Cuenta";
                dgvMovimientos.Columns["numero_cuenta"].Width = 100;
                dgvMovimientos.Columns["monto"].HeaderText = "Monto";
                dgvMovimientos.Columns["monto"].Width = 90;
                dgvMovimientos.Columns["descripcion"].HeaderText = "Descripción";
                dgvMovimientos.Columns["descripcion"].Width = 120;

                dgvMovimientos.Columns["monto"].DefaultCellStyle.Format = "C2";
                dgvMovimientos.Columns["monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMovimientos.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";

                // Colorear movimientos
                foreach (DataGridViewRow row in dgvMovimientos.Rows)
                {
                    if (row.Cells["tipo_movimiento"].Value?.ToString() == "PAGO PROVEEDOR")
                    {
                        row.Cells["monto"].Style.ForeColor = Color.Red;
                    }
                    else if (row.Cells["tipo_movimiento"].Value?.ToString() == "DEPÓSITO")
                    {
                        row.Cells["monto"].Style.ForeColor = Color.Green;
                    }
                }
            }
        }

        private void btnBuscarBancos_Click(object sender, EventArgs e)
        {
            RealizarBusquedaBancos();
        }

        private void txtBuscarBancos_TextChanged(object sender, EventArgs e)
        {
            if (timerBusqueda == null)
            {
                timerBusqueda = new Timer();
                timerBusqueda.Interval = 1000;
                timerBusqueda.Tick += (s, args) =>
                {
                    timerBusqueda.Stop();
                    RealizarBusquedaBancos();
                };
            }

            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void RealizarBusquedaBancos()
        {
            string filtro = txtBuscarBancos.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                CargarBancos();
                return;
            }

            try
            {
                string query = $@"SELECT id_banco, nombre_banco, numero_cuenta, saldo 
                                FROM Bancos 
                                WHERE estado = 1 
                                AND (nombre_banco LIKE '%{filtro}%' 
                                     OR numero_cuenta LIKE '%{filtro}%')";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvBancos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoBanco_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            bancoIdActual = 0;
            LimpiarFormulario();
            HabilitarControles(true);
            txtNombreBanco.Focus();
        }

        private void btnEditarBanco_Click(object sender, EventArgs e)
        {
            if (dgvBancos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una cuenta para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            modoEdicion = true;
            DataGridViewRow row = dgvBancos.SelectedRows[0];
            bancoIdActual = Convert.ToInt32(row.Cells["id_banco"].Value);

            CargarDatosDesdeFila(row);
            HabilitarControles(true);
        }

        private void CargarDatosDesdeFila(DataGridViewRow row)
        {
            txtNombreBanco.Text = row.Cells["nombre_banco"].Value.ToString();
            txtNumeroCuenta.Text = row.Cells["numero_cuenta"].Value.ToString();
            txtSaldoInicial.Text = Convert.ToDecimal(row.Cells["saldo"].Value).ToString("F2");
        }

        private void btnGuardarBanco_Click(object sender, EventArgs e)
        {
            if (!ValidarDatosBanco())
                return;

            try
            {
                if (modoEdicion)
                {
                    ActualizarBanco();
                }
                else
                {
                    InsertarBanco();
                }

                CargarBancos();
                CargarMovimientos();
                LimpiarFormulario();
                HabilitarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertarBanco()
        {
            SqlParameter[] parameters = {
            new SqlParameter("@nombre_banco", txtNombreBanco.Text.Trim()),
            new SqlParameter("@numero_cuenta", txtNumeroCuenta.Text.Trim()),
            new SqlParameter("@saldo", decimal.Parse(txtSaldoInicial.Text))
            };

            // Llamamos al SP sp_Bancos_Insertar
            DataTable result = dbHelper.ExecuteStoredProcedure("sp_Bancos_Insertar", parameters);

            if (result != null && result.Rows.Count > 0)
            {
                MessageBox.Show("Cuenta bancaria agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarBanco()
        {
            SqlParameter[] parameters = {
            new SqlParameter("@id_banco", bancoIdActual),
            new SqlParameter("@nombre_banco", txtNombreBanco.Text.Trim()),
            new SqlParameter("@numero_cuenta", txtNumeroCuenta.Text.Trim()),
            new SqlParameter("@saldo", decimal.Parse(txtSaldoInicial.Text))
            };

            // Llamamos al SP sp_Bancos_Actualizar
            dbHelper.ExecuteStoredProcedure("sp_Bancos_Actualizar", parameters);

            MessageBox.Show("Cuenta bancaria actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminarBanco_Click(object sender, EventArgs e)
        {
            if (dgvBancos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una cuenta para eliminar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar esta cuenta bancaria?", "Confirmar",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataGridViewRow row = dgvBancos.SelectedRows[0];
                int idBanco = Convert.ToInt32(row.Cells["id_banco"].Value);

                SqlParameter[] parameters = { new SqlParameter("@id_banco", idBanco) };

                // Llamamos al SP sp_Bancos_Eliminar
                dbHelper.ExecuteStoredProcedure("sp_Bancos_Eliminar", parameters);

                MessageBox.Show("Cuenta bancaria eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarBancos();
                LimpiarFormulario();
            }
        }

        private void btnCancelarBanco_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private bool ValidarDatosBanco()
        {
            if (string.IsNullOrWhiteSpace(txtNombreBanco.Text))
            {
                MessageBox.Show("El nombre del banco es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreBanco.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumeroCuenta.Text))
            {
                MessageBox.Show("El número de cuenta es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumeroCuenta.Focus();
                return false;
            }

            if (!decimal.TryParse(txtSaldoInicial.Text, out decimal saldo) || saldo < 0)
            {
                MessageBox.Show("El saldo inicial debe ser un número mayor o igual a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSaldoInicial.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtNombreBanco.Clear();
            txtNumeroCuenta.Clear();
            txtSaldoInicial.Text = "0.00";
            bancoIdActual = 0;
            modoEdicion = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtNombreBanco.Enabled = habilitar;
            txtNumeroCuenta.Enabled = habilitar;
            txtSaldoInicial.Enabled = habilitar;
            btnGuardarBanco.Enabled = habilitar;
            btnCancelarBanco.Enabled = habilitar;

            btnNuevoBanco.Enabled = !habilitar;
            btnEditarBanco.Enabled = !habilitar;
            btnEliminarBanco.Enabled = !habilitar;
            btnBuscarBancos.Enabled = !habilitar;
            txtBuscarBancos.Enabled = !habilitar;
            btnLimpiarBusqueda.Enabled = !habilitar;
        }

        private void dgvBancos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBancos.SelectedRows.Count > 0 && !modoEdicion)
            {
                DataGridViewRow row = dgvBancos.SelectedRows[0];
                CargarDatosDesdeFila(row);
            }
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarBancos.Clear();
            CargarBancos();
            txtBuscarBancos.Focus();
        }

        private void btnRefrescarMovimientos_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
        }

        private void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            frmDepositos formDepositos = new frmDepositos();
            formDepositos.ShowDialog();
            CargarBancos();
            CargarMovimientos();
        }

        private void txtSaldoInicial_KeyPress(object sender, KeyPressEventArgs e)
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