namespace ComercializadoraMedicos
{
    partial class frmPagosClientes
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
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSaldoPendiente = new System.Windows.Forms.Label();
            this.lblTotalPendiente = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvVentasPendientes = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numMontoPago = new System.Windows.Forms.NumericUpDown();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btnProcesarPago = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMontoPago)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RECIBOS DE PAGO DE CLIENTES";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cliente:";
            // 
            // cmbCliente
            // 
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(74, 54);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(179, 21);
            this.cmbCliente.TabIndex = 2;
            this.cmbCliente.SelectedIndexChanged += new System.EventHandler(this.cmbCliente_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "INFORMACION DEL CLIENTE";
            // 
            // lblSaldoPendiente
            // 
            this.lblSaldoPendiente.AutoSize = true;
            this.lblSaldoPendiente.Location = new System.Drawing.Point(28, 111);
            this.lblSaldoPendiente.Name = "lblSaldoPendiente";
            this.lblSaldoPendiente.Size = new System.Drawing.Size(35, 13);
            this.lblSaldoPendiente.TabIndex = 5;
            this.lblSaldoPendiente.Text = "label4";
            // 
            // lblTotalPendiente
            // 
            this.lblTotalPendiente.AutoSize = true;
            this.lblTotalPendiente.Location = new System.Drawing.Point(256, 111);
            this.lblTotalPendiente.Name = "lblTotalPendiente";
            this.lblTotalPendiente.Size = new System.Drawing.Size(35, 13);
            this.lblTotalPendiente.TabIndex = 6;
            this.lblTotalPendiente.Text = "label4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "VENTAS PENDIENTES DE PAGO";
            // 
            // dgvVentasPendientes
            // 
            this.dgvVentasPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasPendientes.Location = new System.Drawing.Point(13, 157);
            this.dgvVentasPendientes.Name = "dgvVentasPendientes";
            this.dgvVentasPendientes.Size = new System.Drawing.Size(483, 150);
            this.dgvVentasPendientes.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 346);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "REGISTRAT PAGO GENERAL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Monto del Pago:";
            // 
            // numMontoPago
            // 
            this.numMontoPago.Location = new System.Drawing.Point(104, 372);
            this.numMontoPago.Name = "numMontoPago";
            this.numMontoPago.Size = new System.Drawing.Size(120, 20);
            this.numMontoPago.TabIndex = 12;
            this.numMontoPago.ValueChanged += new System.EventHandler(this.numMontoPago_ValueChanged);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(16, 402);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(35, 13);
            this.lblMensaje.TabIndex = 13;
            this.lblMensaje.Text = "label7";
            // 
            // btnProcesarPago
            // 
            this.btnProcesarPago.Location = new System.Drawing.Point(312, 374);
            this.btnProcesarPago.Name = "btnProcesarPago";
            this.btnProcesarPago.Size = new System.Drawing.Size(102, 23);
            this.btnProcesarPago.TabIndex = 14;
            this.btnProcesarPago.Text = "Procesar Pago";
            this.btnProcesarPago.UseVisualStyleBackColor = true;
            this.btnProcesarPago.Click += new System.EventHandler(this.btnProcesarPago_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(327, 413);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 15;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmPagosClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 462);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarPago);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.numMontoPago);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvVentasPendientes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTotalPendiente);
            this.Controls.Add(this.lblSaldoPendiente);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbCliente);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmPagosClientes";
            this.Text = "frmPagosClientes";
            this.Load += new System.EventHandler(this.frmPagosClientes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMontoPago)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSaldoPendiente;
        private System.Windows.Forms.Label lblTotalPendiente;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvVentasPendientes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMontoPago;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnProcesarPago;
        private System.Windows.Forms.Button btnCancelar;
    }
}