using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmReportes : Form
    {
        private DatabaseHelper dbHelper;

        public frmReportes()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmReportes_Load(object sender, EventArgs e)
        {
            CargarReportesDisponibles();
        }

        private void CargarReportesDisponibles()
        {
            // Lista de reportes disponibles
            cmbReportes.Items.AddRange(new string[] {
                "📊 Saldo de Proveedores",
                "📦 Existencias por Bodega",
                "📋 Órdenes Recibidas o Pendientes",
                "💰 Movimientos del Día",
                "🏦 Saldo de Cuentas Bancarias",
                "⚠️ Productos con Stock Bajo",
                "👥 Estado de Cuenta de Clientes",
                "📈 Análisis ABC de Productos"
            });
            cmbReportes.SelectedIndex = 0;
        }

        private void cmbReportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReportes.SelectedIndex >= 0)
            {
                CargarReporteSeleccionado();
            }
        }

        private void CargarReporteSeleccionado()
        {
            try
            {
                string reporteSeleccionado = cmbReportes.SelectedItem.ToString();
                DataTable datos = new DataTable();

                switch (reporteSeleccionado)
                {
                    case "📊 Saldo de Proveedores":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vSaldoProveedores ORDER BY saldo_actual DESC");
                        break;

                    case "📦 Existencias por Bodega":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vExistenciasPorBodega ORDER BY bodega, existencia DESC");
                        break;

                    case "📋 Órdenes Recibidas o Pendientes":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vOrdenesEstado ORDER BY fecha_orden DESC");
                        break;

                    case "💰 Movimientos del Día":
                        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                        datos = dbHelper.ExecuteQuery($"SELECT * FROM vMovimientosDia WHERE fecha = '{fechaHoy}' ORDER BY fecha DESC");
                        break;

                    case "🏦 Saldo de Cuentas Bancarias":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vSaldoCuentasBancarias ORDER BY saldo DESC");
                        break;

                    case "⚠️ Productos con Stock Bajo":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vProductosStockBajo ORDER BY faltante DESC");
                        break;

                    case "👥 Estado de Cuenta de Clientes":
                        datos = dbHelper.ExecuteQuery("SELECT * FROM vEstadoCuentaClientes ORDER BY saldo_actual DESC");
                        break;

                    case "📈 Análisis ABC de Productos":
                        string fechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                        string fechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                        datos = dbHelper.ExecuteQuery($"SELECT * FROM fnObtenerAnalisisABC('{fechaInicio}', '{fechaFin}')");
                        break;
                }

                dgvReportes.DataSource = datos;
                AplicarFormatoReporte(reporteSeleccionado);

                // Habilitar/deshabilitar exportación según si hay datos
                btnExportarExcel.Enabled = datos.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFormatoReporte(string reporteSeleccionado)
        {
            if (dgvReportes.Columns.Count == 0) return;

            // Formato general para todas las columnas monetarias
            foreach (DataGridViewColumn columna in dgvReportes.Columns)
            {
                if (columna.Name.Contains("saldo") ||
                    columna.Name.Contains("limite") ||
                    columna.Name.Contains("credito") ||
                    columna.Name.Contains("precio") ||
                    columna.Name.Contains("total") ||
                    columna.Name.Contains("monto") ||
                    columna.Name.Contains("valor"))
                {
                    columna.DefaultCellStyle.Format = "C2";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Ajustar ancho de columnas
                columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Formato específico por reporte
            switch (reporteSeleccionado)
            {
                case "📊 Saldo de Proveedores":
                    AplicarFormatoCondicional("nivel_riesgo", "Alto", Color.Red, Color.White);
                    break;

                case "⚠️ Productos con Stock Bajo":
                    AplicarFormatoCondicional("estado_inventario", "AGOTADO", Color.Red, Color.White);
                    AplicarFormatoCondicional("estado_inventario", "STOCK BAJO", Color.Orange, Color.Black);
                    break;

                case "👥 Estado de Cuenta de Clientes":
                    AplicarFormatoCondicional("nivel_riesgo_credito", "ALTO RIESGO", Color.Red, Color.White);
                    break;
            }
        }

        private void AplicarFormatoCondicional(string nombreColumna, string valor, Color colorFondo, Color colorTexto)
        {
            if (dgvReportes.Columns[nombreColumna] != null)
            {
                foreach (DataGridViewRow fila in dgvReportes.Rows)
                {
                    if (fila.Cells[nombreColumna].Value?.ToString() == valor)
                    {
                        fila.Cells[nombreColumna].Style.BackColor = colorFondo;
                        fila.Cells[nombreColumna].Style.ForeColor = colorTexto;
                        fila.Cells[nombreColumna].Style.Font = new Font(dgvReportes.Font, FontStyle.Bold);
                    }
                }
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvReportes.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivo CSV (*.csv)|*.csv";
                saveFileDialog.Title = "Exportar reporte a Excel";
                saveFileDialog.FileName = $"Reporte_{cmbReportes.SelectedItem.ToString().Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveFileDialog.FileName);
                    MessageBox.Show($"Reporte exportado exitosamente a:\n{saveFileDialog.FileName}", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarACSV(string filePath)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Escribir encabezados
                for (int i = 0; i < dgvReportes.Columns.Count; i++)
                {
                    sw.Write(dgvReportes.Columns[i].HeaderText);
                    if (i < dgvReportes.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                // Escribir datos
                foreach (DataGridViewRow row in dgvReportes.Rows)
                {
                    for (int i = 0; i < dgvReportes.Columns.Count; i++)
                    {
                        if (row.Cells[i].Value != null)
                        {
                            string valor = row.Cells[i].Value.ToString();
                            // Escapar comas y comillas
                            if (valor.Contains(",") || valor.Contains("\"") || valor.Contains("\n"))
                            {
                                valor = "\"" + valor.Replace("\"", "\"\"") + "\"";
                            }
                            sw.Write(valor);
                        }
                        if (i < dgvReportes.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();
                }
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarReporteSeleccionado();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvReportes.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Funcionalidad de impresión será implementada en la siguiente versión.", "Información",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (dgvReportes.DataSource != null)
            {
                DataTable dt = (DataTable)dgvReportes.DataSource;
                string filtro = txtFiltro.Text.Trim();

                if (string.IsNullOrEmpty(filtro))
                {
                    dt.DefaultView.RowFilter = "";
                }
                else
                {
                    string filterExpression = "";
                    foreach (DataColumn columna in dt.Columns)
                    {
                        if (filterExpression != "") filterExpression += " OR ";
                        filterExpression += $"{columna.ColumnName} LIKE '%{filtro}%'";
                    }
                    dt.DefaultView.RowFilter = filterExpression;
                }
            }
        }
    }
}
