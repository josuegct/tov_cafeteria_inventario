using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Login : Form
    {
        private readonly LoginController loginController = new LoginController();

        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var resultado = loginController.AutenticarUsuario(usuario, contrasena);

                if (resultado.usuarioID > 0)
                {
                    MessageBox.Show("Inicio de sesión exitoso!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ REGISTRAR EN BITÁCORA
                    var bitacoraController = new BitacoraController();
                    bitacoraController.RegistrarAccion(resultado.usuarioID, "Inicio de sesión");

                    this.Hide();
                    Pantalla_Principal mainForm = new Pantalla_Principal(resultado.usuarioID);
                    mainForm.Show();
                }

                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos o el usuario no está activo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
