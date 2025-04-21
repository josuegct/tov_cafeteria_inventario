using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Mantenimientos : Form
    {
        private int usuarioID;
        private readonly UsuarioController usuarioController = new UsuarioController();

        public Mantenimientos(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;

            // Validación de permiso (solo RoleID = 1)
            int roleID = usuarioController.ObtenerRoleID(usuarioID);
            if (roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder al módulo de Mantenimientos.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void Mantenimientos_Load(object sender, EventArgs e)
        {
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioYSalir(new Mantenimiento_de_Usuarios(usuarioID));
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormularioYSalir(new Mantenimiento_de_Proveedores(usuarioID));
        }

        private void btn_mantenimientoUsuarios_Click(object sender, EventArgs e)
        {
            btnUsuarios_Click(sender, e);
        }

        private void btn_mantenimientoRolesPermisos_Click(object sender, EventArgs e)
        {
            AbrirFormularioYSalir(new Mantenimiento_de_RolesyPermisos(usuarioID));
        }

        private void btn_mantenimientoProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormularioYSalir(new Mantenimiento_de_Proveedores(usuarioID));
        }

        private void AbrirFormularioYSalir(Form form)
        {
            form.MdiParent = this.MdiParent;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Close();
        }
    }
}
