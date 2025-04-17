using System;
using System.Configuration;
using System.Data.SqlClient;

namespace tov_cafeteria_inventario.Controlador
{
    public class PantallaPrincipalController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public int ObtenerRoleID(int usuarioID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT RoleID FROM Usuarios WHERE UsuarioID = @UsuarioID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }
    }
}