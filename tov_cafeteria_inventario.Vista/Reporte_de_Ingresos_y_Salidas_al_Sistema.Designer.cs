namespace tov_cafeteria_inventario.Vista
{
    partial class Reporte_de_Ingresos_y_Salidas_al_Sistema
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewIngresosSalidas;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridViewIngresosSalidas = new System.Windows.Forms.DataGridView();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnExportartxt = new System.Windows.Forms.Button();
            this.cmbUsuarios = new System.Windows.Forms.ComboBox();
            this.lblUsuarios = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpFin = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.btnFiltrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIngresosSalidas)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewIngresosSalidas
            // 
            this.dataGridViewIngresosSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIngresosSalidas.Location = new System.Drawing.Point(36, 206);
            this.dataGridViewIngresosSalidas.Name = "dataGridViewIngresosSalidas";
            this.dataGridViewIngresosSalidas.Size = new System.Drawing.Size(740, 358);
            this.dataGridViewIngresosSalidas.TabIndex = 0;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(570, 570);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 30);
            this.btnActualizar.TabIndex = 1;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(210, 39);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(387, 22);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "Reporte de Ingresos y Salidas al Sistema";
            // 
            // btnExportartxt
            // 
            this.btnExportartxt.Location = new System.Drawing.Point(676, 570);
            this.btnExportartxt.Name = "btnExportartxt";
            this.btnExportartxt.Size = new System.Drawing.Size(100, 30);
            this.btnExportartxt.TabIndex = 3;
            this.btnExportartxt.Text = "Exportar";
            this.btnExportartxt.UseVisualStyleBackColor = true;
            this.btnExportartxt.Click += new System.EventHandler(this.btnExportartxt_Click);
            // 
            // cmbUsuarios
            // 
            this.cmbUsuarios.FormattingEnabled = true;
            this.cmbUsuarios.Location = new System.Drawing.Point(95, 114);
            this.cmbUsuarios.Name = "cmbUsuarios";
            this.cmbUsuarios.Size = new System.Drawing.Size(198, 21);
            this.cmbUsuarios.TabIndex = 4;
            // 
            // lblUsuarios
            // 
            this.lblUsuarios.AutoSize = true;
            this.lblUsuarios.Location = new System.Drawing.Point(33, 117);
            this.lblUsuarios.Name = "lblUsuarios";
            this.lblUsuarios.Size = new System.Drawing.Size(48, 13);
            this.lblUsuarios.TabIndex = 5;
            this.lblUsuarios.Text = "Usuarios";
            // 
            // dtpInicio
            // 
            this.dtpInicio.Location = new System.Drawing.Point(398, 115);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(200, 20);
            this.dtpInicio.TabIndex = 8;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(336, 117);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(38, 13);
            this.lblDesde.TabIndex = 5;
            this.lblDesde.Text = "Desde";
            // 
            // dtpFin
            // 
            this.dtpFin.Location = new System.Drawing.Point(397, 149);
            this.dtpFin.Name = "dtpFin";
            this.dtpFin.Size = new System.Drawing.Size(200, 20);
            this.dtpFin.TabIndex = 9;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(336, 152);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(35, 13);
            this.lblHasta.TabIndex = 5;
            this.lblHasta.Text = "Hasta";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(677, 115);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(99, 30);
            this.btnFiltrar.TabIndex = 10;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // Reporte_de_Ingresos_y_Salidas_al_Sistema
            // 
            this.ClientSize = new System.Drawing.Size(812, 621);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.dtpFin);
            this.Controls.Add(this.dtpInicio);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.lblUsuarios);
            this.Controls.Add(this.cmbUsuarios);
            this.Controls.Add(this.btnExportartxt);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.dataGridViewIngresosSalidas);
            this.Name = "Reporte_de_Ingresos_y_Salidas_al_Sistema";
            this.Text = "Reporte de Ingresos y Salidas al Sistema";
            this.Load += new System.EventHandler(this.Reporte_de_Ingresos_y_Salidas_al_Sistema_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIngresosSalidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnExportartxt;
        private System.Windows.Forms.ComboBox cmbUsuarios;
        private System.Windows.Forms.Label lblUsuarios;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpFin;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.Button btnFiltrar;
    }
}
