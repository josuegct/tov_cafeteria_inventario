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
                        Estado = reader[5].ToString()

                    });
                }

                conn.Close();
            }

            return lista;
        }

        public List<UsuarioReporte> ObtenerUsuariosFiltrados(string usuario, string rol, DateTime desde, DateTime hasta)
        {
            var lista = new List<UsuarioReporte>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT u.UsuarioID, u.Nombre, u.Usuario, u.Correo, r.NombreRol, u.Estado, u.FechaRegistro
            FROM Usuarios u
            JOIN Roles r ON u.RoleID = r.RoleID
            WHERE (@Usuario = '' OR u.Nombre LIKE '%' + @Usuario + '%')
              AND (@Rol = '' OR r.NombreRol = @Rol)
              AND u.FechaRegistro BETWEEN @Desde AND @Hasta";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Rol", rol);
                cmd.Parameters.AddWithValue("@Desde", desde);
                cmd.Parameters.AddWithValue("@Hasta", hasta);

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
                        Estado = reader[5].ToString(),
                        FechaRegistro = reader.GetDateTime(6)
                    });
                }
            }

            return lista;
        }
        public List<Rol> ObtenerRoles()
        {
            var lista = new List<Rol>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT RoleID, NombreRol, Descripcion FROM Roles";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Rol
                    {
                        RoleID = reader.GetInt32(0),
                        NombreRol = reader.GetString(1),
                        Descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2)
                    });
                }
            }

            return lista;
        }

    }
}
