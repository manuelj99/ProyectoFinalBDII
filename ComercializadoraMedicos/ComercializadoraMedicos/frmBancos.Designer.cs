namespace ComercializadoraMedicos
{
    partial class frmBancos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBuscarBancos = new System.Windows.Forms.TextBox();
            this.btnBuscarBancos = new System.Windows.Forms.Button();
            this.btnLimpiarBusqueda = new System.Windows.Forms.Button();
            this.dgvBancos = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNombreBanco = new System.Windows.Forms.TextBox();
            this.txtNumeroCuenta = new System.Windows.Forms.TextBox();
            this.txtSaldoInicial = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnNuevoDeposito = new System.Windows.Forms.Button();
            this.btnRefrescarMovimientos = new System.Windows.Forms.Button();
            this.dgvMovimientos = new System.Windows.Forms.DataGridView();
            this.btnNuevoBanco = new System.Windows.Forms.Button();
            this.btnEditarBanco = new System.Windows.Forms.Button();
            this.btnGuardarBanco = new System.Windows.Forms.Button();
            this.btnEliminarBanco = new System.Windows.Forms.Button();
            this.btnCancelarBanco = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBancos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "GESTION DE BANCOS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "CUENTAS BANCARIAS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Buscar:";
            // 
            // txtBuscarBancos
            // 
            this.txtBuscarBancos.Location = new System.Drawing.Point(82, 74);
            this.txtBuscarBancos.Name = "txtBuscarBancos";
            this.txtBuscarBancos.Size = new System.Drawing.Size(100, 20);
            this.txtBuscarBancos.TabIndex = 3;
            this.txtBuscarBancos.TextChanged += new System.EventHandler(this.txtBuscarBancos_TextChanged);
            // 
            // btnBuscarBancos
            // 
            this.btnBuscarBancos.Location = new System.Drawing.Point(225, 70);
            this.btnBuscarBancos.Name = "btnBuscarBancos";
            this.btnBuscarBancos.Size = new System.Drawing.Size(75, 23);
            this.btnBuscarBancos.TabIndex = 4;
            this.btnBuscarBancos.Text = "Buscar";
            this.btnBuscarBancos.UseVisualStyleBackColor = true;
            this.btnBuscarBancos.Click += new System.EventHandler(this.btnBuscarBancos_Click);
            // 
            // btnLimpiarBusqueda
            // 
            this.btnLimpiarBusqueda.Location = new System.Drawing.Point(319, 69);
            this.btnLimpiarBusqueda.Name = "btnLimpiarBusqueda";
            this.btnLimpiarBusqueda.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiarBusqueda.TabIndex = 5;
            this.btnLimpiarBusqueda.Text = "Limpiar";
            this.btnLimpiarBusqueda.UseVisualStyleBackColor = true;
            this.btnLimpiarBusqueda.Click += new System.EventHandler(this.btnLimpiarBusqueda_Click);
            // 
            // dgvBancos
            // 
            this.dgvBancos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBancos.Location = new System.Drawing.Point(9, 99);
            this.dgvBancos.Name = "dgvBancos";
            this.dgvBancos.Size = new System.Drawing.Size(400, 113);
            this.dgvBancos.TabIndex = 6;
            this.dgvBancos.SelectionChanged += new System.EventHandler(this.dgvBancos_SelectionChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "DETALLES DE CUENTA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Nombre Banco:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 278);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Numero Cuenta:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 307);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Saldo:";
            // 
            // txtNombreBanco
            // 
            this.txtNombreBanco.Location = new System.Drawing.Point(100, 246);
            this.txtNombreBanco.Name = "txtNombreBanco";
            this.txtNombreBanco.Size = new System.Drawing.Size(100, 20);
            this.txtNombreBanco.TabIndex = 11;
            // 
            // txtNumeroCuenta
            // 
            this.txtNumeroCuenta.Location = new System.Drawing.Point(106, 278);
            this.txtNumeroCuenta.Name = "txtNumeroCuenta";
            this.txtNumeroCuenta.Size = new System.Drawing.Size(100, 20);
            this.txtNumeroCuenta.TabIndex = 12;
            // 
            // txtSaldoInicial
            // 
            this.txtSaldoInicial.Location = new System.Drawing.Point(59, 307);
            this.txtSaldoInicial.Name = "txtSaldoInicial";
            this.txtSaldoInicial.Size = new System.Drawing.Size(100, 20);
            this.txtSaldoInicial.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 350);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "MOVIMIENTOS BANCARIOS";
            // 
            // btnNuevoDeposito
            // 
            this.btnNuevoDeposito.Location = new System.Drawing.Point(18, 366);
            this.btnNuevoDeposito.Name = "btnNuevoDeposito";
            this.btnNuevoDeposito.Size = new System.Drawing.Size(75, 23);
            this.btnNuevoDeposito.TabIndex = 15;
            this.btnNuevoDeposito.Text = "Nuevo Deposito";
            this.btnNuevoDeposito.UseVisualStyleBackColor = true;
            this.btnNuevoDeposito.Click += new System.EventHandler(this.btnNuevoDeposito_Click);
            // 
            // btnRefrescarMovimientos
            // 
            this.btnRefrescarMovimientos.Location = new System.Drawing.Point(125, 366);
            this.btnRefrescarMovimientos.Name = "btnRefrescarMovimientos";
            this.btnRefrescarMovimientos.Size = new System.Drawing.Size(133, 23);
            this.btnRefrescarMovimientos.TabIndex = 16;
            this.btnRefrescarMovimientos.Text = "Refrescar Movimientos";
            this.btnRefrescarMovimientos.UseVisualStyleBackColor = true;
            this.btnRefrescarMovimientos.Click += new System.EventHandler(this.btnRefrescarMovimientos_Click);
            // 
            // dgvMovimientos
            // 
            this.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientos.Location = new System.Drawing.Point(12, 395);
            this.dgvMovimientos.Name = "dgvMovimientos";
            this.dgvMovimientos.Size = new System.Drawing.Size(397, 150);
            this.dgvMovimientos.TabIndex = 17;
            // 
            // btnNuevoBanco
            // 
            this.btnNuevoBanco.Location = new System.Drawing.Point(235, 246);
            this.btnNuevoBanco.Name = "btnNuevoBanco";
            this.btnNuevoBanco.Size = new System.Drawing.Size(75, 23);
            this.btnNuevoBanco.TabIndex = 18;
            this.btnNuevoBanco.Text = "Nuevo";
            this.btnNuevoBanco.UseVisualStyleBackColor = true;
            this.btnNuevoBanco.Click += new System.EventHandler(this.btnNuevoBanco_Click);
            // 
            // btnEditarBanco
            // 
            this.btnEditarBanco.Location = new System.Drawing.Point(319, 246);
            this.btnEditarBanco.Name = "btnEditarBanco";
            this.btnEditarBanco.Size = new System.Drawing.Size(75, 23);
            this.btnEditarBanco.TabIndex = 19;
            this.btnEditarBanco.Text = "Editar";
            this.btnEditarBanco.UseVisualStyleBackColor = true;
            this.btnEditarBanco.Click += new System.EventHandler(this.btnEditarBanco_Click);
            // 
            // btnGuardarBanco
            // 
            this.btnGuardarBanco.Location = new System.Drawing.Point(235, 274);
            this.btnGuardarBanco.Name = "btnGuardarBanco";
            this.btnGuardarBanco.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarBanco.TabIndex = 20;
            this.btnGuardarBanco.Text = "Guardar";
            this.btnGuardarBanco.UseVisualStyleBackColor = true;
            this.btnGuardarBanco.Click += new System.EventHandler(this.btnGuardarBanco_Click);
            // 
            // btnEliminarBanco
            // 
            this.btnEliminarBanco.Location = new System.Drawing.Point(319, 276);
            this.btnEliminarBanco.Name = "btnEliminarBanco";
            this.btnEliminarBanco.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarBanco.TabIndex = 21;
            this.btnEliminarBanco.Text = "Eliminar";
            this.btnEliminarBanco.UseVisualStyleBackColor = true;
            this.btnEliminarBanco.Click += new System.EventHandler(this.btnEliminarBanco_Click);
            // 
            // btnCancelarBanco
            // 
            this.btnCancelarBanco.Location = new System.Drawing.Point(281, 307);
            this.btnCancelarBanco.Name = "btnCancelarBanco";
            this.btnCancelarBanco.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarBanco.TabIndex = 22;
            this.btnCancelarBanco.Text = "Cancelar";
            this.btnCancelarBanco.UseVisualStyleBackColor = true;
            this.btnCancelarBanco.Click += new System.EventHandler(this.btnCancelarBanco_Click);
            // 
            // frmBancos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 565);
            this.Controls.Add(this.btnCancelarBanco);
            this.Controls.Add(this.btnEliminarBanco);
            this.Controls.Add(this.btnGuardarBanco);
            this.Controls.Add(this.btnEditarBanco);
            this.Controls.Add(this.btnNuevoBanco);
            this.Controls.Add(this.dgvMovimientos);
            this.Controls.Add(this.btnRefrescarMovimientos);
            this.Controls.Add(this.btnNuevoDeposito);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSaldoInicial);
            this.Controls.Add(this.txtNumeroCuenta);
            this.Controls.Add(this.txtNombreBanco);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvBancos);
            this.Controls.Add(this.btnLimpiarBusqueda);
            this.Controls.Add(this.btnBuscarBancos);
            this.Controls.Add(this.txtBuscarBancos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmBancos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bancos";
            this.Load += new System.EventHandler(this.frmBancos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBancos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBuscarBancos;
        private System.Windows.Forms.Button btnBuscarBancos;
        private System.Windows.Forms.Button btnLimpiarBusqueda;
        private System.Windows.Forms.DataGridView dgvBancos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNombreBanco;
        private System.Windows.Forms.TextBox txtNumeroCuenta;
        private System.Windows.Forms.TextBox txtSaldoInicial;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnNuevoDeposito;
        private System.Windows.Forms.Button btnRefrescarMovimientos;
        private System.Windows.Forms.DataGridView dgvMovimientos;
        private System.Windows.Forms.Button btnNuevoBanco;
        private System.Windows.Forms.Button btnEditarBanco;
        private System.Windows.Forms.Button btnGuardarBanco;
        private System.Windows.Forms.Button btnEliminarBanco;
        private System.Windows.Forms.Button btnCancelarBanco;
    }
}