using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmPagosClientes : Form
    {
        private DatabaseHelper dbHelper;
        private int clienteIdActual = 0;
        private decimal saldoPendienteCliente = 0;

        public frmPagosClientes()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmPagosClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
            LimpiarFormulario();
        }

        private void CargarClientes()
        {
            try
            {
                string query = @"SELECT id_cliente, nombre, saldo_actual 
                               FROM Clientes 
                               WHERE estado = 1 AND saldo_actual > 0 
                               ORDER BY nombre";
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

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue != null && cmbCliente.SelectedValue is int)
            {
                clienteIdActual = (int)cmbCliente.SelectedValue;
                CargarVentasPendientes();
                ActualizarInfoCliente();
            }
        }

        private void ActualizarInfoCliente()
        {
            try
            {
                string query = $"SELECT saldo_actual FROM Clientes WHERE id_cliente = {clienteIdActual}";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    saldoPendienteCliente = Convert.ToDecimal(dt.Rows[0]["saldo_actual"]);
                    lblSaldoPendiente.Text = $"Saldo Pendiente: {saldoPendienteCliente:C2}";

                    // Actualizar máximo permitido
                    numMontoPago.Maximum = saldoPendienteCliente;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar información del cliente: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVentasPendientes()
        {
            try
            {
                string query = $@"SELECT 
                                    v.id_venta,
                                    v.fecha_venta,
                                    v.total as total_venta,
                                    ISNULL((SELECT SUM(monto) FROM PagosClientes WHERE id_venta = v.id_venta), 0) as total_pagado,
                                    (v.total - ISNULL((SELECT SUM(monto) FROM PagosClientes WHERE id_venta = v.id_venta), 0)) as saldo_pendiente
                                FROM Ventas v
                                WHERE v.id_cliente = {clienteIdActual} 
                                AND v.tipo_venta = 'Credito'
                                AND (v.total - ISNULL((SELECT SUM(monto) FROM PagosClientes WHERE id_venta = v.id_venta), 0)) > 0
                                ORDER BY v.fecha_venta";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvVentasPendientes.DataSource = dt;

                ConfigurarGridVentas();
                CalcularTotalPendiente();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas pendientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGridVentas()
        {
            if (dgvVentasPendientes.Columns.Count > 0)
            {
                dgvVentasPendientes.Columns["id_venta"].HeaderText = "N° Venta";
                dgvVentasPendientes.Columns["id_venta"].Width = 70;
                dgvVentasPendientes.Columns["fecha_venta"].HeaderText = "Fecha";
                dgvVentasPendientes.Columns["fecha_venta"].Width = 90;
                dgvVentasPendientes.Columns["total_venta"].HeaderText = "Total Venta";
                dgvVentasPendientes.Columns["total_venta"].Width = 100;
                dgvVentasPendientes.Columns["total_pagado"].HeaderText = "Total Pagado";
                dgvVentasPendientes.Columns["total_pagado"].Width = 100;
                dgvVentasPendientes.Columns["saldo_pendiente"].HeaderText = "Saldo Pendiente";
                dgvVentasPendientes.Columns["saldo_pendiente"].Width = 100;

                // Formato de moneda
                dgvVentasPendientes.Columns["total_venta"].DefaultCellStyle.Format = "C2";
                dgvVentasPendientes.Columns["total_pagado"].DefaultCellStyle.Format = "C2";
                dgvVentasPendientes.Columns["saldo_pendiente"].DefaultCellStyle.Format = "C2";
            }
        }

        private void CalcularTotalPendiente()
        {
            decimal totalPendiente = 0;
            foreach (DataGridViewRow row in dgvVentasPendientes.Rows)
            {
                if (row.Cells["saldo_pendiente"].Value != null)
                {
                    totalPendiente += Convert.ToDecimal(row.Cells["saldo_pendiente"].Value);
                }
            }
            lblTotalPendiente.Text = $"Total Pendiente: {totalPendiente:C2}";
        }

        private void numMontoPago_ValueChanged(object sender, EventArgs e)
        {
            decimal montoPago = numMontoPago.Value;

            if (montoPago > saldoPendienteCliente)
            {
                lblMensaje.Text = "⚠️ El monto excede el saldo pendiente";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                btnProcesarPago.Enabled = false;
            }
            else if (montoPago <= 0)
            {
                lblMensaje.Text = "ℹ️ Ingrese un monto válido";
                lblMensaje.ForeColor = System.Drawing.Color.Black;
                btnProcesarPago.Enabled = false;
            }
            else
            {
                lblMensaje.Text = "✅ Monto válido";
                lblMensaje.ForeColor = System.Drawing.Color.Green;
                btnProcesarPago.Enabled = true;
            }
        }

        private void btnProcesarPago_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un cliente.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal montoPago = numMontoPago.Value;

            if (montoPago <= 0)
            {
                MessageBox.Show("Por favor ingrese un monto válido mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMontoPago.Focus();
                return;
            }

            if (montoPago > saldoPendienteCliente)
            {
                MessageBox.Show("El monto del pago no puede ser mayor al saldo pendiente del cliente.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirmar el pago
            var result = MessageBox.Show($"¿Está seguro de procesar el pago por {montoPago:C2}?\n\nCliente: {cmbCliente.Text}",
                                       "Confirmar Pago",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarPago(montoPago);
            }
        }

        private void ProcesarPago(decimal montoPago)
        {
            try
            {
                // Usar el procedimiento almacenado para insertar el pago
                string query = $@"INSERT INTO PagosClientes (id_cliente, fecha_pago, monto, estado)
                                VALUES ({clienteIdActual}, GETDATE(), {montoPago}, 'Aplicado')";

                int affected = dbHelper.ExecuteNonQuery(query);

                if (affected > 0)
                {
                    // Actualizar saldo del cliente
                    string updateCliente = $@"UPDATE Clientes 
                                            SET saldo_actual = saldo_actual - {montoPago}
                                            WHERE id_cliente = {clienteIdActual}";
                    dbHelper.ExecuteNonQuery(updateCliente);

                    // Aplicar el pago a las ventas pendientes (método FIFO)
                    AplicarPagoAVentas(montoPago);

                    MessageBox.Show($"✅ Pago procesado exitosamente!\n\n" +
                                  $"Cliente: {cmbCliente.Text}\n" +
                                  $"Monto: {montoPago:C2}",
                                  "Pago Exitoso",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarClientes(); // Recargar clientes para actualizar saldos
                }
                else
                {
                    MessageBox.Show("❌ No se pudo procesar el pago.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar el pago: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarPagoAVentas(decimal montoPago)
        {
            try
            {
                decimal montoRestante = montoPago;

                // Obtener ventas pendientes ordenadas por fecha (más antiguas primero)
                string queryVentas = $@"SELECT 
                                        id_venta,
                                        (total - ISNULL((SELECT SUM(monto) FROM PagosClientes WHERE id_venta = v.id_venta), 0)) as saldo_pendiente
                                    FROM Ventas v
                                    WHERE id_cliente = {clienteIdActual} 
                                    AND tipo_venta = 'Credito'
                                    AND (total - ISNULL((SELECT SUM(monto) FROM PagosClientes WHERE id_venta = v.id_venta), 0)) > 0
                                    ORDER BY fecha_venta";

                DataTable ventasPendientes = dbHelper.ExecuteQuery(queryVentas);

                foreach (DataRow venta in ventasPendientes.Rows)
                {
                    if (montoRestante <= 0) break;

                    int idVenta = Convert.ToInt32(venta["id_venta"]);
                    decimal saldoPendienteVenta = Convert.ToDecimal(venta["saldo_pendiente"]);

                    decimal montoAAplicar = Math.Min(montoRestante, saldoPendienteVenta);

                    montoRestante -= montoAAplicar;

                    // Verificar si la venta queda completamente pagada
                    if (montoAAplicar >= saldoPendienteVenta)
                    {
                        string updateVenta = $"UPDATE Ventas SET estado = 'Pagada' WHERE id_venta = {idVenta}";
                        dbHelper.ExecuteNonQuery(updateVenta);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Advertencia: El pago se registró pero hubo un error al aplicarlo a ventas específicas: {ex.Message}",
                              "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimpiarFormulario()
        {
            numMontoPago.Value = 0;
            lblMensaje.Text = "";
            lblSaldoPendiente.Text = "Saldo Pendiente: L0.00";
            lblTotalPendiente.Text = "Total Pendiente: L0.00";
            btnProcesarPago.Enabled = false;

            // Limpiar DataGridView
            if (dgvVentasPendientes.DataSource != null)
            {
                DataTable dt = (DataTable)dgvVentasPendientes.DataSource;
                dt.Clear();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}