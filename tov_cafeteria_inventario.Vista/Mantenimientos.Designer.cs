namespace tov_cafeteria_inventario.Vista
{
    partial class Mantenimientos
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lbl_titulo;
        private System.Windows.Forms.Button btn_mantenimientoUsuarios;
        private System.Windows.Forms.Button btn_mantenimientoRolesPermisos;
        private System.Windows.Forms.Button btn_mantenimientoProveedores;

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
            this.btn_mantenimientoUsuarios = new System.Windows.Forms.Button();
            this.btn_mantenimientoRolesPermisos = new System.Windows.Forms.Button();
            this.btn_mantenimientoProveedores = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_titulo
            // 
            this.lbl_titulo.AutoSize = true;
            this.lbl_titulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lbl_titulo.Location = new System.Drawing.Point(47, 22);
            this.lbl_titulo.Name = "lbl_titulo";
            this.lbl_titulo.Size = new System.Drawing.Size(261, 22);
            this.lbl_titulo.TabIndex = 0;
            this.lbl_titulo.Text = "Módulo de Mantenimientos";
            // 
            // btn_mantenimientoUsuarios
            // 
            this.btn_mantenimientoUsuarios.Location = new System.Drawing.Point(78, 81);
            this.btn_mantenimientoUsuarios.Name = "btn_mantenimientoUsuarios";
            this.btn_mantenimientoUsuarios.Size = new System.Drawing.Size(200, 50);
            this.btn_mantenimientoUsuarios.TabIndex = 1;
            this.btn_mantenimientoUsuarios.Text = "Usuarios";
            this.btn_mantenimientoUsuarios.Click += new System.EventHandler(this.btn_mantenimientoUsuarios_Click);
            // 
            // btn_mantenimientoRolesPermisos
            // 
            this.btn_mantenimientoRolesPermisos.Location = new System.Drawing.Point(78, 141);
            this.btn_mantenimientoRolesPermisos.Name = "btn_mantenimientoRolesPermisos";
            this.btn_mantenimientoRolesPermisos.Size = new System.Drawing.Size(200, 50);
            this.btn_mantenimientoRolesPermisos.TabIndex = 2;
            this.btn_mantenimientoRolesPermisos.Text = "Roles y Permisos";
            this.btn_mantenimientoRolesPermisos.Click += new System.EventHandler(this.btn_mantenimientoRolesPermisos_Click);
            // 
            // btn_mantenimientoProveedores
            // 
            this.btn_mantenimientoProveedores.Location = new System.Drawing.Point(78, 201);
            this.btn_mantenimientoProveedores.Name = "btn_mantenimientoProveedores";
            this.btn_mantenimientoProveedores.Size = new System.Drawing.Size(200, 50);
            this.btn_mantenimientoProveedores.TabIndex = 3;
            this.btn_mantenimientoProveedores.Text = "Proveedores";
            this.btn_mantenimientoProveedores.Click += new System.EventHandler(this.btn_mantenimientoProveedores_Click);
            // 
            // Mantenimientos
            // 
            this.ClientSize = new System.Drawing.Size(353, 300);
            this.Controls.Add(this.lbl_titulo);
            this.Controls.Add(this.btn_mantenimientoUsuarios);
            this.Controls.Add(this.btn_mantenimientoRolesPermisos);
            this.Controls.Add(this.btn_mantenimientoProveedores);
            this.Name = "Mantenimientos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mantenimientos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
