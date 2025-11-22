using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmPagosProveedores : Form
    {
        private DatabaseHelper dbHelper;
        private int proveedorIdActual = 0;
        private int bancoIdActual = 0;
        private decimal saldoProveedor = 0;
        private decimal saldoBanco = 0;

        public frmPagosProveedores()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmPagosProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarBancos();
            CargarOrdenesPendientes();
            LimpiarFormulario();
        }

        private void CargarProveedores()
        {
            try
            {
                string query = "SELECT id_proveedor, nombre, saldo_actual FROM Proveedores WHERE estado = 1 AND saldo_actual > 0 ORDER BY nombre";
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

        private void CargarOrdenesPendientes()
        {
            try
            {
                if (proveedorIdActual > 0)
                {
                    string query = $@"SELECT oc.id_orden_compra, oc.fecha_orden, oc.total, oc.estado
                                    FROM OrdenesCompra oc
                                    WHERE oc.id_proveedor = {proveedorIdActual}
                                    AND oc.estado = 'Recibida'
                                    AND oc.total > ISNULL((SELECT SUM(pp.monto) 
                                                         FROM PagosProveedores pp 
                                                         WHERE pp.id_proveedor = {proveedorIdActual} 
                                                         AND pp.estado = 'Aplicado'), 0)
                                    ORDER BY oc.fecha_orden";

                    DataTable dt = dbHelper.ExecuteQuery(query);
                    dgvOrdenesPendientes.DataSource = dt;

                    // Configurar el DataGridView
                    if (dgvOrdenesPendientes.Columns.Count > 0)
                    {
                        dgvOrdenesPendientes.Columns["id_orden_compra"].HeaderText = "ID Orden";
                        dgvOrdenesPendientes.Columns["id_orden_compra"].Width = 80;
                        dgvOrdenesPendientes.Columns["fecha_orden"].HeaderText = "Fecha";
                        dgvOrdenesPendientes.Columns["fecha_orden"].Width = 100;
                        dgvOrdenesPendientes.Columns["total"].HeaderText = "Total";
                        dgvOrdenesPendientes.Columns["total"].Width = 100;
                        dgvOrdenesPendientes.Columns["estado"].HeaderText = "Estado";
                        dgvOrdenesPendientes.Columns["estado"].Width = 80;

                        dgvOrdenesPendientes.Columns["total"].DefaultCellStyle.Format = "C2";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar órdenes pendientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue != null && cmbProveedor.SelectedValue is int)
            {
                proveedorIdActual = (int)cmbProveedor.SelectedValue;
                ActualizarInfoProveedor();
                CargarOrdenesPendientes();
            }
        }

        private void cmbBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBanco.SelectedValue != null && cmbBanco.SelectedValue is int)
            {
                bancoIdActual = (int)cmbBanco.SelectedValue;
                ActualizarInfoBanco();
            }
        }

        private void ActualizarInfoProveedor()
        {
            try
            {
                string query = $"SELECT saldo_actual, limite_credito FROM Proveedores WHERE id_proveedor = {proveedorIdActual}";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    saldoProveedor = Convert.ToDecimal(dt.Rows[0]["saldo_actual"]);
                    decimal limiteCredito = Convert.ToDecimal(dt.Rows[0]["limite_credito"]);

                    lblSaldoProveedor.Text = $"Saldo Pendiente: {saldoProveedor:C2}";
                    lblLimiteCredito.Text = $"Límite Crédito: {limiteCredito:C2}";

                    // Actualizar validación en tiempo real
                    ValidarMontoEnTiempoReal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del proveedor: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarInfoBanco()
        {
            try
            {
                string query = $"SELECT saldo FROM Bancos WHERE id_banco = {bancoIdActual}";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    saldoBanco = Convert.ToDecimal(dt.Rows[0]["saldo"]);
                    lblSaldoBanco.Text = $"Saldo Disponible: {saldoBanco:C2}";

                    // Actualizar validación en tiempo real
                    ValidarMontoEnTiempoReal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del banco: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMonto_TextChanged(object sender, EventArgs e)
        {
            ValidarMontoEnTiempoReal();
        }

        private void ValidarMontoEnTiempoReal()
        {
            if (decimal.TryParse(txtMonto.Text, out decimal monto) && monto > 0)
            {
                // Validar contra saldo del proveedor
                if (monto > saldoProveedor)
                {
                    lblMensajeValidacion.Text = "❌ Monto mayor al saldo del proveedor";
                    lblMensajeValidacion.ForeColor = System.Drawing.Color.Red;
                    btnProcesarPago.Enabled = false;
                }
                // Validar contra saldo del banco
                else if (monto > saldoBanco)
                {
                    lblMensajeValidacion.Text = "❌ Fondos insuficientes en el banco";
                    lblMensajeValidacion.ForeColor = System.Drawing.Color.Red;
                    btnProcesarPago.Enabled = false;
                }
                else
                {
                    lblMensajeValidacion.Text = "✅ Monto válido";
                    lblMensajeValidacion.ForeColor = System.Drawing.Color.Green;
                    btnProcesarPago.Enabled = true;
                }

                // Mostrar nuevo saldo proyectado
                decimal nuevoSaldoProveedor = saldoProveedor - monto;
                decimal nuevoSaldoBanco = saldoBanco - monto;

                lblProyeccionProveedor.Text = $"Nuevo saldo: {nuevoSaldoProveedor:C2}";
                lblProyeccionBanco.Text = $"Nuevo saldo: {nuevoSaldoBanco:C2}";
            }
            else
            {
                lblMensajeValidacion.Text = "Ingrese un monto válido";
                lblMensajeValidacion.ForeColor = System.Drawing.Color.Black;
                btnProcesarPago.Enabled = false;

                lblProyeccionProveedor.Text = "Nuevo saldo: --";
                lblProyeccionBanco.Text = "Nuevo saldo: --";
            }
        }

        private void btnProcesarPago_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            // Confirmar el pago
            var result = MessageBox.Show($"¿Está seguro de procesar el pago por {decimal.Parse(txtMonto.Text):C2}?", "Confirmar Pago",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarPago();
            }
        }

        private bool ValidarDatos()
        {
            if (cmbProveedor.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un proveedor.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbBanco.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un banco.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("El monto debe ser un número mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonto.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(cmbTipoPago.Text))
            {
                MessageBox.Show("Por favor seleccione el tipo de pago.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipoPago.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNumeroDocumento.Text))
            {
                MessageBox.Show("Por favor ingrese el número de documento.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumeroDocumento.Focus();
                return false;
            }

            // Validar saldos
            if (monto > saldoProveedor)
            {
                MessageBox.Show("El monto excede el saldo del proveedor.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (monto > saldoBanco)
            {
                MessageBox.Show("El banco no tiene fondos suficientes.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ProcesarPago()
        {
            try
            {
                decimal monto = decimal.Parse(txtMonto.Text);
                string tipoPago = cmbTipoPago.Text;
                string numeroDocumento = txtNumeroDocumento.Text;

                // Preparar facturas para aplicar el pago (si se seleccionaron órdenes)
                string facturasJSON = "NULL";
                if (dgvOrdenesPendientes.SelectedRows.Count > 0)
                {
                    facturasJSON = GenerarJSONFacturas();
                }

                // Llamar al procedimiento almacenado
                string query = $@"EXEC spTransaccionPagoProveedores 
                                {proveedorIdActual}, 
                                {bancoIdActual}, 
                                {monto}, 
                                '{tipoPago}', 
                                '{numeroDocumento}'";

                DataTable result = dbHelper.ExecuteQuery(query);

                if (result.Rows.Count > 0)
                {
                    string resultado = result.Rows[0]["resultado"].ToString();
                    if (resultado == "Éxito")
                    {
                        int idPago = Convert.ToInt32(result.Rows[0]["id_pago_proveedor"]);
                        decimal montoPagado = Convert.ToDecimal(result.Rows[0]["monto_pagado"]);

                        MessageBox.Show($"✅ Pago procesado exitosamente!\n\n" +
                                      $"Número de pago: {idPago}\n" +
                                      $"Monto: {montoPagado:C2}\n" +
                                      $"Tipo: {tipoPago}\n" +
                                      $"Documento: {numeroDocumento}",
                                      "Pago Exitoso",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarFormulario();
                        CargarProveedores(); // Recargar para actualizar saldos
                        CargarBancos(); // Recargar para actualizar saldos
                    }
                    else
                    {
                        string mensaje = result.Rows[0]["mensaje"].ToString();
                        MessageBox.Show($"❌ Error al procesar el pago: {mensaje}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar el pago: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarJSONFacturas()
        {
            // Crear JSON manualmente para las facturas seleccionadas
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            json.Append("[");

            int count = 0;
            foreach (DataGridViewRow row in dgvOrdenesPendientes.SelectedRows)
            {
                if (count > 0)
                    json.Append(",");

                json.Append("{");
                json.Append($"\"id_orden_compra\": {row.Cells["id_orden_compra"].Value},");
                json.Append($"\"monto_aplicado\": {row.Cells["total"].Value}");
                json.Append("}");

                count++;
            }

            json.Append("]");
            return json.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarFormulario()
        {
            cmbProveedor.SelectedIndex = -1;
            cmbBanco.SelectedIndex = -1;
            cmbTipoPago.SelectedIndex = 0;
            txtMonto.Clear();
            txtNumeroDocumento.Clear();
            lblSaldoProveedor.Text = "Saldo Pendiente: L0.00";
            lblLimiteCredito.Text = "Límite Crédito: L0.00";
            lblSaldoBanco.Text = "Saldo Disponible: L0.00";
            lblMensajeValidacion.Text = "";
            lblProyeccionProveedor.Text = "Nuevo saldo: --";
            lblProyeccionBanco.Text = "Nuevo saldo: --";
            proveedorIdActual = 0;
            bancoIdActual = 0;
            saldoProveedor = 0;
            saldoBanco = 0;

            // Limpiar DataGridView
            if (dgvOrdenesPendientes.DataSource != null)
            {
                DataTable dt = (DataTable)dgvOrdenesPendientes.DataSource;
                dt.Clear();
            }
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

        private void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOrdenesPendientes.Rows)
            {
                row.Selected = true;
            }
        }

        private void btnDeseleccionarTodo_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOrdenesPendientes.Rows)
            {
                row.Selected = false;
            }
        }

        private void dgvOrdenesPendientes_SelectionChanged(object sender, EventArgs e)
        {
            CalcularTotalSeleccionado();
        }

        private void CalcularTotalSeleccionado()
        {
            decimal totalSeleccionado = 0;
            foreach (DataGridViewRow row in dgvOrdenesPendientes.SelectedRows)
            {
                if (row.Cells["total"].Value != null)
                {
                    totalSeleccionado += Convert.ToDecimal(row.Cells["total"].Value);
                }
            }

            lblTotalSeleccionado.Text = $"Total seleccionado: {totalSeleccionado:C2}";

            // Sugerir el monto automáticamente
            if (totalSeleccionado > 0 && totalSeleccionado <= saldoProveedor)
            {
                txtMonto.Text = totalSeleccionado.ToString("F2");
            }
        }
    }
}