using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace ComercializadoraMedicos
{
    public partial class frmRecepcionMercaderia : Form
    {
        private DatabaseHelper dbHelper;
        private DataTable ordenesPendientes;
        private int ordenCompraIdActual = 0;
        private int proveedorIdActual = 0;
        private DataTable detalleRecepcion;

        public frmRecepcionMercaderia()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmRecepcionMercaderia_Load(object sender, EventArgs e)
        {
            CargarBodegas();
            CargarOrdenesPendientes();
            InicializarDetalleRecepcion();
            LimpiarFormulario();
        }

        private void CargarBodegas()
        {
            try
            {
                string query = "SELECT id_bodega, nombre FROM Bodegas WHERE estado = 1 ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbBodega.DataSource = dt;
                cmbBodega.DisplayMember = "nombre";
                cmbBodega.ValueMember = "id_bodega";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar bodegas: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarOrdenesPendientes()
        {
            try
            {
                string query = @"SELECT oc.id_orden_compra, oc.fecha_orden, p.nombre as proveedor,
                                oc.total, oc.fecha_esperada
                                FROM OrdenesCompra oc
                                INNER JOIN Proveedores p ON oc.id_proveedor = p.id_proveedor
                                WHERE oc.estado = 'Aprobada'
                                ORDER BY oc.fecha_orden DESC";

                ordenesPendientes = dbHelper.ExecuteQuery(query);

                cmbOrdenCompra.DataSource = ordenesPendientes;
                cmbOrdenCompra.DisplayMember = "id_orden_compra";
                cmbOrdenCompra.ValueMember = "id_orden_compra";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar órdenes pendientes: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarDetalleRecepcion()
        {
            detalleRecepcion = new DataTable();
            detalleRecepcion.Columns.Add("id_producto", typeof(int));
            detalleRecepcion.Columns.Add("codigo", typeof(string));
            detalleRecepcion.Columns.Add("nombre", typeof(string));
            detalleRecepcion.Columns.Add("cantidad_solicitada", typeof(int));
            detalleRecepcion.Columns.Add("cantidad_recibida", typeof(int));
            detalleRecepcion.Columns.Add("cantidad_aceptada", typeof(int));
            detalleRecepcion.Columns.Add("cantidad_rechazada", typeof(int));
            detalleRecepcion.Columns.Add("motivo_rechazo", typeof(string));

            dgvDetalleRecepcion.DataSource = detalleRecepcion;
            ConfigurarGridDetalle();
        }

        private void ConfigurarGridDetalle()
        {
            if (dgvDetalleRecepcion.Columns.Count > 0)
            {
                dgvDetalleRecepcion.Columns["id_producto"].Visible = false;
                dgvDetalleRecepcion.Columns["codigo"].HeaderText = "Código";
                dgvDetalleRecepcion.Columns["codigo"].Width = 80;
                dgvDetalleRecepcion.Columns["nombre"].HeaderText = "Producto";
                dgvDetalleRecepcion.Columns["nombre"].Width = 200;
                dgvDetalleRecepcion.Columns["cantidad_solicitada"].HeaderText = "Solicitada";
                dgvDetalleRecepcion.Columns["cantidad_solicitada"].Width = 70;
                dgvDetalleRecepcion.Columns["cantidad_recibida"].HeaderText = "Recibida";
                dgvDetalleRecepcion.Columns["cantidad_recibida"].Width = 70;
                dgvDetalleRecepcion.Columns["cantidad_aceptada"].HeaderText = "Aceptada";
                dgvDetalleRecepcion.Columns["cantidad_aceptada"].Width = 70;
                dgvDetalleRecepcion.Columns["cantidad_rechazada"].HeaderText = "Rechazada";
                dgvDetalleRecepcion.Columns["cantidad_rechazada"].Width = 70;
                dgvDetalleRecepcion.Columns["motivo_rechazo"].HeaderText = "Motivo Rechazo";
                dgvDetalleRecepcion.Columns["motivo_rechazo"].Width = 150;

                // Hacer editable solo las columnas necesarias
                foreach (DataGridViewColumn col in dgvDetalleRecepcion.Columns)
                {
                    col.ReadOnly = true;
                }
                dgvDetalleRecepcion.Columns["cantidad_recibida"].ReadOnly = false;
                dgvDetalleRecepcion.Columns["cantidad_aceptada"].ReadOnly = false;
                dgvDetalleRecepcion.Columns["cantidad_rechazada"].ReadOnly = false;
                dgvDetalleRecepcion.Columns["motivo_rechazo"].ReadOnly = false;
            }
        }

        private void cmbOrdenCompra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOrdenCompra.SelectedValue != null && cmbOrdenCompra.SelectedValue is int)
            {
                ordenCompraIdActual = (int)cmbOrdenCompra.SelectedValue;
                CargarDetalleOrdenCompra();
                ActualizarInfoOrden();
            }
        }

        private void ActualizarInfoOrden()
        {
            if (ordenesPendientes != null)
            {
                DataRow[] rows = ordenesPendientes.Select($"id_orden_compra = {ordenCompraIdActual}");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    lblProveedor.Text = $"Proveedor: {row["proveedor"]}";
                    lblFechaOrden.Text = $"Fecha Orden: {Convert.ToDateTime(row["fecha_orden"]):dd/MM/yyyy}";
                    lblFechaEsperada.Text = $"Fecha Esperada: {Convert.ToDateTime(row["fecha_esperada"]):dd/MM/yyyy}";
                    lblTotalOrden.Text = $"Total Orden: {Convert.ToDecimal(row["total"]):C2}";

                    proveedorIdActual = ObtenerIdProveedor(row["proveedor"].ToString());
                }
            }
        }

        private int ObtenerIdProveedor(string nombreProveedor)
        {
            try
            {
                string query = $"SELECT id_proveedor FROM Proveedores WHERE nombre = '{nombreProveedor}'";
                DataTable dt = dbHelper.ExecuteQuery(query);
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["id_proveedor"]) : 0;
            }
            catch
            {
                return 0;
            }
        }

        private void CargarDetalleOrdenCompra()
        {
            try
            {
                detalleRecepcion.Clear();

                string query = $@"SELECT p.id_producto, p.codigo, p.nombre, ocd.cantidad as cantidad_solicitada
                                FROM OrdenesCompraDetalle ocd
                                INNER JOIN Productos p ON ocd.id_producto = p.id_producto
                                WHERE ocd.id_orden_compra = {ordenCompraIdActual}";

                DataTable dtDetalle = dbHelper.ExecuteQuery(query);

                foreach (DataRow row in dtDetalle.Rows)
                {
                    DataRow newRow = detalleRecepcion.NewRow();
                    newRow["id_producto"] = row["id_producto"];
                    newRow["codigo"] = row["codigo"];
                    newRow["nombre"] = row["nombre"];
                    newRow["cantidad_solicitada"] = row["cantidad_solicitada"];
                    newRow["cantidad_recibida"] = row["cantidad_solicitada"]; // Por defecto, lo recibido es lo solicitado
                    newRow["cantidad_aceptada"] = row["cantidad_solicitada"]; // Por defecto, se acepta todo
                    newRow["cantidad_rechazada"] = 0;
                    newRow["motivo_rechazo"] = "";
                    detalleRecepcion.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar detalle de orden: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDetalleRecepcion_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvDetalleRecepcion.Rows[e.RowIndex];
                int cantidadSolicitada = Convert.ToInt32(row.Cells["cantidad_solicitada"].Value);
                int cantidadRecibida = Convert.ToInt32(row.Cells["cantidad_recibida"].Value);
                int cantidadAceptada = Convert.ToInt32(row.Cells["cantidad_aceptada"].Value);

                // Validar que la cantidad aceptada no sea mayor a la recibida
                if (cantidadAceptada > cantidadRecibida)
                {
                    MessageBox.Show("La cantidad aceptada no puede ser mayor a la cantidad recibida.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    row.Cells["cantidad_aceptada"].Value = cantidadRecibida;
                    cantidadAceptada = cantidadRecibida;
                }

                // Calcular automáticamente la cantidad rechazada
                int cantidadRechazada = cantidadRecibida - cantidadAceptada;
                row.Cells["cantidad_rechazada"].Value = cantidadRechazada;

                // Si hay rechazo, solicitar motivo si está vacío
                if (cantidadRechazada > 0 && string.IsNullOrEmpty(row.Cells["motivo_rechazo"].Value?.ToString()))
                {
                    string motivo = Interaction.InputBox(
                        "Ingrese el motivo del rechazo:",
                        "Motivo de Rechazo",
                        "Producto en mal estado");

                    row.Cells["motivo_rechazo"].Value = string.IsNullOrEmpty(motivo) ? "Sin especificar" : motivo;
                }
            }
        }

        private void btnProcesarRecepcion_Click(object sender, EventArgs e)
        {
            if (cmbOrdenCompra.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione una orden de compra.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbBodega.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione una bodega.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (detalleRecepcion.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos para procesar en la recepción.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarRecepcion())
            {
                return;
            }

            // Confirmar la recepción
            var result = MessageBox.Show("¿Está seguro de procesar la recepción de mercadería?", "Confirmar Recepción",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarRecepcion();
            }
        }

        private bool ValidarRecepcion()
        {
            foreach (DataRow row in detalleRecepcion.Rows)
            {
                int cantidadRecibida = Convert.ToInt32(row["cantidad_recibida"]);
                int cantidadAceptada = Convert.ToInt32(row["cantidad_aceptada"]);

                if (cantidadRecibida < 0)
                {
                    MessageBox.Show("La cantidad recibida no puede ser negativa.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (cantidadAceptada < 0)
                {
                    MessageBox.Show("La cantidad aceptada no puede ser negativa.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (cantidadAceptada > cantidadRecibida)
                {
                    MessageBox.Show("La cantidad aceptada no puede ser mayor que la cantidad recibida.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void ProcesarRecepcion()
        {
            try
            {
                // Preparar los detalles en formato JSON para el stored procedure
                string detallesJSON = GenerarJSONDetalles();

                // Llamar al procedimiento almacenado
                string query = $@"EXEC spTransaccionRecepcionCompleta 
                                {ordenCompraIdActual}, 
                                {cmbBodega.SelectedValue}, 
                                '{txtObservaciones.Text}',
                                '{detallesJSON}'";

                DataTable result = dbHelper.ExecuteQuery(query);

                if (result.Rows.Count > 0)
                {
                    string resultado = result.Rows[0]["resultado"].ToString();
                    if (resultado == "Éxito")
                    {
                        int idRecepcion = Convert.ToInt32(result.Rows[0]["id_recepcion"]);

                        MessageBox.Show($"✅ Recepción procesada exitosamente!\n\n" +
                                      $"Número de recepción: {idRecepcion}",
                                      "Recepción Exitosa",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarFormulario();
                        CargarOrdenesPendientes(); // Recargar órdenes pendientes
                    }
                    else
                    {
                        string mensaje = result.Rows[0]["mensaje"].ToString();
                        MessageBox.Show($"❌ Error al procesar la recepción: {mensaje}", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar la recepción: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarJSONDetalles()
        {
            // Crear JSON manualmente para los detalles
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            json.Append("[");

            for (int i = 0; i < detalleRecepcion.Rows.Count; i++)
            {
                DataRow row = detalleRecepcion.Rows[i];
                json.Append("{");
                json.Append($"\"id_producto\": {row["id_producto"]},");
                json.Append($"\"cantidad_solicitada\": {row["cantidad_solicitada"]},");
                json.Append($"\"cantidad_recibida\": {row["cantidad_recibida"]}");
                json.Append("}");

                if (i < detalleRecepcion.Rows.Count - 1)
                    json.Append(",");
            }

            json.Append("]");
            return json.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de cancelar la recepción? Se perderán todos los datos ingresados.",
                                       "Cancelar Recepción",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LimpiarFormulario()
        {
            if (cmbOrdenCompra.Items.Count > 0)
                cmbOrdenCompra.SelectedIndex = 0;
            if (cmbBodega.Items.Count > 0)
                cmbBodega.SelectedIndex = 0;
            txtObservaciones.Clear();
            detalleRecepcion.Clear();

            lblProveedor.Text = "Proveedor:";
            lblFechaOrden.Text = "Fecha Orden:";
            lblFechaEsperada.Text = "Fecha Esperada:";
            lblTotalOrden.Text = "Total Orden:";
        }

        private void btnAceptarTodo_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in detalleRecepcion.Rows)
            {
                int cantidadRecibida = Convert.ToInt32(row["cantidad_recibida"]);
                row["cantidad_aceptada"] = cantidadRecibida;
                row["cantidad_rechazada"] = 0;
                row["motivo_rechazo"] = "";
            }
            dgvDetalleRecepcion.Refresh();
        }

        private void btnRechazarTodo_Click(object sender, EventArgs e)
        {
            string motivo = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingrese el motivo del rechazo general:",
                "Motivo de Rechazo General",
                "Productos no cumplen con especificaciones",
                -1, -1);

            if (string.IsNullOrEmpty(motivo))
            {
                motivo = "Rechazo general";
            }

            foreach (DataRow row in detalleRecepcion.Rows)
            {
                int cantidadRecibida = Convert.ToInt32(row["cantidad_recibida"]);
                row["cantidad_aceptada"] = 0;
                row["cantidad_rechazada"] = cantidadRecibida;
                row["motivo_rechazo"] = motivo;
            }
            dgvDetalleRecepcion.Refresh();
        }
    }
}