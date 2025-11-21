namespace ComercializadoraMedicos
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnMenuSalir = new System.Windows.Forms.Button();
            this.btnMenuConsultas = new System.Windows.Forms.Button();
            this.btnMenuBancos = new System.Windows.Forms.Button();
            this.btnMenuVentas = new System.Windows.Forms.Button();
            this.btnMenuInventario = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelTop.Controls.Add(this.btnMenuSalir);
            this.panelTop.Controls.Add(this.btnMenuConsultas);
            this.panelTop.Controls.Add(this.btnMenuBancos);
            this.panelTop.Controls.Add(this.btnMenuVentas);
            this.panelTop.Controls.Add(this.btnMenuInventario);
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 80);
            this.panelTop.TabIndex = 0;
            // 
            // btnMenuSalir
            // 
            this.btnMenuSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnMenuSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuSalir.FlatAppearance.BorderSize = 0;
            this.btnMenuSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuSalir.ForeColor = System.Drawing.Color.White;
            this.btnMenuSalir.Location = new System.Drawing.Point(1050, 25);
            this.btnMenuSalir.Name = "btnMenuSalir";
            this.btnMenuSalir.Size = new System.Drawing.Size(120, 40);
            this.btnMenuSalir.TabIndex = 5;
            this.btnMenuSalir.Text = "Salir";
            this.btnMenuSalir.UseVisualStyleBackColor = false;
            this.btnMenuSalir.Click += new System.EventHandler(this.btnMenuSalir_Click);
            this.btnMenuSalir.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnMenuSalir.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnMenuConsultas
            // 
            this.btnMenuConsultas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMenuConsultas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuConsultas.FlatAppearance.BorderSize = 0;
            this.btnMenuConsultas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuConsultas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuConsultas.ForeColor = System.Drawing.Color.White;
            this.btnMenuConsultas.Location = new System.Drawing.Point(900, 25);
            this.btnMenuConsultas.Name = "btnMenuConsultas";
            this.btnMenuConsultas.Size = new System.Drawing.Size(120, 40);
            this.btnMenuConsultas.TabIndex = 4;
            this.btnMenuConsultas.Text = "Consultas";
            this.btnMenuConsultas.UseVisualStyleBackColor = false;
            this.btnMenuConsultas.Click += new System.EventHandler(this.btnMenuConsultas_Click);
            this.btnMenuConsultas.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnMenuConsultas.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnMenuBancos
            // 
            this.btnMenuBancos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMenuBancos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuBancos.FlatAppearance.BorderSize = 0;
            this.btnMenuBancos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuBancos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuBancos.ForeColor = System.Drawing.Color.White;
            this.btnMenuBancos.Location = new System.Drawing.Point(750, 25);
            this.btnMenuBancos.Name = "btnMenuBancos";
            this.btnMenuBancos.Size = new System.Drawing.Size(120, 40);
            this.btnMenuBancos.TabIndex = 3;
            this.btnMenuBancos.Text = "Bancos";
            this.btnMenuBancos.UseVisualStyleBackColor = false;
            this.btnMenuBancos.Click += new System.EventHandler(this.btnMenuBancos_Click);
            this.btnMenuBancos.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnMenuBancos.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnMenuVentas
            // 
            this.btnMenuVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMenuVentas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuVentas.FlatAppearance.BorderSize = 0;
            this.btnMenuVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuVentas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuVentas.ForeColor = System.Drawing.Color.White;
            this.btnMenuVentas.Location = new System.Drawing.Point(600, 25);
            this.btnMenuVentas.Name = "btnMenuVentas";
            this.btnMenuVentas.Size = new System.Drawing.Size(120, 40);
            this.btnMenuVentas.TabIndex = 2;
            this.btnMenuVentas.Text = "Ventas";
            this.btnMenuVentas.UseVisualStyleBackColor = false;
            this.btnMenuVentas.Click += new System.EventHandler(this.btnMenuVentas_Click);
            this.btnMenuVentas.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnMenuVentas.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // btnMenuInventario
            // 
            this.btnMenuInventario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMenuInventario.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenuInventario.FlatAppearance.BorderSize = 0;
            this.btnMenuInventario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuInventario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenuInventario.ForeColor = System.Drawing.Color.White;
            this.btnMenuInventario.Location = new System.Drawing.Point(450, 25);
            this.btnMenuInventario.Name = "btnMenuInventario";
            this.btnMenuInventario.Size = new System.Drawing.Size(120, 40);
            this.btnMenuInventario.TabIndex = 1;
            this.btnMenuInventario.Text = "Inventario";
            this.btnMenuInventario.UseVisualStyleBackColor = false;
            this.btnMenuInventario.Click += new System.EventHandler(this.btnMenuInventario_Click);
            this.btnMenuInventario.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.btnMenuInventario.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 25);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(387, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Comercializadora de Productos Médicos";
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 80);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(250, 620);
            this.panelSidebar.TabIndex = 1;
            this.panelSidebar.Visible = false;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.lblBienvenida);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(250, 80);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(950, 620);
            this.panelContent.TabIndex = 2;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblBienvenida.Location = new System.Drawing.Point(0, 250);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(950, 120);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido al Sistema\r\nSeleccione una opción del menú superior";
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelTop);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Comercializadora de Productos Médicos";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMenuInventario;
        private System.Windows.Forms.Button btnMenuVentas;
        private System.Windows.Forms.Button btnMenuBancos;
        private System.Windows.Forms.Button btnMenuConsultas;
        private System.Windows.Forms.Button btnMenuSalir;
        private System.Windows.Forms.Label lblBienvenida;
    }
}

