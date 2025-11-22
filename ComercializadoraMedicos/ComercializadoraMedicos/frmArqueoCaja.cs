using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmArqueoCaja : Form
    {
        private DatabaseHelper dbHelper;

        public frmArqueoCaja()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmArqueoCaja_Load(object sender, EventArgs e)
        {
            dtpFechaArqueo.Value = DateTime.Now;
            CargarDatosDelDia();
            CalcularTotales();
        }

        private void CargarDatosDelDia()
        {
            try
            {
                string fecha = dtpFechaArqueo.Value.ToString("yyyy-MM-dd");

                // Cargar ventas al contado del día
                string queryVentasContado = $@"
                    SELECT ISNULL(SUM(total), 0) as total_ventas_contado
                    FROM Ventas 
                    WHERE fecha_venta = '{fecha}' 
                    AND tipo_venta = 'Contado' 
                    AND estado = 'Pagada'";

                DataTable dtVentasContado = dbHelper.ExecuteQuery(queryVentasContado);
                if (dtVentasContado.Rows.Count > 0)
                {
                    txtVentasContado.Text = Convert.ToDecimal(dtVentasContado.Rows[0]["total_ventas_contado"]).ToString("F2");
                }

                // Cargar pagos de clientes del día
                string queryPagosClientes = $@"
                    SELECT ISNULL(SUM(monto), 0) as total_pagos
                    FROM PagosClientes 
                    WHERE fecha_pago = '{fecha}' 
                    AND estado = 'Aplicado'";

                DataTable dtPagosClientes = dbHelper.ExecuteQuery(queryPagosClientes);
                if (dtPagosClientes.Rows.Count > 0)
                {
                    txtPagosRecibidos.Text = Convert.ToDecimal(dtPagosClientes.Rows[0]["total_pagos"]).ToString("F2");
                }

                // Cargar ventas a crédito del día (solo para información)
                string queryVentasCredito = $@"
                    SELECT ISNULL(SUM(total), 0) as total_ventas_credito
                    FROM Ventas 
                    WHERE fecha_venta = '{fecha}' 
                    AND tipo_venta = 'Credito' 
                    AND estado = 'Pagada'";

                DataTable dtVentasCredito = dbHelper.ExecuteQuery(queryVentasCredito);
                if (dtVentasCredito.Rows.Count > 0)
                {
                    lblVentasCredito.Text = $"Ventas a Crédito: {Convert.ToDecimal(dtVentasCredito.Rows[0]["total_ventas_credito"]):C2}";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del día: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcularTotales()
        {
            try
            {
                decimal ventasContado = string.IsNullOrEmpty(txtVentasContado.Text) ? 0 : decimal.Parse(txtVentasContado.Text);
                decimal pagosRecibidos = string.IsNullOrEmpty(txtPagosRecibidos.Text) ? 0 : decimal.Parse(txtPagosRecibidos.Text);
                decimal totalDepositado = string.IsNullOrEmpty(txtTotalDepositado.Text) ? 0 : decimal.Parse(txtTotalDepositado.Text);

                // Calcular total recaudado
                decimal totalRecaudado = ventasContado + pagosRecibidos;
                lblTotalRecaudado.Text = totalRecaudado.ToString("C2");

                // Calcular diferencia
                decimal diferencia = totalRecaudado - totalDepositado;
                lblDiferencia.Text = diferencia.ToString("C2");

                // Formatear diferencia
                if (diferencia == 0)
                {
                    lblDiferencia.ForeColor = System.Drawing.Color.Green;
                    lblEstado.Text = "✅ CUADRA";
                    lblEstado.ForeColor = System.Drawing.Color.Green;
                }
                else if (diferencia > 0)
                {
                    lblDiferencia.ForeColor = System.Drawing.Color.Orange;
                    lblEstado.Text = "⚠️ SOBRANTE";
                    lblEstado.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lblDiferencia.ForeColor = System.Drawing.Color.Red;
                    lblEstado.Text = "❌ FALTANTE";
                    lblEstado.ForeColor = System.Drawing.Color.Red;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en cálculos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                string fecha = dtpFechaArqueo.Value.ToString("yyyy-MM-dd");
                decimal ventasContado = decimal.Parse(txtVentasContado.Text);
                decimal pagosRecibidos = decimal.Parse(txtPagosRecibidos.Text);
                decimal totalDepositado = decimal.Parse(txtTotalDepositado.Text);
                decimal diferencia = decimal.Parse(lblDiferencia.Text.Replace("Q", "").Replace(",", ""));

                // Verificar si ya existe un arqueo para esta fecha
                string queryVerificar = $"SELECT COUNT(*) as existe FROM ArqueosCaja WHERE fecha_arqueo = '{fecha}'";
                DataTable dtVerificar = dbHelper.ExecuteQuery(queryVerificar);
                bool existe = Convert.ToInt32(dtVerificar.Rows[0]["existe"]) > 0;

                if (existe)
                {
                    MessageBox.Show("Ya existe un arqueo para esta fecha. La edición requiere un procedimiento adicional.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // O implementa sp_ArqueosCaja_Actualizar en SQL si deseas editar
                }
                else
                {
                    // Usamos sp_ArqueosCaja_Insertar
                    SqlParameter[] parameters = {
                    new SqlParameter("@total_ventas_contado", ventasContado),
                    new SqlParameter("@total_pagos_recibidos", pagosRecibidos),
                    new SqlParameter("@total_depositado", totalDepositado),
                    new SqlParameter("@observaciones", txtObservaciones.Text)
                    };

                    // El SP calcula la diferencia internamente

                    dbHelper.ExecuteStoredProcedure("sp_ArqueosCaja_Insertar", parameters);

                    MessageBox.Show("Arqueo de caja guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el arqueo: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (!decimal.TryParse(txtVentasContado.Text, out decimal ventasContado) || ventasContado < 0)
            {
                MessageBox.Show("El total de ventas al contado debe ser un número válido.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVentasContado.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPagosRecibidos.Text, out decimal pagosRecibidos) || pagosRecibidos < 0)
            {
                MessageBox.Show("El total de pagos recibidos debe ser un número válido.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPagosRecibidos.Focus();
                return false;
            }

            if (!decimal.TryParse(txtTotalDepositado.Text, out decimal totalDepositado) || totalDepositado < 0)
            {
                MessageBox.Show("El total depositado debe ser un número válido.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTotalDepositado.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtVentasContado.Text = "0";
            txtPagosRecibidos.Text = "0";
            txtTotalDepositado.Text = "0";
            txtObservaciones.Clear();
            CargarDatosDelDia();
            CalcularTotales();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpFechaArqueo_ValueChanged(object sender, EventArgs e)
        {
            CargarDatosDelDia();
            CalcularTotales();
        }

        // Eventos para cálculo automático
        private void txtVentasContado_TextChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        private void txtPagosRecibidos_TextChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        private void txtTotalDepositado_TextChanged(object sender, EventArgs e)
        {
            CalcularTotales();
        }

        // Validación para campos numéricos
        private void ValidarNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        private void btnVerReporte_Click(object sender, EventArgs e)
        {
            try
            {
                string fecha = dtpFechaArqueo.Value.ToString("yyyy-MM-dd");
                string query = $@"
                    SELECT 
                        fecha_arqueo as Fecha,
                        total_ventas_contado as [Ventas Contado],
                        total_pagos_recibidos as [Pagos Recibidos],
                        total_depositado as [Total Depositado],
                        diferencia as Diferencia,
                        observaciones as Observaciones
                    FROM ArqueosCaja 
                    WHERE fecha_arqueo = '{fecha}'";

                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    string mensaje = $"📊 ARQUEO DE CAJA - {fecha}\n\n";
                    mensaje += $"Ventas al Contado: {Convert.ToDecimal(dt.Rows[0]["Ventas Contado"]):C2}\n";
                    mensaje += $"Pagos Recibidos: {Convert.ToDecimal(dt.Rows[0]["Pagos Recibidos"]):C2}\n";
                    mensaje += $"Total Depositado: {Convert.ToDecimal(dt.Rows[0]["Total Depositado"]):C2}\n";
                    mensaje += $"Diferencia: {Convert.ToDecimal(dt.Rows[0]["Diferencia"]):C2}\n";

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Observaciones"].ToString()))
                    {
                        mensaje += $"\nObservaciones: {dt.Rows[0]["Observaciones"]}";
                    }

                    MessageBox.Show(mensaje, "Reporte de Arqueo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encontró un arqueo registrado para esta fecha.", "Información",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}