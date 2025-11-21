namespace ComercializadoraMedicos
{
    partial class frmArqueoCaja
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
            this.dtpFechaArqueo = new System.Windows.Forms.DateTimePicker();
            this.btnVerReporte = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblVentasCredito = new System.Windows.Forms.Label();
            this.txtVentasContado = new System.Windows.Forms.TextBox();
            this.txtPagosRecibidos = new System.Windows.Forms.TextBox();
            this.lblTotalRecaudado = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTotalDepositado = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDiferencia = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnGuardarArqueo = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ARQUEO DE CAJA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha:";
            // 
            // dtpFechaArqueo
            // 
            this.dtpFechaArqueo.Location = new System.Drawing.Point(76, 52);
            this.dtpFechaArqueo.Name = "dtpFechaArqueo";
            this.dtpFechaArqueo.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaArqueo.TabIndex = 2;
            this.dtpFechaArqueo.ValueChanged += new System.EventHandler(this.dtpFechaArqueo_ValueChanged);
            // 
            // btnVerReporte
            // 
            this.btnVerReporte.Location = new System.Drawing.Point(388, 48);
            this.btnVerReporte.Name = "btnVerReporte";
            this.btnVerReporte.Size = new System.Drawing.Size(75, 23);
            this.btnVerReporte.TabIndex = 3;
            this.btnVerReporte.Text = "Ver Reporte";
            this.btnVerReporte.UseVisualStyleBackColor = true;
            this.btnVerReporte.Click += new System.EventHandler(this.btnVerReporte_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "INGRESOS DEL DIA";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Ventas al Contado:";
            // 
            // btnCalcular
            // 
            this.btnCalcular.Location = new System.Drawing.Point(86, 422);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(75, 23);
            this.btnCalcular.TabIndex = 6;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Pagos Recibidos:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Total Recaudado:";
            // 
            // lblVentasCredito
            // 
            this.lblVentasCredito.AutoSize = true;
            this.lblVentasCredito.Location = new System.Drawing.Point(38, 238);
            this.lblVentasCredito.Name = "lblVentasCredito";
            this.lblVentasCredito.Size = new System.Drawing.Size(35, 13);
            this.lblVentasCredito.TabIndex = 9;
            this.lblVentasCredito.Text = "label7";
            // 
            // txtVentasContado
            // 
            this.txtVentasContado.Location = new System.Drawing.Point(136, 140);
            this.txtVentasContado.Name = "txtVentasContado";
            this.txtVentasContado.Size = new System.Drawing.Size(158, 20);
            this.txtVentasContado.TabIndex = 10;
            this.txtVentasContado.TextChanged += new System.EventHandler(this.txtVentasContado_TextChanged);
            // 
            // txtPagosRecibidos
            // 
            this.txtPagosRecibidos.Location = new System.Drawing.Point(131, 169);
            this.txtPagosRecibidos.Name = "txtPagosRecibidos";
            this.txtPagosRecibidos.Size = new System.Drawing.Size(163, 20);
            this.txtPagosRecibidos.TabIndex = 11;
            this.txtPagosRecibidos.TextChanged += new System.EventHandler(this.txtPagosRecibidos_TextChanged);
            // 
            // lblTotalRecaudado
            // 
            this.lblTotalRecaudado.AutoSize = true;
            this.lblTotalRecaudado.Location = new System.Drawing.Point(138, 206);
            this.lblTotalRecaudado.Name = "lblTotalRecaudado";
            this.lblTotalRecaudado.Size = new System.Drawing.Size(35, 13);
            this.lblTotalRecaudado.TabIndex = 12;
            this.lblTotalRecaudado.Text = "label7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "DEPOSITOS Y CONTROL";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 303);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Total Depositado:";
            // 
            // txtTotalDepositado
            // 
            this.txtTotalDepositado.Location = new System.Drawing.Point(141, 303);
            this.txtTotalDepositado.Name = "txtTotalDepositado";
            this.txtTotalDepositado.Size = new System.Drawing.Size(100, 20);
            this.txtTotalDepositado.TabIndex = 15;
            this.txtTotalDepositado.TextChanged += new System.EventHandler(this.txtTotalDepositado_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(41, 332);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Diferencia:";
            // 
            // lblDiferencia
            // 
            this.lblDiferencia.AutoSize = true;
            this.lblDiferencia.Location = new System.Drawing.Point(133, 332);
            this.lblDiferencia.Name = "lblDiferencia";
            this.lblDiferencia.Size = new System.Drawing.Size(41, 13);
            this.lblDiferencia.TabIndex = 17;
            this.lblDiferencia.Text = "label10";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 357);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Estado:";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(83, 357);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(41, 13);
            this.lblEstado.TabIndex = 19;
            this.lblEstado.Text = "label11";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(41, 386);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(136, 386);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(342, 20);
            this.txtObservaciones.TabIndex = 21;
            // 
            // btnGuardarArqueo
            // 
            this.btnGuardarArqueo.Location = new System.Drawing.Point(180, 422);
            this.btnGuardarArqueo.Name = "btnGuardarArqueo";
            this.btnGuardarArqueo.Size = new System.Drawing.Size(139, 23);
            this.btnGuardarArqueo.TabIndex = 22;
            this.btnGuardarArqueo.Text = "Guardar Arqueo";
            this.btnGuardarArqueo.UseVisualStyleBackColor = true;
            this.btnGuardarArqueo.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(325, 422);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 23;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmArqueoCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 457);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardarArqueo);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblDiferencia);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtTotalDepositado);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTotalRecaudado);
            this.Controls.Add(this.txtPagosRecibidos);
            this.Controls.Add(this.txtVentasContado);
            this.Controls.Add(this.lblVentasCredito);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnVerReporte);
            this.Controls.Add(this.dtpFechaArqueo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmArqueoCaja";
            this.Text = "frmArqueoCaja";
            this.Load += new System.EventHandler(this.frmArqueoCaja_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFechaArqueo;
        private System.Windows.Forms.Button btnVerReporte;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblVentasCredito;
        private System.Windows.Forms.TextBox txtVentasContado;
        private System.Windows.Forms.TextBox txtPagosRecibidos;
        private System.Windows.Forms.Label lblTotalRecaudado;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTotalDepositado;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDiferencia;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnGuardarArqueo;
        private System.Windows.Forms.Button btnCancelar;
    }
}