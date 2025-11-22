using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmConsultaFacturas : Form
    {
        private DatabaseHelper dbHelper;

        public frmConsultaFacturas()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmConsultaFacturas_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarEstados();
            ConfigurarFechas();
            CargarFacturas();
        }

        private void CargarClientes()
        {
            try
            {
                string query = "SELECT id_cliente, nombre FROM Clientes WHERE estado = 1 ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbCliente.DataSource = dt;
                cmbCliente.DisplayMember = "nombre";
                cmbCliente.ValueMember = "id_cliente";

                // Agregar opción "Todos los clientes"
                cmbCliente.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstados()
        {
            cmbEstado.Items.AddRange(new string[] { "Todas", "Pagada", "Pendiente", "Cancelada" });
            cmbEstado.SelectedIndex = 0;
        }

        private void ConfigurarFechas()
        {
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;
        }

        private void CargarFacturas()
        {
            try
            {
                string query = ConstruirConsulta();
                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvFacturas.DataSource = dt;

                ConfigurarDataGridView();
                CalcularTotales(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar facturas: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConstruirConsulta()
        {
            string query = @"SELECT 
                            v.id_venta as 'Número Factura',
                            c.nombre as 'Cliente',
                            v.fecha_venta as 'Fecha',
                            v.tipo_venta as 'Tipo',
                            v.total as 'Total',
                            v.estado as 'Estado',
                            (SELECT COUNT(*) FROM VentasDetalle vd WHERE vd.id_venta = v.id_venta) as 'Items'
                            FROM Ventas v
                            INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
                            WHERE 1=1";

            // Filtro por cliente
            if (cmbCliente.SelectedValue != null)
            {
                query += $" AND v.id_cliente = {cmbCliente.SelectedValue}";
            }

            // Filtro por estado
            if (cmbEstado.SelectedIndex > 0)
            {
                query += $" AND v.estado = '{cmbEstado.SelectedItem}'";
            }

            // Filtro por fechas
            query += $" AND v.fecha_venta BETWEEN '{dtpFechaDesde.Value:yyyy-MM-dd}' AND '{dtpFechaHasta.Value:yyyy-MM-dd 23:59:59}'";

            // Orden
            query += " ORDER BY v.fecha_venta DESC, v.id_venta DESC";

            return query;
        }

        private void ConfigurarDataGridView()
        {
            if (dgvFacturas.Columns.Count > 0)
            {
                dgvFacturas.Columns["Número Factura"].Width = 80;
                dgvFacturas.Columns["Cliente"].Width = 150;
                dgvFacturas.Columns["Fecha"].Width = 90;
                dgvFacturas.Columns["Tipo"].Width = 80;
                dgvFacturas.Columns["Total"].Width = 90;
                dgvFacturas.Columns["Estado"].Width = 80;
                dgvFacturas.Columns["Items"].Width = 50;

                dgvFacturas.Columns["Total"].DefaultCellStyle.Format = "C2";
                dgvFacturas.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";

                AplicarFormatoCondicional();
            }
        }

        private void AplicarFormatoCondicional()
        {
            foreach (DataGridViewRow row in dgvFacturas.Rows)
            {
                string estado = row.Cells["Estado"].Value?.ToString();

                switch (estado)
                {
                    case "Pendiente":
                        row.Cells["Estado"].Style.ForeColor = Color.OrangeRed;
                        row.Cells["Estado"].Style.Font = new Font(dgvFacturas.Font, FontStyle.Bold);
                        break;
                    case "Pagada":
                        row.Cells["Estado"].Style.ForeColor = Color.Green;
                        break;
                    case "Cancelada":
                        row.Cells["Estado"].Style.ForeColor = Color.Gray;
                        break;
                }

                // Resaltar ventas a crédito
                string tipo = row.Cells["Tipo"].Value?.ToString();
                if (tipo == "Credito")
                {
                    row.Cells["Tipo"].Style.ForeColor = Color.Blue;
                    row.Cells["Tipo"].Style.Font = new Font(dgvFacturas.Font, FontStyle.Bold);
                }
            }
        }

        private void CalcularTotales(DataTable dt)
        {
            decimal totalGeneral = 0;
            int totalFacturas = dt.Rows.Count;
            int pendientes = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalGeneral += Convert.ToDecimal(row["Total"]);
                if (row["Estado"].ToString() == "Pendiente")
                    pendientes++;
            }

            lblResumen.Text = $@"Facturas: {totalFacturas} 
Total: {totalGeneral:C2}
Pendientes: {pendientes}";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarFacturas();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbCliente.SelectedIndex = -1;
            cmbEstado.SelectedIndex = 0;
            ConfigurarFechas();
            CargarFacturas();
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvFacturas.SelectedRows.Count > 0)
            {
                int idVenta = Convert.ToInt32(dgvFacturas.SelectedRows[0].Cells["Número Factura"].Value);
                VerDetalleFactura(idVenta);
            }
            else
            {
                MessageBox.Show("Por favor seleccione una factura para ver el detalle.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VerDetalleFactura(int idVenta)
        {
            try
            {
                // Obtener información de la cabecera
                string queryCabecera = $@"SELECT 
                                        v.id_venta,
                                        c.nombre as Cliente,
                                        v.fecha_venta,
                                        v.tipo_venta,
                                        v.total,
                                        v.estado
                                        FROM Ventas v
                                        INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
                                        WHERE v.id_venta = {idVenta}";

                DataTable dtCabecera = dbHelper.ExecuteQuery(queryCabecera);

                if (dtCabecera.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontró la factura seleccionada.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow factura = dtCabecera.Rows[0];

                // Obtener detalle
                string queryDetalle = $@"SELECT 
                                        p.codigo as 'Código',
                                        p.nombre as 'Producto',
                                        vd.cantidad as 'Cantidad',
                                        vd.precio_unitario as 'Precio Unitario',
                                        vd.subtotal as 'Subtotal'
                                        FROM VentasDetalle vd
                                        INNER JOIN Productos p ON vd.id_producto = p.id_producto
                                        WHERE vd.id_venta = {idVenta}";

                DataTable dtDetalle = dbHelper.ExecuteQuery(queryDetalle);

                // Crear formulario de detalle
                Form detalleForm = new Form();
                detalleForm.Text = $"Detalle de Factura #{idVenta}";
                detalleForm.Size = new Size(700, 500);
                detalleForm.StartPosition = FormStartPosition.CenterParent;
                detalleForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                detalleForm.MaximizeBox = false;

                // Panel para información de la factura
                Panel panelInfo = new Panel();
                panelInfo.Dock = DockStyle.Top;
                panelInfo.Height = 120;
                panelInfo.BorderStyle = BorderStyle.FixedSingle;
                panelInfo.Padding = new Padding(10);

                // Información de la factura
                Label lblInfo = new Label();
                lblInfo.Text = $@"FACTURA #: {factura["id_venta"]}
CLIENTE: {factura["Cliente"]}
FECHA: {Convert.ToDateTime(factura["fecha_venta"]):dd/MM/yyyy}
TIPO: {factura["tipo_venta"]}
ESTADO: {factura["estado"]}
TOTAL: {Convert.ToDecimal(factura["total"]):C2}";
                lblInfo.AutoSize = true;
                lblInfo.Font = new Font("Arial", 10, FontStyle.Bold);
                lblInfo.Location = new Point(10, 10);

                panelInfo.Controls.Add(lblInfo);

                // DataGridView para el detalle
                DataGridView dgvDetalle = new DataGridView();
                dgvDetalle.Dock = DockStyle.Fill;
                dgvDetalle.DataSource = dtDetalle;
                dgvDetalle.ReadOnly = true;
                dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Configurar columnas del detalle
                dgvDetalle.Columns["Código"].Width = 80;
                dgvDetalle.Columns["Producto"].Width = 200;
                dgvDetalle.Columns["Cantidad"].Width = 70;
                dgvDetalle.Columns["Precio Unitario"].Width = 100;
                dgvDetalle.Columns["Subtotal"].Width = 100;

                dgvDetalle.Columns["Precio Unitario"].DefaultCellStyle.Format = "C2";
                dgvDetalle.Columns["Subtotal"].DefaultCellStyle.Format = "C2";

                // Agregar controles al formulario
                detalleForm.Controls.Add(dgvDetalle);
                detalleForm.Controls.Add(panelInfo);

                detalleForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el detalle: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvFacturas.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivo CSV (*.csv)|*.csv";
                saveFileDialog.Title = "Exportar consulta de facturas";
                saveFileDialog.FileName = $"Facturas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveFileDialog.FileName);
                    MessageBox.Show($"Consulta exportada exitosamente a:\n{saveFileDialog.FileName}", "Éxito",
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
                 string separador = ";"; // Usar punto y coma para Excel
        
                 // Escribir encabezados
                 for (int i = 0; i < dgvFacturas.Columns.Count; i++)
                 {
                     sw.Write(dgvFacturas.Columns[i].HeaderText);
                     if (i < dgvFacturas.Columns.Count - 1)
                         sw.Write(separador);
                 }
                 sw.WriteLine();
        
                 // Escribir datos
                 foreach (DataGridViewRow row in dgvFacturas.Rows)
                 {
                     for (int i = 0; i < dgvFacturas.Columns.Count; i++)
                     {
                         if (row.Cells[i].Value != null)
                         {
                             string valor = row.Cells[i].Value.ToString();
        
                             // Escapar comas, comillas, saltos de línea
                             if (valor.Contains(";") || valor.Contains("\"") || valor.Contains("\n"))
                                 valor = "\"" + valor.Replace("\"", "\"\"") + "\"";
        
                             sw.Write(valor);
                         }
                         if (i < dgvFacturas.Columns.Count - 1)
                             sw.Write(separador);
                     }
                     sw.WriteLine();
                 }
             }
         }

        private void dgvFacturas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idVenta = Convert.ToInt32(dgvFacturas.Rows[e.RowIndex].Cells["Número Factura"].Value);
                VerDetalleFactura(idVenta);
            }
        }

        private void txtBusquedaRapida_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBusquedaRapida.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                (dgvFacturas.DataSource as DataTable).DefaultView.RowFilter = "";
            }
            else
            {
                string filterExpression = $@"Cliente LIKE '%{filtro}%' OR 
                                           [Número Factura] LIKE '%{filtro}%' OR 
                                           Estado LIKE '%{filtro}%'";
                (dgvFacturas.DataSource as DataTable).DefaultView.RowFilter = filterExpression;
            }
        }
    }
}
