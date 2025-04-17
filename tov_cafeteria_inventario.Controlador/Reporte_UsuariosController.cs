using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_UsuariosController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<UsuarioReporte> ObtenerUsuarios()
        {
            var lista = new List<UsuarioReporte>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT u.UsuarioID, u.Nombre, u.Usuario, u.Correo, r.NombreRol, u.Estado 
                                 FROM Usuarios u 
                                 JOIN Roles r ON u.RoleID = r.RoleID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new UsuarioReporte
                    {
                        UsuarioID = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Usuario = reader.GetString(2),
                        Correo = reader.GetString(3),
                        Rol = reader.GetString(4),
                        Estado = reader.GetBoolean(5) ? "Activo" : "Inactivo"
                    });
                }

                conn.Close();
            }

            return lista;
        }
    }
}
