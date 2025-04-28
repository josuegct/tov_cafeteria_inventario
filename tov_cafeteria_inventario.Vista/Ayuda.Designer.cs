namespace tov_cafeteria_inventario.Vista
{
    partial class Ayuda
    {
        private System.ComponentModel.IContainer components = null;

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
            this.btnManualUsuario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManualUsuario
            // 
            this.btnManualUsuario.ForeColor = System.Drawing.Color.Red;
            this.btnManualUsuario.Location = new System.Drawing.Point(95, 149);
            this.btnManualUsuario.Name = "btnManualUsuario";
            this.btnManualUsuario.Size = new System.Drawing.Size(176, 149);
            this.btnManualUsuario.TabIndex = 0;
            this.btnManualUsuario.Text = "MANUAL DE USUARIO";
            this.btnManualUsuario.UseVisualStyleBackColor = true;
            this.btnManualUsuario.Click += new System.EventHandler(this.btnManualUsuario_Click);
            // 
            // Ayuda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 486);
            this.Controls.Add(this.btnManualUsuario);
            this.Name = "Ayuda";
            this.Text = "Ayuda";
            this.Load += new System.EventHandler(this.Ayuda_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnManualUsuario;
    }
}