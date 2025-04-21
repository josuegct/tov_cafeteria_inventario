namespace tov_cafeteria_inventario.Vista
{
    partial class Existencias
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvExistencias = new System.Windows.Forms.DataGridView();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExistencias)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(330, 26);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(117, 22);
            this.lblTitulo.TabIndex = 12;
            this.lblTitulo.Text = "Existencias";
            // 
            // dgvExistencias
            // 
            this.dgvExistencias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExistencias.Location = new System.Drawing.Point(41, 90);
            this.dgvExistencias.Name = "dgvExistencias";
            this.dgvExistencias.Size = new System.Drawing.Size(719, 353);
            this.dgvExistencias.TabIndex = 13;
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(651, 449);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(109, 23);
            this.btnExportar.TabIndex = 14;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(536, 449);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(109, 23);
            this.btnActualizar.TabIndex = 15;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // Existencias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 498);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.dgvExistencias);
            this.Controls.Add(this.lblTitulo);
            this.Name = "Existencias";
            this.Text = "Existencias";
            ((System.ComponentModel.ISupportInitialize)(this.dgvExistencias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvExistencias;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnActualizar;
    }
}