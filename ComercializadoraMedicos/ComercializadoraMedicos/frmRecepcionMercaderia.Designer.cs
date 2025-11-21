namespace ComercializadoraMedicos
{
    partial class frmRecepcionMercaderia
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
            this.cmbOrdenCompra = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBodega = new System.Windows.Forms.ComboBox();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.lblFechaOrden = new System.Windows.Forms.Label();
            this.lblFechaEsperada = new System.Windows.Forms.Label();
            this.lblTotalOrden = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvDetalleRecepcion = new System.Windows.Forms.DataGridView();
            this.btnAceptarTodo = new System.Windows.Forms.Button();
            this.btnRechazarTodo = new System.Windows.Forms.Button();
            this.btnProcesarRecepcion = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleRecepcion)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RECEPCION DE MERCADERIA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "INFORMACION DE ORDEN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Orden Compra:";
            // 
            // cmbOrdenCompra
            // 
            this.cmbOrdenCompra.FormattingEnabled = true;
            this.cmbOrdenCompra.Location = new System.Drawing.Point(129, 84);
            this.cmbOrdenCompra.Name = "cmbOrdenCompra";
            this.cmbOrdenCompra.Size = new System.Drawing.Size(121, 21);
            this.cmbOrdenCompra.TabIndex = 3;
            this.cmbOrdenCompra.SelectedIndexChanged += new System.EventHandler(this.cmbOrdenCompra_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Bodega:";
            // 
            // cmbBodega
            // 
            this.cmbBodega.FormattingEnabled = true;
            this.cmbBodega.Location = new System.Drawing.Point(85, 111);
            this.cmbBodega.Name = "cmbBodega";
            this.cmbBodega.Size = new System.Drawing.Size(121, 21);
            this.cmbBodega.TabIndex = 5;
            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Location = new System.Drawing.Point(33, 148);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(0, 13);
            this.lblProveedor.TabIndex = 6;
            // 
            // lblFechaOrden
            // 
            this.lblFechaOrden.AutoSize = true;
            this.lblFechaOrden.Location = new System.Drawing.Point(33, 178);
            this.lblFechaOrden.Name = "lblFechaOrden";
            this.lblFechaOrden.Size = new System.Drawing.Size(0, 13);
            this.lblFechaOrden.TabIndex = 7;
            // 
            // lblFechaEsperada
            // 
            this.lblFechaEsperada.AutoSize = true;
            this.lblFechaEsperada.Location = new System.Drawing.Point(267, 178);
            this.lblFechaEsperada.Name = "lblFechaEsperada";
            this.lblFechaEsperada.Size = new System.Drawing.Size(0, 13);
            this.lblFechaEsperada.TabIndex = 8;
            // 
            // lblTotalOrden
            // 
            this.lblTotalOrden.AutoSize = true;
            this.lblTotalOrden.Location = new System.Drawing.Point(36, 209);
            this.lblTotalOrden.Name = "lblTotalOrden";
            this.lblTotalOrden.Size = new System.Drawing.Size(0, 13);
            this.lblTotalOrden.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(121, 237);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(100, 20);
            this.txtObservaciones.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 271);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "DETALLE DE RECEPCION";
            // 
            // dgvDetalleRecepcion
            // 
            this.dgvDetalleRecepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleRecepcion.Location = new System.Drawing.Point(10, 296);
            this.dgvDetalleRecepcion.Name = "dgvDetalleRecepcion";
            this.dgvDetalleRecepcion.Size = new System.Drawing.Size(423, 150);
            this.dgvDetalleRecepcion.TabIndex = 13;
            // 
            // btnAceptarTodo
            // 
            this.btnAceptarTodo.Location = new System.Drawing.Point(12, 452);
            this.btnAceptarTodo.Name = "btnAceptarTodo";
            this.btnAceptarTodo.Size = new System.Drawing.Size(96, 23);
            this.btnAceptarTodo.TabIndex = 14;
            this.btnAceptarTodo.Text = "Aceptar Todo";
            this.btnAceptarTodo.UseVisualStyleBackColor = true;
            this.btnAceptarTodo.Click += new System.EventHandler(this.btnAceptarTodo_Click);
            // 
            // btnRechazarTodo
            // 
            this.btnRechazarTodo.Location = new System.Drawing.Point(121, 453);
            this.btnRechazarTodo.Name = "btnRechazarTodo";
            this.btnRechazarTodo.Size = new System.Drawing.Size(100, 23);
            this.btnRechazarTodo.TabIndex = 15;
            this.btnRechazarTodo.Text = "Rechazar Todo";
            this.btnRechazarTodo.UseVisualStyleBackColor = true;
            this.btnRechazarTodo.Click += new System.EventHandler(this.btnRechazarTodo_Click);
            // 
            // btnProcesarRecepcion
            // 
            this.btnProcesarRecepcion.Location = new System.Drawing.Point(277, 452);
            this.btnProcesarRecepcion.Name = "btnProcesarRecepcion";
            this.btnProcesarRecepcion.Size = new System.Drawing.Size(75, 23);
            this.btnProcesarRecepcion.TabIndex = 16;
            this.btnProcesarRecepcion.Text = "Procesar";
            this.btnProcesarRecepcion.UseVisualStyleBackColor = true;
            this.btnProcesarRecepcion.Click += new System.EventHandler(this.btnProcesarRecepcion_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(358, 452);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 17;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmRecepcionMercaderia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 484);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarRecepcion);
            this.Controls.Add(this.btnRechazarTodo);
            this.Controls.Add(this.btnAceptarTodo);
            this.Controls.Add(this.dgvDetalleRecepcion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTotalOrden);
            this.Controls.Add(this.lblFechaEsperada);
            this.Controls.Add(this.lblFechaOrden);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.cmbBodega);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbOrdenCompra);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmRecepcionMercaderia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recepcion de Mercaderia";
            this.Load += new System.EventHandler(this.frmRecepcionMercaderia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleRecepcion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOrdenCompra;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBodega;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label lblFechaOrden;
        private System.Windows.Forms.Label lblFechaEsperada;
        private System.Windows.Forms.Label lblTotalOrden;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvDetalleRecepcion;
        private System.Windows.Forms.Button btnAceptarTodo;
        private System.Windows.Forms.Button btnRechazarTodo;
        private System.Windows.Forms.Button btnProcesarRecepcion;
        private System.Windows.Forms.Button btnCancelar;
    }
}