using System;
using System.Windows.Forms;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reportes : Form
    {
        private readonly int usuarioID;

        public Reportes(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
        }

        private void Reportes_Load(object sender, EventArgs e)
        {
            // Podés cargar datos o configuraciones al cargar si lo necesitás.
        }

        private void btn_reporteUsuarios_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Usuarios(usuarioID);
            AbrirFormulario(form);
        }

        private void btn_ordenesPedidos_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Ordenes_de_Productos(usuarioID);
            AbrirFormulario(form);
        }

        private void btn_reporteInventario_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Inventario(usuarioID);
            AbrirFormulario(form);
        }

        private void btn_reporteMovimientos_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Movimientos_en_el_Sistema(usuarioID);
            AbrirFormulario(form);
        }

        private void AbrirFormulario(Form form)
        {
            form.MdiParent = this.MdiParent;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Close();
        }
    }
}
