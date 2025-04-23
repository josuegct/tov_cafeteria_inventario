using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class LoginController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public (int usuarioID, int roleID) AutenticarUsuario(string usuario, string contrasena)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT UsuarioID, PasswordHash, RoleID FROM Usuarios WHERE Usuario = @Usuario AND Estado = 'Activo'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Usuario", usuario);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int usuarioID = reader.GetInt32(0);
                                byte[] hashedPasswordFromDB = (byte[])reader["PasswordHash"];
                                int roleID = reader.GetInt32(2);
                                byte[] hashedInputPassword = HashPassword(contrasena);

                                if (StructuralComparisons.StructuralEqualityComparer.Equals(hashedPasswordFromDB, hashedInputPassword))
                                {
                                    return (usuarioID, roleID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al autenticar usuario: " + ex.Message);
            }

            return (-1, -1);
        }

        private byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
