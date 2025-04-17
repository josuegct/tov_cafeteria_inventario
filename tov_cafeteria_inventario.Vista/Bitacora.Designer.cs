namespace tov_cafeteria_inventario.Vista
{
    partial class Bitacora
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btn_movimientosSistema;
        private System.Windows.Forms.Button btn_reporteIngresoSalida;
        private System.Windows.Forms.Label lbl_bitacora;

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
            this.btn_movimientosSistema = new System.Windows.Forms.Button();
            this.btn_reporteIngresoSalida = new System.Windows.Forms.Button();
            this.lbl_bitacora = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_movimientosSistema
            // 
            this.btn_movimientosSistema.Location = new System.Drawing.Point(82, 202);
            this.btn_movimientosSistema.Name = "btn_movimientosSistema";
            this.btn_movimientosSistema.Size = new System.Drawing.Size(160, 121);
            this.btn_movimientosSistema.TabIndex = 1;
            this.btn_movimientosSistema.Text = "Movimientos en el Sistema";
            this.btn_movimientosSistema.UseVisualStyleBackColor = true;
            this.btn_movimientosSistema.Click += new System.EventHandler(this.btn_movimientosSistema_Click);
            // 
            // btn_reporteIngresoSalida
            // 
            this.btn_reporteIngresoSalida.Location = new System.Drawing.Point(82, 77);
            this.btn_reporteIngresoSalida.Name = "btn_reporteIngresoSalida";
            this.btn_reporteIngresoSalida.Size = new System.Drawing.Size(160, 121);
            this.btn_reporteIngresoSalida.TabIndex = 2;
            this.btn_reporteIngresoSalida.Text = "Ingresos y Salidas";
            this.btn_reporteIngresoSalida.UseVisualStyleBackColor = true;
            this.btn_reporteIngresoSalida.Click += new System.EventHandler(this.btn_reporteIngresoSalida_Click);
            // 
            // lbl_bitacora
            // 
            this.lbl_bitacora.AutoSize = true;
            this.lbl_bitacora.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lbl_bitacora.Location = new System.Drawing.Point(106, 28);
            this.lbl_bitacora.Name = "lbl_bitacora";
            this.lbl_bitacora.Size = new System.Drawing.Size(110, 22);
            this.lbl_bitacora.TabIndex = 0;
            this.lbl_bitacora.Text = "BITÁCORA";
            // 
            // Bitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 366);
            this.Controls.Add(this.lbl_bitacora);
            this.Controls.Add(this.btn_movimientosSistema);
            this.Controls.Add(this.btn_reporteIngresoSalida);
            this.Name = "Bitacora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitácora";
            this.Load += new System.EventHandler(this.Bitacora_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
