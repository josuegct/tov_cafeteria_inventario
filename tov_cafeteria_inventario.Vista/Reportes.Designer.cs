namespace tov_cafeteria_inventario.Vista
{
    partial class Reportes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lbl_titulo;
        private System.Windows.Forms.Button btn_reporteUsuarios;
        private System.Windows.Forms.Button btn_ordenesPedidos;
        private System.Windows.Forms.Button btn_reporteInventario;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbl_titulo = new System.Windows.Forms.Label();
            this.btn_reporteUsuarios = new System.Windows.Forms.Button();
            this.btn_ordenesPedidos = new System.Windows.Forms.Button();
            this.btn_reporteInventario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_titulo
            // 
            this.lbl_titulo.AutoSize = true;
            this.lbl_titulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lbl_titulo.Location = new System.Drawing.Point(76, 19);
            this.lbl_titulo.Name = "lbl_titulo";
            this.lbl_titulo.Size = new System.Drawing.Size(209, 22);
            this.lbl_titulo.TabIndex = 0;
            this.lbl_titulo.Text = "Reportes Disponibles";
            // 
            // btn_reporteUsuarios
            // 
            this.btn_reporteUsuarios.Location = new System.Drawing.Point(50, 80);
            this.btn_reporteUsuarios.Name = "btn_reporteUsuarios";
            this.btn_reporteUsuarios.Size = new System.Drawing.Size(250, 50);
            this.btn_reporteUsuarios.TabIndex = 1;
            this.btn_reporteUsuarios.Text = "Reporte de Usuarios";
            this.btn_reporteUsuarios.UseVisualStyleBackColor = true;
            this.btn_reporteUsuarios.Click += new System.EventHandler(this.btn_reporteUsuarios_Click);
            // 
            // btn_ordenesPedidos
            // 
            this.btn_ordenesPedidos.Location = new System.Drawing.Point(50, 140);
            this.btn_ordenesPedidos.Name = "btn_ordenesPedidos";
            this.btn_ordenesPedidos.Size = new System.Drawing.Size(250, 50);
            this.btn_ordenesPedidos.TabIndex = 2;
            this.btn_ordenesPedidos.Text = "Reporte de Órdenes y Pedidos";
            this.btn_ordenesPedidos.UseVisualStyleBackColor = true;
            this.btn_ordenesPedidos.Click += new System.EventHandler(this.btn_ordenesPedidos_Click);
            // 
            // btn_reporteInventario
            // 
            this.btn_reporteInventario.Location = new System.Drawing.Point(50, 200);
            this.btn_reporteInventario.Name = "btn_reporteInventario";
            this.btn_reporteInventario.Size = new System.Drawing.Size(250, 50);
            this.btn_reporteInventario.TabIndex = 3;
            this.btn_reporteInventario.Text = "Reporte de Inventario";
            this.btn_reporteInventario.UseVisualStyleBackColor = true;
            this.btn_reporteInventario.Click += new System.EventHandler(this.btn_reporteInventario_Click);
            // 
            // Reportes
            // 
            this.ClientSize = new System.Drawing.Size(350, 300);
            this.Controls.Add(this.lbl_titulo);
            this.Controls.Add(this.btn_reporteUsuarios);
            this.Controls.Add(this.btn_ordenesPedidos);
            this.Controls.Add(this.btn_reporteInventario);
            this.Name = "Reportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.Reportes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
