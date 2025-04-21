using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reportes : Form
    {
        private readonly int usuarioID;
        private readonly UsuarioController usuarioController = new UsuarioController();

        public Reportes(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;

            // Validación de rol antes de permitir uso del formulario
            int roleID = usuarioController.ObtenerRoleID(usuarioID);
            if (roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder al módulo de Reportes.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
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
