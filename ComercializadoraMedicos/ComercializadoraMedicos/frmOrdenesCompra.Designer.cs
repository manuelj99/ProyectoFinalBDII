namespace ComercializadoraMedicos
{
    partial class frmOrdenesCompra
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
            this.label4 = new System.Windows.Forms.Label();
            this.dtpFechaEsperada = new System.Windows.Forms.DateTimePicker();
            this.lblLimiteCredito = new System.Windows.Forms.Label();
            this.lblSaldoActual = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.lblStockActual = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPrecioCompra = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.btnAgregarProducto = new System.Windows.Forms.Button();
            this.dgvDetalleOrden = new System.Windows.Forms.DataGridView();
            this.btnQuitarProducto = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTotalOrden = new System.Windows.Forms.Label();
            this.lblMensajeCredito = new System.Windows.Forms.Label();
            this.btnGenerarAutomaticas = new System.Windows.Forms.Button();
            this.btnProcesarOrden = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleOrden)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ORDEN DE COMPRA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "INFORMACION DEL PROVEEDOR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Proveedor:";
            // 
            // cmbProveedor
            // 
            this.cmbProveedor.FormattingEnabled = true;
            this.cmbProveedor.Location = new System.Drawing.Point(99, 81);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(121, 21);
            this.cmbProveedor.TabIndex = 3;
            this.cmbProveedor.SelectedIndexChanged += new System.EventHandler(this.cmbProveedor_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Fecha esperada:";
            // 
            // dtpFechaEsperada
            // 
            this.dtpFechaEsperada.Location = new System.Drawing.Point(140, 105);
            this.dtpFechaEsperada.Name = "dtpFechaEsperada";
            this.dtpFechaEsperada.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaEsperada.TabIndex = 5;
            // 
            // lblLimiteCredito
            // 
            this.lblLimiteCredito.AutoSize = true;
            this.lblLimiteCredito.Location = new System.Drawing.Point(29, 135);
            this.lblLimiteCredito.Name = "lblLimiteCredito";
            this.lblLimiteCredito.Size = new System.Drawing.Size(0, 13);
            this.lblLimiteCredito.TabIndex = 6;
            // 
            // lblSaldoActual
            // 
            this.lblSaldoActual.AutoSize = true;
            this.lblSaldoActual.Location = new System.Drawing.Point(222, 135);
            this.lblSaldoActual.Name = "lblSaldoActual";
            this.lblSaldoActual.Size = new System.Drawing.Size(0, 13);
            this.lblSaldoActual.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "AGREGAR PRODUCTOS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Producto:";
            // 
            // cmbProducto
            // 
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(101, 208);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(121, 21);
            this.cmbProducto.TabIndex = 10;
            this.cmbProducto.SelectedIndexChanged += new System.EventHandler(this.cmbProducto_SelectedIndexChanged);
            // 
            // lblStockActual
            // 
            this.lblStockActual.AutoSize = true;
            this.lblStockActual.Location = new System.Drawing.Point(266, 208);
            this.lblStockActual.Name = "lblStockActual";
            this.lblStockActual.Size = new System.Drawing.Size(0, 13);
            this.lblStockActual.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Precio Compra:";
            // 
            // txtPrecioCompra
            // 
            this.txtPrecioCompra.Location = new System.Drawing.Point(119, 237);
            this.txtPrecioCompra.Name = "txtPrecioCompra";
            this.txtPrecioCompra.Size = new System.Drawing.Size(100, 20);
            this.txtPrecioCompra.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(239, 237);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Cantidad:";
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(304, 237);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(100, 20);
            this.txtCantidad.TabIndex = 15;
            // 
            // btnAgregarProducto
            // 
            this.btnAgregarProducto.Location = new System.Drawing.Point(203, 276);
            this.btnAgregarProducto.Name = "btnAgregarProducto";
            this.btnAgregarProducto.Size = new System.Drawing.Size(109, 23);
            this.btnAgregarProducto.TabIndex = 16;
            this.btnAgregarProducto.Text = "Agregar Producto";
            this.btnAgregarProducto.UseVisualStyleBackColor = true;
            this.btnAgregarProducto.Click += new System.EventHandler(this.btnAgregarProducto_Click);
            // 
            // dgvDetalleOrden
            // 
            this.dgvDetalleOrden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleOrden.Location = new System.Drawing.Point(12, 305);
            this.dgvDetalleOrden.Name = "dgvDetalleOrden";
            this.dgvDetalleOrden.Size = new System.Drawing.Size(491, 150);
            this.dgvDetalleOrden.TabIndex = 17;
            // 
            // btnQuitarProducto
            // 
            this.btnQuitarProducto.Location = new System.Drawing.Point(389, 461);
            this.btnQuitarProducto.Name = "btnQuitarProducto";
            this.btnQuitarProducto.Size = new System.Drawing.Size(103, 23);
            this.btnQuitarProducto.TabIndex = 18;
            this.btnQuitarProducto.Text = "Quitar Producto";
            this.btnQuitarProducto.UseVisualStyleBackColor = true;
            this.btnQuitarProducto.Click += new System.EventHandler(this.btnQuitarProducto_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 486);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "TOTAL ORDEN:";
            // 
            // lblTotalOrden
            // 
            this.lblTotalOrden.AutoSize = true;
            this.lblTotalOrden.Location = new System.Drawing.Point(107, 485);
            this.lblTotalOrden.Name = "lblTotalOrden";
            this.lblTotalOrden.Size = new System.Drawing.Size(0, 13);
            this.lblTotalOrden.TabIndex = 20;
            // 
            // lblMensajeCredito
            // 
            this.lblMensajeCredito.AutoSize = true;
            this.lblMensajeCredito.Location = new System.Drawing.Point(13, 512);
            this.lblMensajeCredito.Name = "lblMensajeCredito";
            this.lblMensajeCredito.Size = new System.Drawing.Size(0, 13);
            this.lblMensajeCredito.TabIndex = 21;
            // 
            // btnGenerarAutomaticas
            // 
            this.btnGenerarAutomaticas.Location = new System.Drawing.Point(59, 529);
            this.btnGenerarAutomaticas.Name = "btnGenerarAutomaticas";
            this.btnGenerarAutomaticas.Size = new System.Drawing.Size(124, 23);
            this.btnGenerarAutomaticas.TabIndex = 22;
            this.btnGenerarAutomaticas.Text = "Generar Automaticas";
            this.btnGenerarAutomaticas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarAutomaticas.UseVisualStyleBackColor = true;
            this.btnGenerarAutomaticas.Click += new System.EventHandler(this.btnGenerarAutomaticas_Click);
            // 
            // btnProcesarOrden
            // 
            this.btnProcesarOrden.Location = new System.Drawing.Point(203, 529);
            this.btnProcesarOrden.Name = "btnProcesarOrden";
            this.btnProcesarOrden.Size = new System.Drawing.Size(97, 23);
            this.btnProcesarOrden.TabIndex = 23;
            this.btnProcesarOrden.Text = "Procesar Orden";
            this.btnProcesarOrden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProcesarOrden.UseVisualStyleBackColor = true;
            this.btnProcesarOrden.Click += new System.EventHandler(this.btnProcesarOrden_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(329, 529);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 24;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmOrdenesCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 564);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarOrden);
            this.Controls.Add(this.btnGenerarAutomaticas);
            this.Controls.Add(this.lblMensajeCredito);
            this.Controls.Add(this.lblTotalOrden);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnQuitarProducto);
            this.Controls.Add(this.dgvDetalleOrden);
            this.Controls.Add(this.btnAgregarProducto);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPrecioCompra);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblStockActual);
            this.Controls.Add(this.cmbProducto);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblSaldoActual);
            this.Controls.Add(this.lblLimiteCredito);
            this.Controls.Add(this.dtpFechaEsperada);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbProveedor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmOrdenesCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ordenes de Compra";
            this.Load += new System.EventHandler(this.frmOrdenesCompra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleOrden)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpFechaEsperada;
        private System.Windows.Forms.Label lblLimiteCredito;
        private System.Windows.Forms.Label lblSaldoActual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Label lblStockActual;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPrecioCompra;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Button btnAgregarProducto;
        private System.Windows.Forms.DataGridView dgvDetalleOrden;
        private System.Windows.Forms.Button btnQuitarProducto;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblTotalOrden;
        private System.Windows.Forms.Label lblMensajeCredito;
        private System.Windows.Forms.Button btnGenerarAutomaticas;
        private System.Windows.Forms.Button btnProcesarOrden;
        private System.Windows.Forms.Button btnCancelar;
    }
}