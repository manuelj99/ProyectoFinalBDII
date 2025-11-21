namespace ComercializadoraMedicos
{
    partial class frmPagosProveedores
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
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.lblSaldoProveedor = new System.Windows.Forms.Label();
            this.lblLimiteCredito = new System.Windows.Forms.Label();
            this.lblProyeccionProveedor = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBanco = new System.Windows.Forms.ComboBox();
            this.lblSaldoBanco = new System.Windows.Forms.Label();
            this.lblProyeccionBanco = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvOrdenesPendientes = new System.Windows.Forms.DataGridView();
            this.lblTotalSeleccionado = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbTipoPago = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNumeroDocumento = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.lblMensajeValidacion = new System.Windows.Forms.Label();
            this.btnProcesarPago = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesPendientes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PAGOS A PROVEEDORES";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "INFORMACION DEL PROVEEDOR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Proveedor:";
            // 
            // cmbProveedor
            // 
            this.cmbProveedor.FormattingEnabled = true;
            this.cmbProveedor.Location = new System.Drawing.Point(99, 84);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(121, 21);
            this.cmbProveedor.TabIndex = 3;
            this.cmbProveedor.SelectedIndexChanged += new System.EventHandler(this.cmbProveedor_SelectedIndexChanged);
            // 
            // lblSaldoProveedor
            // 
            this.lblSaldoProveedor.AutoSize = true;
            this.lblSaldoProveedor.Location = new System.Drawing.Point(33, 112);
            this.lblSaldoProveedor.Name = "lblSaldoProveedor";
            this.lblSaldoProveedor.Size = new System.Drawing.Size(35, 13);
            this.lblSaldoProveedor.TabIndex = 4;
            this.lblSaldoProveedor.Text = "label4";
            // 
            // lblLimiteCredito
            // 
            this.lblLimiteCredito.AutoSize = true;
            this.lblLimiteCredito.Location = new System.Drawing.Point(281, 112);
            this.lblLimiteCredito.Name = "lblLimiteCredito";
            this.lblLimiteCredito.Size = new System.Drawing.Size(35, 13);
            this.lblLimiteCredito.TabIndex = 5;
            this.lblLimiteCredito.Text = "label5";
            // 
            // lblProyeccionProveedor
            // 
            this.lblProyeccionProveedor.AutoSize = true;
            this.lblProyeccionProveedor.Location = new System.Drawing.Point(33, 147);
            this.lblProyeccionProveedor.Name = "lblProyeccionProveedor";
            this.lblProyeccionProveedor.Size = new System.Drawing.Size(35, 13);
            this.lblProyeccionProveedor.TabIndex = 6;
            this.lblProyeccionProveedor.Text = "label6";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "INFORMACION DEL BANCO";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Bnaco:";
            // 
            // cmbBanco
            // 
            this.cmbBanco.FormattingEnabled = true;
            this.cmbBanco.Location = new System.Drawing.Point(77, 203);
            this.cmbBanco.Name = "cmbBanco";
            this.cmbBanco.Size = new System.Drawing.Size(121, 21);
            this.cmbBanco.TabIndex = 9;
            this.cmbBanco.SelectedIndexChanged += new System.EventHandler(this.cmbBanco_SelectedIndexChanged);
            // 
            // lblSaldoBanco
            // 
            this.lblSaldoBanco.AutoSize = true;
            this.lblSaldoBanco.Location = new System.Drawing.Point(33, 240);
            this.lblSaldoBanco.Name = "lblSaldoBanco";
            this.lblSaldoBanco.Size = new System.Drawing.Size(35, 13);
            this.lblSaldoBanco.TabIndex = 10;
            this.lblSaldoBanco.Text = "label6";
            // 
            // lblProyeccionBanco
            // 
            this.lblProyeccionBanco.AutoSize = true;
            this.lblProyeccionBanco.Location = new System.Drawing.Point(251, 240);
            this.lblProyeccionBanco.Name = "lblProyeccionBanco";
            this.lblProyeccionBanco.Size = new System.Drawing.Size(35, 13);
            this.lblProyeccionBanco.TabIndex = 11;
            this.lblProyeccionBanco.Text = "label6";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 275);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "ORDENES PENDIENTES DE PAGO";
            // 
            // dgvOrdenesPendientes
            // 
            this.dgvOrdenesPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesPendientes.Location = new System.Drawing.Point(13, 291);
            this.dgvOrdenesPendientes.Name = "dgvOrdenesPendientes";
            this.dgvOrdenesPendientes.Size = new System.Drawing.Size(499, 79);
            this.dgvOrdenesPendientes.TabIndex = 13;
            this.dgvOrdenesPendientes.SelectionChanged += new System.EventHandler(this.dgvOrdenesPendientes_SelectionChanged);
            // 
            // lblTotalSeleccionado
            // 
            this.lblTotalSeleccionado.AutoSize = true;
            this.lblTotalSeleccionado.Location = new System.Drawing.Point(13, 377);
            this.lblTotalSeleccionado.Name = "lblTotalSeleccionado";
            this.lblTotalSeleccionado.Size = new System.Drawing.Size(35, 13);
            this.lblTotalSeleccionado.TabIndex = 14;
            this.lblTotalSeleccionado.Text = "label7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 404);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "DETALLES DEL PAGO";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(284, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Seleccionar Todo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSeleccionarTodo_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(396, 377);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "Deseleccionar Todo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnDeseleccionarTodo_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 433);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Tipo Pago:";
            // 
            // cmbTipoPago
            // 
            this.cmbTipoPago.FormattingEnabled = true;
            this.cmbTipoPago.Items.AddRange(new object[] {
            "Cheque",
            "Transferencia"});
            this.cmbTipoPago.Location = new System.Drawing.Point(85, 433);
            this.cmbTipoPago.Name = "cmbTipoPago";
            this.cmbTipoPago.Size = new System.Drawing.Size(121, 21);
            this.cmbTipoPago.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 467);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "No. Documento:";
            // 
            // txtNumeroDocumento
            // 
            this.txtNumeroDocumento.Location = new System.Drawing.Point(120, 464);
            this.txtNumeroDocumento.Name = "txtNumeroDocumento";
            this.txtNumeroDocumento.Size = new System.Drawing.Size(148, 20);
            this.txtNumeroDocumento.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 502);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Monto:";
            // 
            // txtMonto
            // 
            this.txtMonto.Location = new System.Drawing.Point(85, 502);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(100, 20);
            this.txtMonto.TabIndex = 23;
            this.txtMonto.TextChanged += new System.EventHandler(this.txtMonto_TextChanged);
            // 
            // lblMensajeValidacion
            // 
            this.lblMensajeValidacion.AutoSize = true;
            this.lblMensajeValidacion.Location = new System.Drawing.Point(325, 440);
            this.lblMensajeValidacion.Name = "lblMensajeValidacion";
            this.lblMensajeValidacion.Size = new System.Drawing.Size(41, 13);
            this.lblMensajeValidacion.TabIndex = 24;
            this.lblMensajeValidacion.Text = "label11";
            // 
            // btnProcesarPago
            // 
            this.btnProcesarPago.Location = new System.Drawing.Point(291, 482);
            this.btnProcesarPago.Name = "btnProcesarPago";
            this.btnProcesarPago.Size = new System.Drawing.Size(99, 23);
            this.btnProcesarPago.TabIndex = 25;
            this.btnProcesarPago.Text = "Procesar Pago";
            this.btnProcesarPago.UseVisualStyleBackColor = true;
            this.btnProcesarPago.Click += new System.EventHandler(this.btnProcesarPago_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(396, 482);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 26;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmPagosProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 536);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarPago);
            this.Controls.Add(this.lblMensajeValidacion);
            this.Controls.Add(this.txtMonto);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtNumeroDocumento);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbTipoPago);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTotalSeleccionado);
            this.Controls.Add(this.dgvOrdenesPendientes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblProyeccionBanco);
            this.Controls.Add(this.lblSaldoBanco);
            this.Controls.Add(this.cmbBanco);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblProyeccionProveedor);
            this.Controls.Add(this.lblLimiteCredito);
            this.Controls.Add(this.lblSaldoProveedor);
            this.Controls.Add(this.cmbProveedor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPagosProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pago a Proveedores";
            this.Load += new System.EventHandler(this.frmPagosProveedores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesPendientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label lblSaldoProveedor;
        private System.Windows.Forms.Label lblLimiteCredito;
        private System.Windows.Forms.Label lblProyeccionProveedor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBanco;
        private System.Windows.Forms.Label lblSaldoBanco;
        private System.Windows.Forms.Label lblProyeccionBanco;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvOrdenesPendientes;
        private System.Windows.Forms.Label lblTotalSeleccionado;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbTipoPago;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNumeroDocumento;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Label lblMensajeValidacion;
        private System.Windows.Forms.Button btnProcesarPago;
        private System.Windows.Forms.Button btnCancelar;
    }
}