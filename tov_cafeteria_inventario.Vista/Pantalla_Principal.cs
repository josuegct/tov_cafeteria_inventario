using System;
using System.Linq;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;
using tov_cafeteria_inventario.Vista;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Pantalla_Principal : Form
    {
        private int usuarioID;
        private int roleID;
        private PantallaPrincipalController controller;
        private Ordenes ordenesForm;
        private Inventario inventarioForm;
        private Existencias existenciasForm;
        private Mantenimientos mantenimientosForm;
        private Reportes reportesForm;
        private Bitacora bitacoraForm;
        private AcercaDe acercaDeForm;
        private Ayuda ayudaForm;

        public Pantalla_Principal(int usuarioID)
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.usuarioID = usuarioID;
            controller = new PantallaPrincipalController();
            this.roleID = controller.ObtenerRoleID(usuarioID);
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {
            if (this.roleID != 1)
            {
                bitacorasToolStripMenuItem.Enabled = false;
                mantenimientoToolStripMenuItem.Enabled = false;
                reportesToolStripMenuItem.Enabled = false;
                inventarioToolStripMenuItem.Enabled = false;
            }
        }

        private void ordenesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.ordenesForm = new Ordenes(this.usuarioID);
            this.ordenesForm.MdiParent = this;
            this.ordenesForm.Show();
        }

        private void mantenimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder a Mantenimientos.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.mantenimientosForm = new Mantenimientos(this.usuarioID);
            this.mantenimientosForm.MdiParent = this;
            this.mantenimientosForm.Show();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder a Reportes.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.reportesForm = new Reportes(usuarioID);
            this.reportesForm.MdiParent = this;
            this.reportesForm.Show();
        }

        private void bitacorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder a la bitácora.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.bitacoraForm = new Bitacora(this.usuarioID);
            this.bitacoraForm.MdiParent = this;
            this.bitacoraForm.Show();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.acercaDeForm = new AcercaDe();
            this.acercaDeForm.MdiParent = this;
            this.acercaDeForm.Show();
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ayudaForm = new Ayuda();
            this.ayudaForm.MdiParent = this;
            this.ayudaForm.Show();
        }

        private void inventarioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder al Inventario.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (Form form in this.MdiChildren)
            {
                if (form is Inventario)
                {
                    form.Activate();
                    return;
                }
            }

            this.inventarioForm = new Inventario(this.usuarioID);
            this.inventarioForm.MdiParent = this;
            this.inventarioForm.StartPosition = FormStartPosition.CenterScreen;
            this.inventarioForm.Show();
        }

        private void existenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form is Existencias)
                {
                    if (form.WindowState == FormWindowState.Minimized)
                        form.WindowState = FormWindowState.Normal;

                    form.BringToFront();
                    form.Activate();
                    return;
                }
            }

            this.existenciasForm = new Existencias(this.roleID);
            this.existenciasForm.MdiParent = this;
            this.existenciasForm.StartPosition = FormStartPosition.CenterScreen;
            this.existenciasForm.Show();
        }
    }
    internal class InventarioForm : Inventario
    {
        public InventarioForm(int usuarioID) : base(usuarioID) { }
    }
}