﻿using System;
using System.Drawing;
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

            txtCorreo.ForeColor = Color.Gray;
            txtCorreo.Text = "ejemplo@correo.com";
            txtCorreo.Enter += txtCorreo_Enter;
            txtCorreo.Leave += txtCorreo_Leave;
        }

        private void txtCorreo_Enter(object sender, EventArgs e)
        {
            if (txtCorreo.Text == "ejemplo@correo.com")
            {
                txtCorreo.Text = "";
                txtCorreo.ForeColor = Color.Black;
            }
        }

        private void txtCorreo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                txtCorreo.Text = "ejemplo@correo.com";
                txtCorreo.ForeColor = Color.Gray;
            }
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
                chkActivo.Checked = row.Cells["Estado"].Value.ToString() == "Activo";
                txtContraseña.Clear();
                txtConfirmarContraseña.Clear();
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtUsuario.Clear();
            txtCorreo.Clear();
            txtContraseña.Clear();
            txtConfirmarContraseña.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = false;
            dgvUsuarios.ClearSelection();
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

                if (!string.IsNullOrWhiteSpace(txtContraseña.Text))
                {
                    if (txtContraseña.Text != txtConfirmarContraseña.Text)
                    {
                        MessageBox.Show("Las contraseñas no coinciden.");
                        return;
                    }
                }

                int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["UsuarioID"].Value);

                UsuarioSistema usuarioModificado = new UsuarioSistema
                {
                    UsuarioID = id,
                    Nombre = txtNombre.Text,
                    Usuario = txtUsuario.Text,
                    Correo = txtCorreo.Text,
                    RoleID = Convert.ToInt32(cmbRol.SelectedValue),
                    Estado = chkActivo.Checked ? "Activo" : "Inactivo",
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

        private void Mantenimiento_de_Usuarios_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                CargarUsuarios();
            }
            else
            {
                dgvUsuarios.DataSource = usuarioController.BuscarUsuarios(filtro);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text == "" || txtUsuario.Text == "" || txtCorreo.Text == "" || txtContraseña.Text == "" || txtConfirmarContraseña.Text == "")
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                if (txtContraseña.Text != txtConfirmarContraseña.Text)
                {
                    MessageBox.Show("Las contraseñas no coinciden.");
                    return;
                }

                UsuarioSistema nuevoUsuario = new UsuarioSistema
                {
                    Nombre = txtNombre.Text,
                    Usuario = txtUsuario.Text,
                    Correo = txtCorreo.Text,
                    RoleID = Convert.ToInt32(cmbRol.SelectedValue),
                    Estado = chkActivo.Checked ? "Activo" : "Inactivo",
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
    }
}
