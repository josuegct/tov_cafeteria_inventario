namespace tov_cafeteria_inventario.Vista
{
    partial class DetalleOrden
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblOrdenID;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label lblFechaOrden;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.Label lblPrecioUnitario;
        private System.Windows.Forms.TextBox txtProducto;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvOrdenDetalles;
        private System.Windows.Forms.Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblOrdenID = new System.Windows.Forms.Label();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.lblFechaOrden = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.lblPrecioUnitario = new System.Windows.Forms.Label();
            this.txtProducto = new System.Windows.Forms.TextBox();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvOrdenDetalles = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtFechaOrden = new System.Windows.Forms.TextBox();
            this.txtProveedor = new System.Windows.Forms.TextBox();
            this.txtOrdenID = new System.Windows.Forms.TextBox();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenDetalles)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOrdenID
            // 
            this.lblOrdenID.AutoSize = true;
            this.lblOrdenID.Location = new System.Drawing.Point(20, 60);
            this.lblOrdenID.Name = "lblOrdenID";
            this.lblOrdenID.Size = new System.Drawing.Size(53, 13);
            this.lblOrdenID.TabIndex = 1;
            this.lblOrdenID.Text = "Orden ID:";
            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Location = new System.Drawing.Point(20, 90);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(59, 13);
            this.lblProveedor.TabIndex = 2;
            this.lblProveedor.Text = "Proveedor:";
            // 
            // lblFechaOrden
            // 
            this.lblFechaOrden.AutoSize = true;
            this.lblFechaOrden.Location = new System.Drawing.Point(20, 120);
            this.lblFechaOrden.Name = "lblFechaOrden";
            this.lblFechaOrden.Size = new System.Drawing.Size(72, 13);
            this.lblFechaOrden.TabIndex = 3;
            this.lblFechaOrden.Text = "Fecha Orden:";
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(20, 150);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(53, 13);
            this.lblProducto.TabIndex = 4;
            this.lblProducto.Text = "Producto:";
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(20, 180);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(52, 13);
            this.lblCantidad.TabIndex = 6;
            this.lblCantidad.Text = "Cantidad:";
            // 
            // lblPrecioUnitario
            // 
            this.lblPrecioUnitario.AutoSize = true;
            this.lblPrecioUnitario.Location = new System.Drawing.Point(20, 210);
            this.lblPrecioUnitario.Name = "lblPrecioUnitario";
            this.lblPrecioUnitario.Size = new System.Drawing.Size(79, 13);
            this.lblPrecioUnitario.TabIndex = 8;
            this.lblPrecioUnitario.Text = "Precio Unitario:";
            // 
            // txtProducto
            // 
            this.txtProducto.Location = new System.Drawing.Point(167, 150);
            this.txtProducto.Name = "txtProducto";
            this.txtProducto.ReadOnly = true;
            this.txtProducto.Size = new System.Drawing.Size(356, 20);
            this.txtProducto.TabIndex = 5;
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(167, 180);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(356, 20);
            this.txtCantidad.TabIndex = 7;
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(167, 210);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(356, 20);
            this.txtPrecio.TabIndex = 9;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(23, 392);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 10;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(185, 392);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 11;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvOrdenDetalles
            // 
            this.dgvOrdenDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenDetalles.Location = new System.Drawing.Point(23, 236);
            this.dgvOrdenDetalles.Name = "dgvOrdenDetalles";
            this.dgvOrdenDetalles.Size = new System.Drawing.Size(500, 150);
            this.dgvOrdenDetalles.TabIndex = 12;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(19, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(196, 22);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Detalles de la Orden";
            // 
            // txtFechaOrden
            // 
            this.txtFechaOrden.Location = new System.Drawing.Point(167, 117);
            this.txtFechaOrden.Name = "txtFechaOrden";
            this.txtFechaOrden.Size = new System.Drawing.Size(356, 20);
            this.txtFechaOrden.TabIndex = 7;
            // 
            // txtProveedor
            // 
            this.txtProveedor.Location = new System.Drawing.Point(167, 87);
            this.txtProveedor.Name = "txtProveedor";
            this.txtProveedor.Size = new System.Drawing.Size(356, 20);
            this.txtProveedor.TabIndex = 7;
            // 
            // txtOrdenID
            // 
            this.txtOrdenID.Location = new System.Drawing.Point(167, 57);
            this.txtOrdenID.Name = "txtOrdenID";
            this.txtOrdenID.Size = new System.Drawing.Size(356, 20);
            this.txtOrdenID.TabIndex = 7;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(266, 392);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(75, 23);
            this.btnActualizar.TabIndex = 10;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Location = new System.Drawing.Point(104, 392);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(75, 23);
            this.btnModificar.TabIndex = 13;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // DetalleOrden
            // 
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblOrdenID);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.lblFechaOrden);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.txtProducto);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.txtOrdenID);
            this.Controls.Add(this.txtProveedor);
            this.Controls.Add(this.txtFechaOrden);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.lblPrecioUnitario);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.dgvOrdenDetalles);
            this.Name = "DetalleOrden";
            this.Text = "Detalles de la Orden";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenDetalles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtFechaOrden;
        private System.Windows.Forms.TextBox txtProveedor;
        private System.Windows.Forms.TextBox txtOrdenID;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnModificar;
    }
}
