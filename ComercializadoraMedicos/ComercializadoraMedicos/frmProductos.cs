using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class frmProductos : Form
    {
        private DatabaseHelper dbHelper;
        private int productoIdActual = 0;
        private bool modoEdicion = false;
        private Timer timerBusqueda;

        public frmProductos()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarCategorias();
            CargarProductos();
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void CargarCategorias()
        {
            string query = "SELECT id_categoria, nombre FROM Categorias ORDER BY nombre";
            DataTable dt = dbHelper.ExecuteQuery(query);

            cmbCategoria.DataSource = dt;
            cmbCategoria.DisplayMember = "nombre";
            cmbCategoria.ValueMember = "id_categoria";
        }

        private void CargarProductos()
        {
            string query = @"SELECT p.id_producto, p.codigo, p.nombre, p.descripcion, 
                            c.nombre as categoria, p.tipo, p.precio_compra, p.precio_venta,
                            p.stock_actual, p.stock_minimo, p.es_materia_prima
                            FROM Productos p
                            LEFT JOIN Categorias c ON p.id_categoria = c.id_categoria
                            WHERE p.estado = 1";

            DataTable dt = dbHelper.ExecuteQuery(query);
            dgvProductos.DataSource = dt;

            ConfigurarDataGridView();
            //lblResultados.Text = $"Total de productos: {dt.Rows.Count}";
        }

        private void ConfigurarDataGridView()
        {
            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["id_producto"].HeaderText = "ID";
                dgvProductos.Columns["id_producto"].Width = 50;
                dgvProductos.Columns["codigo"].HeaderText = "Código";
                dgvProductos.Columns["codigo"].Width = 80;
                dgvProductos.Columns["nombre"].HeaderText = "Nombre";
                dgvProductos.Columns["nombre"].Width = 150;
                dgvProductos.Columns["descripcion"].HeaderText = "Descripción";
                dgvProductos.Columns["descripcion"].Width = 200;
                dgvProductos.Columns["categoria"].HeaderText = "Categoría";
                dgvProductos.Columns["categoria"].Width = 120;
                dgvProductos.Columns["tipo"].HeaderText = "Tipo";
                dgvProductos.Columns["tipo"].Width = 100;
                dgvProductos.Columns["precio_compra"].HeaderText = "Precio Compra";
                dgvProductos.Columns["precio_compra"].Width = 90;
                dgvProductos.Columns["precio_venta"].HeaderText = "Precio Venta";
                dgvProductos.Columns["precio_venta"].Width = 90;
                dgvProductos.Columns["stock_actual"].HeaderText = "Stock Actual";
                dgvProductos.Columns["stock_actual"].Width = 80;
                dgvProductos.Columns["stock_minimo"].HeaderText = "Stock Mínimo";
                dgvProductos.Columns["stock_minimo"].Width = 80;
                dgvProductos.Columns["es_materia_prima"].HeaderText = "Es Materia Prima";
                dgvProductos.Columns["es_materia_prima"].Width = 100;

                // Formato de moneda
                dgvProductos.Columns["precio_compra"].DefaultCellStyle.Format = "C2";
                dgvProductos.Columns["precio_venta"].DefaultCellStyle.Format = "C2";

                // Centrar columnas booleanas
                dgvProductos.Columns["es_materia_prima"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                CargarProductos();
                return;
            }

            try
            {
                string query = $@"SELECT p.id_producto, p.codigo, p.nombre, p.descripcion, 
                                c.nombre as categoria, p.tipo, p.precio_compra, p.precio_venta,
                                p.stock_actual, p.stock_minimo, p.es_materia_prima
                                FROM Productos p
                                LEFT JOIN Categorias c ON p.id_categoria = c.id_categoria
                                WHERE p.estado = 1 
                                AND (p.codigo LIKE '%{filtro}%' 
                                     OR p.nombre LIKE '%{filtro}%' 
                                     OR p.descripcion LIKE '%{filtro}%'
                                     OR c.nombre LIKE '%{filtro}%')";

                DataTable dt = dbHelper.ExecuteQuery(query);
                dgvProductos.DataSource = dt;
                //lblResultados.Text = $"Se encontraron {dt.Rows.Count} productos";
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
            CargarProductos();
            txtBuscar.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            productoIdActual = 0;
            LimpiarFormulario();
            HabilitarControles(true);
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un producto para editar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            modoEdicion = true;
            DataGridViewRow row = dgvProductos.SelectedRows[0];
            productoIdActual = Convert.ToInt32(row.Cells["id_producto"].Value);

            CargarDatosDesdeFila(row);
            HabilitarControles(true);
        }

        private void CargarDatosDesdeFila(DataGridViewRow row)
        {
            txtCodigo.Text = row.Cells["codigo"].Value.ToString();
            txtNombre.Text = row.Cells["nombre"].Value.ToString();
            txtDescripcion.Text = row.Cells["descripcion"].Value?.ToString() ?? "";

            // Cargar categoría
            string categoriaNombre = row.Cells["categoria"].Value?.ToString() ?? "";
            foreach (DataRowView item in cmbCategoria.Items)
            {
                if (item["nombre"].ToString() == categoriaNombre)
                {
                    cmbCategoria.SelectedItem = item;
                    break;
                }
            }

            cmbTipo.Text = row.Cells["tipo"].Value.ToString();
            txtPrecioCompra.Text = Convert.ToDecimal(row.Cells["precio_compra"].Value).ToString("F2");
            txtPrecioVenta.Text = Convert.ToDecimal(row.Cells["precio_venta"].Value).ToString("F2");
            txtStockMinimo.Text = row.Cells["stock_minimo"].Value.ToString();
            chkMateriaPrima.Checked = Convert.ToBoolean(row.Cells["es_materia_prima"].Value);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                if (modoEdicion)
                {
                    ActualizarProducto();
                }
                else
                {
                    InsertarProducto();
                }

                CargarProductos();
                LimpiarFormulario();
                HabilitarControles(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertarProducto()
        {
            int idCategoria = cmbCategoria.SelectedValue != null ?
                            Convert.ToInt32(cmbCategoria.SelectedValue) : 0;

            string query = $@"INSERT INTO Productos 
                            (codigo, nombre, descripcion, id_categoria, tipo, 
                             precio_compra, precio_venta, stock_minimo, es_materia_prima) 
                            VALUES 
                            ('{txtCodigo.Text}', '{txtNombre.Text}', '{txtDescripcion.Text}', 
                             {idCategoria}, '{cmbTipo.Text}', {decimal.Parse(txtPrecioCompra.Text)}, 
                             {decimal.Parse(txtPrecioVenta.Text)}, {int.Parse(txtStockMinimo.Text)}, 
                             {(chkMateriaPrima.Checked ? 1 : 0)})";

            int affected = dbHelper.ExecuteNonQuery(query);

            if (affected > 0)
            {
                MessageBox.Show("Producto agregado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarProducto()
        {
            int idCategoria = cmbCategoria.SelectedValue != null ?
                            Convert.ToInt32(cmbCategoria.SelectedValue) : 0;

            string query = $@"UPDATE Productos 
                            SET codigo = '{txtCodigo.Text}',
                                nombre = '{txtNombre.Text}',
                                descripcion = '{txtDescripcion.Text}',
                                id_categoria = {idCategoria},
                                tipo = '{cmbTipo.Text}',
                                precio_compra = {decimal.Parse(txtPrecioCompra.Text)},
                                precio_venta = {decimal.Parse(txtPrecioVenta.Text)},
                                stock_minimo = {int.Parse(txtStockMinimo.Text)},
                                es_materia_prima = {(chkMateriaPrima.Checked ? 1 : 0)}
                            WHERE id_producto = {productoIdActual}";

            int affected = dbHelper.ExecuteNonQuery(query);

            if (affected > 0)
            {
                MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un producto para eliminar.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataGridViewRow row = dgvProductos.SelectedRows[0];
                int idProducto = Convert.ToInt32(row.Cells["id_producto"].Value);

                string query = $"UPDATE Productos SET estado = 0 WHERE id_producto = {idProducto}";
                int affected = dbHelper.ExecuteNonQuery(query);

                if (affected > 0)
                {
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
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
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código del producto es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar una categoría.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategoria.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbTipo.Text))
            {
                MessageBox.Show("Debe seleccionar un tipo de producto.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipo.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) || precioCompra <= 0)
            {
                MessageBox.Show("El precio de compra debe ser un número mayor a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out decimal precioVenta) || precioVenta <= 0)
            {
                MessageBox.Show("El precio de venta debe ser un número mayor a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus();
                return false;
            }

            if (precioVenta <= precioCompra)
            {
                MessageBox.Show("El precio de venta debe ser mayor al precio de compra.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioVenta.Focus();
                return false;
            }

            if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo) || stockMinimo < 0)
            {
                MessageBox.Show("El stock mínimo debe ser un número mayor o igual a cero.", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockMinimo.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            cmbCategoria.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
            txtPrecioCompra.Clear();
            txtPrecioVenta.Clear();
            txtStockMinimo.Clear();
            chkMateriaPrima.Checked = false;
            productoIdActual = 0;
            modoEdicion = false;
        }

        private void HabilitarControles(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
            cmbCategoria.Enabled = habilitar;
            cmbTipo.Enabled = habilitar;
            txtPrecioCompra.Enabled = habilitar;
            txtPrecioVenta.Enabled = habilitar;
            txtStockMinimo.Enabled = habilitar;
            chkMateriaPrima.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;

            btnNuevo.Enabled = !habilitar;
            btnEditar.Enabled = !habilitar;
            btnEliminar.Enabled = !habilitar;
            btnBuscar.Enabled = !habilitar;
            txtBuscar.Enabled = !habilitar;
            btnLimpiar.Enabled = !habilitar;
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0 && !modoEdicion)
            {
                DataGridViewRow row = dgvProductos.SelectedRows[0];
                CargarDatosDesdeFila(row);
            }
        }

        // Evento para calcular automáticamente el precio de venta sugerido
        private void txtPrecioCompra_TextChanged(object sender, EventArgs e)
        {
            if (!modoEdicion && decimal.TryParse(txtPrecioCompra.Text, out decimal precioCompra) && precioCompra > 0)
            {
                // Sugerir un precio de venta con 30% de ganancia
                decimal precioVentaSugerido = precioCompra * 1.3m;
                txtPrecioVenta.Text = precioVentaSugerido.ToString("F2");
            }
        }
    }
}
