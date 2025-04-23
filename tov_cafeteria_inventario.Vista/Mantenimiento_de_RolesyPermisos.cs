using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Mantenimiento_de_RolesyPermisos : Form
    {
        private readonly RolController rolController = new RolController();
        private readonly int usuarioID;

        public Mantenimiento_de_RolesyPermisos(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Mantenimiento_de_RolesyPermisos_FormClosed;
            this.Load += Mantenimiento_de_RolesyPermisos_Load;

            dataGridViewRoles.SelectionChanged += dataGridViewRoles_SelectionChanged;
        }

        private void Mantenimiento_de_RolesyPermisos_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mantenimientos menu = new Mantenimientos(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void Mantenimiento_de_RolesyPermisos_Load(object sender, EventArgs e)
        {
            CargarRoles();
        }

        private void CargarRoles()
        {
            try
            {
                var roles = rolController.ObtenerRoles();
                dataGridViewRoles.DataSource = roles;

                dataGridViewRoles.ReadOnly = true;
                dataGridViewRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewRoles.MultiSelect = false;
                dataGridViewRoles.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los roles: " + ex.Message);
            }
        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un rol para modificar.");
                return;
            }

            int roleID = Convert.ToInt32(dataGridViewRoles.SelectedRows[0].Cells["RoleID"].Value);

            Rol rolModificado = new Rol
            {
                RoleID = roleID,
                NombreRol = txt_nombreRol.Text,
                Descripcion = txt_descripcion.Text
            };

            try
            {
                rolController.ModificarRol(rolModificado);
                MessageBox.Show("Rol modificado correctamente.");
                CargarRoles();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el rol: " + ex.Message);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un rol para eliminar.");
                return;
            }

            int roleID = Convert.ToInt32(dataGridViewRoles.SelectedRows[0].Cells["RoleID"].Value);

            DialogResult result = MessageBox.Show("¿Está seguro de eliminar este rol?", "Confirmación", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            try
            {
                rolController.EliminarRol(roleID);
                MessageBox.Show("Rol eliminado correctamente.");
                CargarRoles();
                LimpiarCampos();
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message, "Operación no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el rol: " + ex.Message);
            }
        }

        private void dataGridViewRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewRoles.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewRoles.SelectedRows[0];
                txt_nombreRol.Text = row.Cells["NombreRol"].Value?.ToString() ?? "";
                txt_descripcion.Text = row.Cells["Descripcion"].Value?.ToString() ?? "";
            }
        }

        private void LimpiarCampos()
        {
            txt_nombreRol.Clear();
            txt_descripcion.Clear();
            dataGridViewRoles.ClearSelection();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            "Rol agregado correctamente.\n\n⚠️ Contacte al administrador del sistema para asignar permisos de módulos.",
            "Rol guardado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
);
            if (string.IsNullOrWhiteSpace(txt_nombreRol.Text) || string.IsNullOrWhiteSpace(txt_descripcion.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            Rol nuevoRol = new Rol
            {
                NombreRol = txt_nombreRol.Text,
                Descripcion = txt_descripcion.Text
            };

            try
            {
                rolController.AgregarRol(nuevoRol);
                MessageBox.Show("Rol agregado correctamente.\n\nIMPORTANTE: Contacte al desarrollador para aplicar restricciones de módulos si es necesario.");
                CargarRoles();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el rol: " + ex.Message);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            txt_nombreRol.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                CargarRoles(); // Si no hay texto, carga todos
            }
            else
            {
                try
                {
                    var resultado = rolController.BuscarRoles(filtro);
                    dataGridViewRoles.DataSource = resultado;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar roles: " + ex.Message);
                }
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarRoles();
        }
    }
}
