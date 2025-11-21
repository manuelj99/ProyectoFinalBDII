using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ComercializadoraMedicos
{
    public partial class frmElaboracionProductos : Form
    {
        private DatabaseHelper dbHelper;
        private DataTable detalleElaboracion;
        private int productoElaboradoId = 0;
        private decimal costoTotalElaboracion = 0;

        public frmElaboracionProductos()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmElaboracionProductos_Load(object sender, EventArgs e)
        {
            CargarProductosElaborados();
            CargarMateriasPrimas();
            InicializarDetalleElaboracion();
            LimpiarFormulario();
        }

        private void CargarProductosElaborados()
        {
            try
            {
                string query = @"SELECT id_producto, codigo, nombre, precio_compra 
                               FROM Productos 
                               WHERE estado = 1 AND es_materia_prima = 0 
                               ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbProductoElaborado.DataSource = dt;
                cmbProductoElaborado.DisplayMember = "nombre";
                cmbProductoElaborado.ValueMember = "id_producto";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos elaborados: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMateriasPrimas()
        {
            try
            {
                string query = @"SELECT id_producto, codigo, nombre, precio_compra, stock_actual 
                               FROM Productos 
                               WHERE estado = 1 AND es_materia_prima = 1 
                               ORDER BY nombre";
                DataTable dt = dbHelper.ExecuteQuery(query);

                cmbMateriaPrima.DataSource = dt;
                cmbMateriaPrima.DisplayMember = "nombre";
                cmbMateriaPrima.ValueMember = "id_producto";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materias primas: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicializarDetalleElaboracion()
        {
            detalleElaboracion = new DataTable();
            detalleElaboracion.Columns.Add("id_materia_prima", typeof(int));
            detalleElaboracion.Columns.Add("codigo", typeof(string));
            detalleElaboracion.Columns.Add("nombre", typeof(string));
            detalleElaboracion.Columns.Add("precio_compra", typeof(decimal));
            detalleElaboracion.Columns.Add("stock_actual", typeof(int));
            detalleElaboracion.Columns.Add("cantidad_utilizada", typeof(int));
            detalleElaboracion.Columns.Add("costo_parcial", typeof(decimal));

            dgvDetalleElaboracion.DataSource = detalleElaboracion;
            ConfigurarGridDetalle();
        }

        private void ConfigurarGridDetalle()
        {
            if (dgvDetalleElaboracion.Columns.Count > 0)
            {
                dgvDetalleElaboracion.Columns["id_materia_prima"].Visible = false;
                dgvDetalleElaboracion.Columns["codigo"].HeaderText = "Código";
                dgvDetalleElaboracion.Columns["codigo"].Width = 80;
                dgvDetalleElaboracion.Columns["nombre"].HeaderText = "Materia Prima";
                dgvDetalleElaboracion.Columns["nombre"].Width = 200;
                dgvDetalleElaboracion.Columns["precio_compra"].HeaderText = "Precio Compra";
                dgvDetalleElaboracion.Columns["precio_compra"].Width = 100;
                dgvDetalleElaboracion.Columns["stock_actual"].HeaderText = "Stock Actual";
                dgvDetalleElaboracion.Columns["stock_actual"].Width = 80;
                dgvDetalleElaboracion.Columns["cantidad_utilizada"].HeaderText = "Cantidad Usada";
                dgvDetalleElaboracion.Columns["cantidad_utilizada"].Width = 100;
                dgvDetalleElaboracion.Columns["costo_parcial"].HeaderText = "Costo Parcial";
                dgvDetalleElaboracion.Columns["costo_parcial"].Width = 100;

                dgvDetalleElaboracion.Columns["precio_compra"].DefaultCellStyle.Format = "C2";
                dgvDetalleElaboracion.Columns["costo_parcial"].DefaultCellStyle.Format = "C2";
            }
        }

        private void cmbProductoElaborado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductoElaborado.SelectedValue != null)
            {
                 DataRowView producto = (DataRowView)cmbProductoElaborado.SelectedItem;
                decimal precioCompra = Convert.ToDecimal(producto["precio_compra"]);
                lblPrecioActual.Text = $"Precio Actual: {precioCompra:C2}";
            }
        }

        private void cmbMateriaPrima_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMateriaPrima.SelectedValue != null)
            {
                DataRowView materiaPrima = (DataRowView)cmbMateriaPrima.SelectedItem;
                int stock = Convert.ToInt32(materiaPrima["stock_actual"]);
                decimal precio = Convert.ToDecimal(materiaPrima["precio_compra"]);
                lblStockMateriaPrima.Text = $"Stock: {stock}";
                lblPrecioMateriaPrima.Text = $"Precio: {precio:C2}";
            }
        }

        private void btnAgregarMateriaPrima_Click(object sender, EventArgs e)
        {
            if (cmbMateriaPrima.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione una materia prima.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidadUtilizada.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("La cantidad utilizada debe ser un número mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidadUtilizada.Focus();
                return;
            }

            try
            {
                // Obtener datos de la materia prima seleccionada
                DataRowView materiaPrima = (DataRowView)cmbMateriaPrima.SelectedItem;
                int idMateriaPrima = (int)materiaPrima["id_producto"];
                string codigo = materiaPrima["codigo"].ToString();
                string nombre = materiaPrima["nombre"].ToString();
                decimal precio = Convert.ToDecimal(materiaPrima["precio_compra"]);
                int stockActual = Convert.ToInt32(materiaPrima["stock_actual"]);

                // Verificar stock disponible
                if (cantidad > stockActual)
                {
                    MessageBox.Show($"Stock insuficiente. Solo hay {stockActual} unidades disponibles.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verificar si la materia prima ya está en el detalle
                DataRow[] existingRows = detalleElaboracion.Select($"id_materia_prima = {idMateriaPrima}");
                if (existingRows.Length > 0)
                {
                    // Actualizar cantidad si ya existe
                    int nuevaCantidad = (int)existingRows[0]["cantidad_utilizada"] + cantidad;
                    if (nuevaCantidad > stockActual)
                    {
                        MessageBox.Show($"La cantidad total ({nuevaCantidad}) excede el stock disponible ({stockActual}).", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    existingRows[0]["cantidad_utilizada"] = nuevaCantidad;
                    existingRows[0]["costo_parcial"] = nuevaCantidad * precio;
                }
                else
                {
                    // Agregar nueva materia prima al detalle
                    DataRow newRow = detalleElaboracion.NewRow();
                    newRow["id_materia_prima"] = idMateriaPrima;
                    newRow["codigo"] = codigo;
                    newRow["nombre"] = nombre;
                    newRow["precio_compra"] = precio;
                    newRow["stock_actual"] = stockActual;
                    newRow["cantidad_utilizada"] = cantidad;
                    newRow["costo_parcial"] = cantidad * precio;
                    detalleElaboracion.Rows.Add(newRow);
                }

                CalcularCostoTotal();
                LimpiarControlesMateriaPrima();
                ActualizarStockDisponible();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar materia prima: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitarMateriaPrima_Click(object sender, EventArgs e)
        {
            if (dgvDetalleElaboracion.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDetalleElaboracion.SelectedRows)
                {
                    dgvDetalleElaboracion.Rows.Remove(row);
                }
                CalcularCostoTotal();
                ActualizarStockDisponible();
            }
            else
            {
                MessageBox.Show("Por favor seleccione una materia prima para quitar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CalcularCostoTotal()
        {
            costoTotalElaboracion = 0;
            foreach (DataRow row in detalleElaboracion.Rows)
            {
                costoTotalElaboracion += Convert.ToDecimal(row["costo_parcial"]);
            }
            lblCostoTotal.Text = $"Costo Total: {costoTotalElaboracion:C2}";

            // Calcular y sugerir precio de venta
            if (costoTotalElaboracion > 0)
            {
                decimal precioVentaSugerido = costoTotalElaboracion * 1.3m; // 30% de ganancia
                lblPrecioSugerido.Text = $"Precio Venta Sugerido: {precioVentaSugerido:C2}";
            }
        }

        private void ActualizarStockDisponible()
        {
            if (cmbMateriaPrima.SelectedValue != null)
            {
                DataRowView materiaPrima = (DataRowView)cmbMateriaPrima.SelectedItem;
                int stockOriginal = Convert.ToInt32(materiaPrima["stock_actual"]);

                // Calcular stock reservado en el detalle actual
                int stockReservado = 0;
                DataRow[] rowsInDetail = detalleElaboracion.Select($"id_materia_prima = {materiaPrima["id_producto"]}");
                if (rowsInDetail.Length > 0)
                {
                    stockReservado = Convert.ToInt32(rowsInDetail[0]["cantidad_utilizada"]);
                }

                lblStockMateriaPrima.Text = $"Stock: {stockOriginal - stockReservado}";
            }
        }

        private void LimpiarControlesMateriaPrima()
        {
            txtCantidadUtilizada.Text = "1";
            cmbMateriaPrima.Focus();
        }

        private void LimpiarFormulario()
        {
            cmbProductoElaborado.SelectedIndex = 0;
            txtCantidadElaborada.Text = "1";
            detalleElaboracion.Clear();
            costoTotalElaboracion = 0;
            lblCostoTotal.Text = "Costo Total: L0.00";
            lblPrecioSugerido.Text = "Precio Venta Sugerido: L0.00";
            txtCantidadUtilizada.Text = "1";
            ActualizarStockDisponible();
        }

        private void btnProcesarElaboracion_Click(object sender, EventArgs e)
        {
            if (cmbProductoElaborado.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione el producto a elaborar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidadElaborada.Text, out int cantidadElaborada) || cantidadElaborada <= 0)
            {
                MessageBox.Show("La cantidad a elaborar debe ser un número mayor a cero.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidadElaborada.Focus();
                return;
            }

            if (detalleElaboracion.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos una materia prima a la elaboración.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar la elaboración
            var result = MessageBox.Show($"¿Está seguro de procesar la elaboración de {cantidadElaborada} unidades?\n\n" +
                                       $"Costo total: {costoTotalElaboracion:C2}",
                                       "Confirmar Elaboración",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ProcesarElaboracion(cantidadElaborada);
            }
        }

        private void ProcesarElaboracion(int cantidadElaborada)
        {
            try
            {
                // Preparar los detalles en formato JSON para el stored procedure
                string detallesJSON = GenerarJSONDetalles();

                // Llamar al procedimiento almacenado
                string query = $@"EXEC sp_ElaboracionProductos_Insertar 
                                {productoElaboradoId}, 
                                {cantidadElaborada}, 
                                '{detallesJSON}'";

                DataTable result = dbHelper.ExecuteQuery(query);

                if (result.Rows.Count > 0)
                {
                    int idElaboracion = Convert.ToInt32(result.Rows[0]["id_elaboracion"]);

                    MessageBox.Show($"✅ Elaboración procesada exitosamente!\n\n" +
                                  $"Número de elaboración: {idElaboracion}\n" +
                                  $"Producto elaborado: {cmbProductoElaborado.Text}\n" +
                                  $"Cantidad: {cantidadElaborada} unidades\n" +
                                  $"Costo total: {costoTotalElaboracion:C2}",
                                  "Elaboración Exitosa",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarFormulario();
                    CargarMateriasPrimas(); // Recargar materias primas para actualizar stocks
                    CargarProductosElaborados(); // Recargar productos elaborados
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al procesar la elaboración: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarJSONDetalles()
        {
            // Crear JSON manualmente para los detalles
            System.Text.StringBuilder json = new System.Text.StringBuilder();
            json.Append("[");

            for (int i = 0; i < detalleElaboracion.Rows.Count; i++)
            {
                DataRow row = detalleElaboracion.Rows[i];
                json.Append("{");
                json.Append($"\"id_materia_prima\": {row["id_materia_prima"]},");
                json.Append($"\"cantidad_utilizada\": {row["cantidad_utilizada"]}");
                json.Append("}");

                if (i < detalleElaboracion.Rows.Count - 1)
                    json.Append(",");
            }

            json.Append("]");
            return json.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de cancelar la elaboración? Se perderán todos los datos ingresados.",
                                       "Cancelar Elaboración",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtCantidadUtilizada_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCantidadElaborada_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvDetalleElaboracion_SelectionChanged(object sender, EventArgs e)
        {
            // Actualizar stock disponible cuando se selecciona una materia prima en el grid
            if (dgvDetalleElaboracion.SelectedRows.Count > 0 && cmbMateriaPrima.SelectedValue != null)
            {
                ActualizarStockDisponible();
            }
        }

        private void btnActualizarPrecio_Click(object sender, EventArgs e)
        {
            if (cmbProductoElaborado.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un producto elaborado.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (costoTotalElaboracion <= 0)
            {
                MessageBox.Show("Primero debe calcular el costo total de la elaboración.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal nuevoPrecio = costoTotalElaboracion * 1.3m; // 30% de ganancia

                var result = MessageBox.Show($"¿Desea actualizar el precio de compra del producto a {nuevoPrecio:C2}?\n\n" +
                                           $"Este será el nuevo precio base del producto.",
                                           "Actualizar Precio",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string query = $"UPDATE Productos SET precio_compra = {nuevoPrecio} WHERE id_producto = {productoElaboradoId}";
                    int affected = dbHelper.ExecuteNonQuery(query);

                    if (affected > 0)
                    {
                        MessageBox.Show("Precio de compra actualizado correctamente.", "Éxito",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProductosElaborados(); // Recargar para mostrar el nuevo precio
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el precio: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}