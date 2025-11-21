using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComercializadoraMedicos
{
    public partial class MainForm : Form
    {
        private DatabaseHelper dbHelper;
        private Button currentActiveMenuButton = null;

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
                    // Silently connect - no message box
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error de conexión: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Menu Principal - Top Navigation

        private void btnMenuInventario_Click(object sender, EventArgs e)
        {
            SetActiveMenuButton(btnMenuInventario);
            MostrarMenuInventario();
        }

        private void btnMenuVentas_Click(object sender, EventArgs e)
        {
            SetActiveMenuButton(btnMenuVentas);
            MostrarMenuVentas();
        }

        private void btnMenuBancos_Click(object sender, EventArgs e)
        {
            SetActiveMenuButton(btnMenuBancos);
            MostrarMenuBancos();
        }

        private void btnMenuConsultas_Click(object sender, EventArgs e)
        {
            SetActiveMenuButton(btnMenuConsultas);
            MostrarMenuConsultas();
        }

        private void btnMenuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Gestión de Menús Laterales

        private void SetActiveMenuButton(Button menuButton)
        {
            // Reset previous active button
            if (currentActiveMenuButton != null && currentActiveMenuButton != btnMenuSalir)
            {
                currentActiveMenuButton.BackColor = Color.FromArgb(52, 152, 219);
            }

            // Set new active button
            currentActiveMenuButton = menuButton;
            if (menuButton != btnMenuSalir)
            {
                menuButton.BackColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void LimpiarSidebar()
        {
            panelSidebar.Controls.Clear();
        }

        private void MostrarMenuInventario()
        {
            LimpiarSidebar();
            panelSidebar.Visible = true;
            lblBienvenida.Visible = false;

            int yPos = 20;
            int spacing = 50;

            // Gestionar Proveedores
            AgregarBotonSidebar("Gestionar Proveedores", yPos, (s, e) => {
                frmProveedores form = new frmProveedores();
                form.ShowDialog();
            });
            yPos += spacing;

            // Órdenes de Compra
            AgregarBotonSidebar("Órdenes de Compra", yPos, (s, e) => {
                frmOrdenesCompra form = new frmOrdenesCompra();
                form.ShowDialog();
            });
            yPos += spacing;

            // Recepción de Mercadería
            AgregarBotonSidebar("Recepción de Mercadería", yPos, (s, e) => {
                frmRecepcionMercaderia form = new frmRecepcionMercaderia();
                form.ShowDialog();
            });
            yPos += spacing;

            // Devoluciones a Proveedores
            AgregarBotonSidebar("Devoluciones a Proveedores", yPos, (s, e) => {
                frmDevolucionProveedores form = new frmDevolucionProveedores();
                form.ShowDialog();
            });
            yPos += spacing;

            // Elaboración de Productos
            AgregarBotonSidebar("Elaboración de Productos", yPos, (s, e) => {
                frmElaboracionProductos form = new frmElaboracionProductos();
                form.ShowDialog();
            });
            yPos += spacing;

            // Productos (CRUD)
            AgregarBotonSidebar("Productos", yPos, (s, e) => {
                frmProductos form = new frmProductos();
                form.ShowDialog();
            });
        }

        private void MostrarMenuVentas()
        {
            LimpiarSidebar();
            panelSidebar.Visible = true;
            lblBienvenida.Visible = false;

            int yPos = 20;
            int spacing = 50;

            // Gestionar Clientes
            AgregarBotonSidebar("Gestionar Clientes", yPos, (s, e) => {
                frmClientes form = new frmClientes();
                form.ShowDialog();
            });
            yPos += spacing;

            // Nueva Venta
            AgregarBotonSidebar("Nueva Venta", yPos, (s, e) => {
                frmVentas form = new frmVentas();
                form.ShowDialog();
            });
            yPos += spacing;

            // Registrar Pago Cliente
            AgregarBotonSidebar("Registrar Pago Cliente", yPos, (s, e) => {
                frmPagosClientes form = new frmPagosClientes();
                form.ShowDialog();
            });
            yPos += spacing;

            // Arqueo Diario
            AgregarBotonSidebar("Arqueo Diario", yPos, (s, e) => {
                frmArqueoCaja form = new frmArqueoCaja();
                form.ShowDialog();
            });
        }

        private void MostrarMenuBancos()
        {
            LimpiarSidebar();
            panelSidebar.Visible = true;
            lblBienvenida.Visible = false;

            int yPos = 20;
            int spacing = 50;

            // Cuentas Bancarias
            AgregarBotonSidebar("Cuentas Bancarias", yPos, (s, e) => {
                frmBancos form = new frmBancos();
                form.ShowDialog();
            });
            yPos += spacing;

            // Registrar Depósito
            AgregarBotonSidebar("Registrar Depósito", yPos, (s, e) => {
                frmDepositos form = new frmDepositos();
                form.ShowDialog();
            });
            yPos += spacing;

            // Pago a Proveedor
            AgregarBotonSidebar("Pago a Proveedor", yPos, (s, e) => {
                frmPagosProveedores form = new frmPagosProveedores();
                form.ShowDialog();
            });
        }

        private void MostrarMenuConsultas()
        {
            LimpiarSidebar();
            panelSidebar.Visible = true;
            lblBienvenida.Visible = false;

            int yPos = 20;
            int spacing = 50;

            // Consulta de Facturas
            AgregarBotonSidebar("Consulta de Facturas", yPos, (s, e) => {
                frmConsultaFacturas form = new frmConsultaFacturas();
                form.ShowDialog();
            });
            yPos += spacing;

            // Reportes
            AgregarBotonSidebar("Reportes", yPos, (s, e) => {
                frmReportes form = new frmReportes();
                form.ShowDialog();
            });
        }

        private void AgregarBotonSidebar(string texto, int yPos, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(10, yPos),
                Size = new Size(230, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += clickHandler;
            btn.MouseEnter += SidebarButton_MouseEnter;
            btn.MouseLeave += SidebarButton_MouseLeave;

            panelSidebar.Controls.Add(btn);
        }

        #endregion

        #region Efectos Hover

        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn != currentActiveMenuButton)
            {
                if (btn == btnMenuSalir)
                {
                    btn.BackColor = Color.FromArgb(192, 57, 43);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(41, 128, 185);
                }
            }
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn != currentActiveMenuButton)
            {
                if (btn == btnMenuSalir)
                {
                    btn.BackColor = Color.FromArgb(231, 76, 60);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(52, 152, 219);
                }
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(52, 152, 219);
            }
        }

        #endregion
    }
}