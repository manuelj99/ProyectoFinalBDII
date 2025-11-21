namespace ComercializadoraMedicos
{
    partial class frmElaboracionProductos
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
            this.cmbProductoElaborado = new System.Windows.Forms.ComboBox();
            this.lblPrecioActual = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCantidadElaborada = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbMateriaPrima = new System.Windows.Forms.ComboBox();
            this.lblStockMateriaPrima = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPrecioMateriaPrima = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCantidadUtilizada = new System.Windows.Forms.TextBox();
            this.btnAgregarMateriaPrima = new System.Windows.Forms.Button();
            this.dgvDetalleElaboracion = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.btnQuitarMateriaPrima = new System.Windows.Forms.Button();
            this.lblCostoTotal = new System.Windows.Forms.Label();
            this.lblPrecioSugerido = new System.Windows.Forms.Label();
            this.btnActualizarPrecio = new System.Windows.Forms.Button();
            this.btnProcesarElaboracion = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleElaboracion)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ELABORACION DE PRODUCTOS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "PRODUCTO A ELABORAR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Producto:";
            // 
            // cmbProductoElaborado
            // 
            this.cmbProductoElaborado.FormattingEnabled = true;
            this.cmbProductoElaborado.Location = new System.Drawing.Point(105, 83);
            this.cmbProductoElaborado.Name = "cmbProductoElaborado";
            this.cmbProductoElaborado.Size = new System.Drawing.Size(162, 21);
            this.cmbProductoElaborado.TabIndex = 3;
            this.cmbProductoElaborado.SelectedIndexChanged += new System.EventHandler(this.cmbProductoElaborado_SelectedIndexChanged);
            // 
            // lblPrecioActual
            // 
            this.lblPrecioActual.AutoSize = true;
            this.lblPrecioActual.Location = new System.Drawing.Point(293, 86);
            this.lblPrecioActual.Name = "lblPrecioActual";
            this.lblPrecioActual.Size = new System.Drawing.Size(35, 13);
            this.lblPrecioActual.TabIndex = 4;
            this.lblPrecioActual.Text = "label4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Cantidad a Elaborar:";
            // 
            // txtCantidadElaborada
            // 
            this.txtCantidadElaborada.Location = new System.Drawing.Point(153, 110);
            this.txtCantidadElaborada.Name = "txtCantidadElaborada";
            this.txtCantidadElaborada.Size = new System.Drawing.Size(130, 20);
            this.txtCantidadElaborada.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "MATERIAS PRIMAS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Materia Prima:";
            // 
            // cmbMateriaPrima
            // 
            this.cmbMateriaPrima.FormattingEnabled = true;
            this.cmbMateriaPrima.Location = new System.Drawing.Point(111, 178);
            this.cmbMateriaPrima.Name = "cmbMateriaPrima";
            this.cmbMateriaPrima.Size = new System.Drawing.Size(172, 21);
            this.cmbMateriaPrima.TabIndex = 9;
            this.cmbMateriaPrima.SelectedIndexChanged += new System.EventHandler(this.cmbMateriaPrima_SelectedIndexChanged);
            // 
            // lblStockMateriaPrima
            // 
            this.lblStockMateriaPrima.AutoSize = true;
            this.lblStockMateriaPrima.Location = new System.Drawing.Point(293, 181);
            this.lblStockMateriaPrima.Name = "lblStockMateriaPrima";
            this.lblStockMateriaPrima.Size = new System.Drawing.Size(35, 13);
            this.lblStockMateriaPrima.TabIndex = 10;
            this.lblStockMateriaPrima.Text = "label7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Precio:";
            // 
            // lblPrecioMateriaPrima
            // 
            this.lblPrecioMateriaPrima.AutoSize = true;
            this.lblPrecioMateriaPrima.Location = new System.Drawing.Point(82, 212);
            this.lblPrecioMateriaPrima.Name = "lblPrecioMateriaPrima";
            this.lblPrecioMateriaPrima.Size = new System.Drawing.Size(35, 13);
            this.lblPrecioMateriaPrima.TabIndex = 12;
            this.lblPrecioMateriaPrima.Text = "label8";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Cantidad a Usar:";
            // 
            // txtCantidadUtilizada
            // 
            this.txtCantidadUtilizada.Location = new System.Drawing.Point(132, 244);
            this.txtCantidadUtilizada.Name = "txtCantidadUtilizada";
            this.txtCantidadUtilizada.Size = new System.Drawing.Size(100, 20);
            this.txtCantidadUtilizada.TabIndex = 14;
            // 
            // btnAgregarMateriaPrima
            // 
            this.btnAgregarMateriaPrima.Location = new System.Drawing.Point(193, 270);
            this.btnAgregarMateriaPrima.Name = "btnAgregarMateriaPrima";
            this.btnAgregarMateriaPrima.Size = new System.Drawing.Size(120, 23);
            this.btnAgregarMateriaPrima.TabIndex = 15;
            this.btnAgregarMateriaPrima.Text = "Agregar Materia Prima";
            this.btnAgregarMateriaPrima.UseVisualStyleBackColor = true;
            this.btnAgregarMateriaPrima.Click += new System.EventHandler(this.btnAgregarMateriaPrima_Click);
            // 
            // dgvDetalleElaboracion
            // 
            this.dgvDetalleElaboracion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleElaboracion.Location = new System.Drawing.Point(12, 322);
            this.dgvDetalleElaboracion.Name = "dgvDetalleElaboracion";
            this.dgvDetalleElaboracion.Size = new System.Drawing.Size(501, 133);
            this.dgvDetalleElaboracion.TabIndex = 16;
            this.dgvDetalleElaboracion.SelectionChanged += new System.EventHandler(this.dgvDetalleElaboracion_SelectionChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(152, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "DETALLE DE ELABORACION";
            // 
            // btnQuitarMateriaPrima
            // 
            this.btnQuitarMateriaPrima.Location = new System.Drawing.Point(388, 461);
            this.btnQuitarMateriaPrima.Name = "btnQuitarMateriaPrima";
            this.btnQuitarMateriaPrima.Size = new System.Drawing.Size(117, 23);
            this.btnQuitarMateriaPrima.TabIndex = 18;
            this.btnQuitarMateriaPrima.Text = "Quitar Materia Prima";
            this.btnQuitarMateriaPrima.UseVisualStyleBackColor = true;
            this.btnQuitarMateriaPrima.Click += new System.EventHandler(this.btnQuitarMateriaPrima_Click);
            // 
            // lblCostoTotal
            // 
            this.lblCostoTotal.AutoSize = true;
            this.lblCostoTotal.Location = new System.Drawing.Point(18, 482);
            this.lblCostoTotal.Name = "lblCostoTotal";
            this.lblCostoTotal.Size = new System.Drawing.Size(41, 13);
            this.lblCostoTotal.TabIndex = 19;
            this.lblCostoTotal.Text = "label10";
            // 
            // lblPrecioSugerido
            // 
            this.lblPrecioSugerido.AutoSize = true;
            this.lblPrecioSugerido.Location = new System.Drawing.Point(18, 508);
            this.lblPrecioSugerido.Name = "lblPrecioSugerido";
            this.lblPrecioSugerido.Size = new System.Drawing.Size(41, 13);
            this.lblPrecioSugerido.TabIndex = 20;
            this.lblPrecioSugerido.Text = "label10";
            // 
            // btnActualizarPrecio
            // 
            this.btnActualizarPrecio.Location = new System.Drawing.Point(213, 508);
            this.btnActualizarPrecio.Name = "btnActualizarPrecio";
            this.btnActualizarPrecio.Size = new System.Drawing.Size(97, 23);
            this.btnActualizarPrecio.TabIndex = 21;
            this.btnActualizarPrecio.Text = "Actualizar Precio";
            this.btnActualizarPrecio.UseVisualStyleBackColor = true;
            this.btnActualizarPrecio.Click += new System.EventHandler(this.btnActualizarPrecio_Click);
            // 
            // btnProcesarElaboracion
            // 
            this.btnProcesarElaboracion.Location = new System.Drawing.Point(313, 508);
            this.btnProcesarElaboracion.Name = "btnProcesarElaboracion";
            this.btnProcesarElaboracion.Size = new System.Drawing.Size(119, 23);
            this.btnProcesarElaboracion.TabIndex = 22;
            this.btnProcesarElaboracion.Text = "Procesar Elaboracion";
            this.btnProcesarElaboracion.UseVisualStyleBackColor = true;
            this.btnProcesarElaboracion.Click += new System.EventHandler(this.btnProcesarElaboracion_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(438, 508);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 23;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmElaboracionProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 549);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesarElaboracion);
            this.Controls.Add(this.btnActualizarPrecio);
            this.Controls.Add(this.lblPrecioSugerido);
            this.Controls.Add(this.lblCostoTotal);
            this.Controls.Add(this.btnQuitarMateriaPrima);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dgvDetalleElaboracion);
            this.Controls.Add(this.btnAgregarMateriaPrima);
            this.Controls.Add(this.txtCantidadUtilizada);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblPrecioMateriaPrima);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblStockMateriaPrima);
            this.Controls.Add(this.cmbMateriaPrima);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCantidadElaborada);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPrecioActual);
            this.Controls.Add(this.cmbProductoElaborado);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmElaboracionProductos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Elaboracion de Productos";
            this.Load += new System.EventHandler(this.frmElaboracionProductos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleElaboracion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProductoElaborado;
        private System.Windows.Forms.Label lblPrecioActual;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCantidadElaborada;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbMateriaPrima;
        private System.Windows.Forms.Label lblStockMateriaPrima;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblPrecioMateriaPrima;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCantidadUtilizada;
        private System.Windows.Forms.Button btnAgregarMateriaPrima;
        private System.Windows.Forms.DataGridView dgvDetalleElaboracion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnQuitarMateriaPrima;
        private System.Windows.Forms.Label lblCostoTotal;
        private System.Windows.Forms.Label lblPrecioSugerido;
        private System.Windows.Forms.Button btnActualizarPrecio;
        private System.Windows.Forms.Button btnProcesarElaboracion;
        private System.Windows.Forms.Button btnCancelar;
    }
}