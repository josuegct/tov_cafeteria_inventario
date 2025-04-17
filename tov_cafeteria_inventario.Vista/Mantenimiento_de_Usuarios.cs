using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Mantenimiento_de_Usuarios : Form
    {
        private readonly UsuarioController usuarioController = new UsuarioController();
        private readonly int usuarioID;

        public Mantenimiento_de_Usuarios(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Mantenimiento_de_Usuarios_FormClosed;
            CargarRoles();
            CargarUsuarios();
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
        }

        private void Mantenimiento_de_Usuarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mantenimientos menu = new Mantenimientos(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = usuarioController.ObtenerUsuarios();
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
        }

        private void CargarRoles()
        {
            var roles = usuarioController.ObtenerRoles();
            cmbRol.DataSource = roles;
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "RoleID";
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var row = dgvUsuarios.SelectedRows[0];
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtUsuario.Text = row.Cells["Usuario"].Value.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
                cmbRol.Text = row.Cells["RolNombre"].Value.ToString();
                chkActivo.Checked = Convert.ToBoolean(row.Cells["Estado"].Value);
                txtContraseña.Clear();
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtUsuario.Clear();
            txtCorreo.Clear();
            txtContraseña.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = false;
            dgvUsuarios.ClearSelection();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text == "" || txtUsuario.Text == "" || txtCorreo.Text == "" || txtContraseña.Text == "")
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                UsuarioSistema nuevoUsuario = new UsuarioSistema
                {
                    Nombre = txtNombre.Text,
                    Usuario = txtUsuario.Text,
                    Correo = txtCorreo.Text,
                    RoleID = Convert.ToInt32(cmbRol.SelectedValue),
                    Estado = chkActivo.Checked,
                    PasswordHash = usuarioController.EncriptarContraseña(txtContraseña.Text)
                };

                usuarioController.AgregarUsuario(nuevoUsuario);
                MessageBox.Show("Usuario agregado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un usuario.");
                    return;
                }

                int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["UsuarioID"].Value);

                UsuarioSistema usuarioModificado = new UsuarioSistema
                {
                    UsuarioID = id,
                    Nombre = txtNombre.Text,
                    Usuario = txtUsuario.Text,
                    Correo = txtCorreo.Text,
                    RoleID = Convert.ToInt32(cmbRol.SelectedValue),
                    Estado = chkActivo.Checked,
                    PasswordHash = string.IsNullOrWhiteSpace(txtContraseña.Text)
                        ? usuarioController.ObtenerPasswordActual(id)
                        : usuarioController.EncriptarContraseña(txtContraseña.Text)
                };

                usuarioController.ModificarUsuario(usuarioModificado);
                MessageBox.Show("Usuario modificado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar este usuario?", "Confirmación", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            try
            {
                int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["UsuarioID"].Value);
                usuarioController.EliminarUsuario(id);
                MessageBox.Show("Usuario eliminado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}
