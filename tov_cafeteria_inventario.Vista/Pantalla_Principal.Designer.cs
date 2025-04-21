namespace tov_cafeteria_inventario.Vista
{
    partial class Pantalla_Principal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar recursos.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary> ro
        /// Método requerido para el soporte del Diseñador: no modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ordenesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existenciasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventarioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacorasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordenesToolStripMenuItem,
            this.inventarioToolStripMenuItem,
            this.mantenimientoToolStripMenuItem,
            this.reportesToolStripMenuItem,
            this.bitacorasToolStripMenuItem,
            this.acercaDeToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ordenesToolStripMenuItem
            // 
            this.ordenesToolStripMenuItem.Name = "ordenesToolStripMenuItem";
            this.ordenesToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.ordenesToolStripMenuItem.Text = "Órdenes";
            this.ordenesToolStripMenuItem.Click += new System.EventHandler(this.ordenesToolStripMenuItem_Click_1);
            // 
            // inventarioToolStripMenuItem
            // 
            this.inventarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.existenciasToolStripMenuItem,
            this.inventarioToolStripMenuItem1});
            this.inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            this.inventarioToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.inventarioToolStripMenuItem.Text = "Inventario";
            // 
            // existenciasToolStripMenuItem
            // 
            this.existenciasToolStripMenuItem.Name = "existenciasToolStripMenuItem";
            this.existenciasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.existenciasToolStripMenuItem.Text = "Existencias";
            this.existenciasToolStripMenuItem.Click += new System.EventHandler(this.existenciasToolStripMenuItem_Click);
            // 
            // inventarioToolStripMenuItem1
            // 
            this.inventarioToolStripMenuItem1.Name = "inventarioToolStripMenuItem1";
            this.inventarioToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.inventarioToolStripMenuItem1.Text = "Inventario";
            this.inventarioToolStripMenuItem1.Click += new System.EventHandler(this.inventarioToolStripMenuItem1_Click);
            // 
            // mantenimientoToolStripMenuItem
            // 
            this.mantenimientoToolStripMenuItem.Name = "mantenimientoToolStripMenuItem";
            this.mantenimientoToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.mantenimientoToolStripMenuItem.Text = "Mantenimiento";
            this.mantenimientoToolStripMenuItem.Click += new System.EventHandler(this.mantenimientoToolStripMenuItem_Click);
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.reportesToolStripMenuItem.Text = "Reportes";
            this.reportesToolStripMenuItem.Click += new System.EventHandler(this.reportesToolStripMenuItem_Click);
            // 
            // bitacorasToolStripMenuItem
            // 
            this.bitacorasToolStripMenuItem.Name = "bitacorasToolStripMenuItem";
            this.bitacorasToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.bitacorasToolStripMenuItem.Text = "Bitácoras";
            this.bitacorasToolStripMenuItem.Click += new System.EventHandler(this.bitacorasToolStripMenuItem_Click);
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.acercaDeToolStripMenuItem.Text = "Acerca de...";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.acercaDeToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            this.ayudaToolStripMenuItem.Click += new System.EventHandler(this.ayudaToolStripMenuItem_Click);
            // 
            // Pantalla_Principal
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Pantalla_Principal";
            this.Text = "Sistema de Inventario - Cafetería";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Pantalla_Principal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ordenesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacorasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existenciasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventarioToolStripMenuItem1;
    }
}

