using System;
using System.Data;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class MainForm : Form
    {
        private DatabaseHelper dbHelper;

        public MainForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            ProbarConexion();
        }

        private void ProbarConexion()
        {
            try
            {
                string query = "SELECT COUNT(*) as Total FROM Proveedores";
                DataTable dt = dbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    int total = Convert.ToInt32(dt.Rows[0]["Total"]);
                    MessageBox.Show($"✅ Conexión exitosa!", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error de conexión: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón Proveedores
        private void btnProveedores_Click(object sender, EventArgs e)
        {
            frmProveedores formProveedores = new frmProveedores();
            formProveedores.ShowDialog();
        }

        // Botón Productos
        private void btnProductos_Click(object sender, EventArgs e)
        {
            frmProductos formProductos = new frmProductos();
            formProductos.ShowDialog();
        }

        // Botón Clientes
        private void btnClientes_Click(object sender, EventArgs e)
        {
            frmClientes formClientes = new frmClientes();
            formClientes.ShowDialog();
        }

        // Botón Ventas
        private void btnVentas_Click(object sender, EventArgs e)
        {
            frmVentas formVentas = new frmVentas();
            formVentas.ShowDialog();
        }

        // Botón Salir
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            frmReportes formReportes = new frmReportes();
            formReportes.ShowDialog();
        }

        private void btnOrdenesCompra_Click(object sender, EventArgs e)
        {
            frmOrdenesCompra formOrdenesCompra = new frmOrdenesCompra();
            formOrdenesCompra.ShowDialog();
        }

        private void btnRecepcionMercaderia_Click(object sender, EventArgs e)
        {
            frmRecepcionMercaderia formRecepcion = new frmRecepcionMercaderia();
            formRecepcion.ShowDialog();
        }

        private void btnPagosProveedores_Click(object sender, EventArgs e)
        {
            frmPagosProveedores formPagosProveedores = new frmPagosProveedores();
            formPagosProveedores.ShowDialog();
        }

        private void btnPagosClientes_Click(object sender, EventArgs e)
        {
            frmPagosClientes formPagosClientes = new frmPagosClientes();
            formPagosClientes.ShowDialog();
        }

        private void btnElaboracion_Click(object sender, EventArgs e)
        {
            frmElaboracionProductos formElaboracion = new frmElaboracionProductos();
            formElaboracion.ShowDialog();
        }

        private void btnArqueos_Click(object sender, EventArgs e)
        {
            frmArqueoCaja formArqueo = new frmArqueoCaja();
            formArqueo.ShowDialog();
        }

        private void btnBancos_Click(object sender, EventArgs e)
        {
            frmBancos formBancos = new frmBancos();
            formBancos.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDevolucionProveedores formDevolucion = new frmDevolucionProveedores();
            formDevolucion.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmConsultaFacturas formConsultaFacturas = new frmConsultaFacturas();
            formConsultaFacturas.ShowDialog();
        }
    }
}