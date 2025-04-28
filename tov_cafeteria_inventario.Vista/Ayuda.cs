using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Ayuda : Form
    {
        public Ayuda()
        {
            InitializeComponent();
        }

        private void Ayuda_Load(object sender, EventArgs e)
        {
        }

        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            string rutaPDF = Application.StartupPath + @"\Manual_Usuario_Cafeteria_TOV.pdf";

            if (File.Exists(rutaPDF))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = rutaPDF;
                    startInfo.UseShellExecute = true;
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo abrir el manual de usuario.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el manual de usuario.\n\nRuta buscada: " + rutaPDF, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
