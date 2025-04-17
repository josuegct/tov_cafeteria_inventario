using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class RolController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<Rol> ObtenerRoles()
        {
            List<Rol> roles = new List<Rol>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT RoleID, NombreRol, Descripcion FROM Roles";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            RoleID = reader.GetInt32(0),
                            NombreRol = reader.GetString(1),
                            Descripcion = reader.GetString(2)
                        });
                    }
                }
            }
            return roles;
        }

        public void AgregarRol(Rol rol)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Roles (NombreRol, Descripcion) VALUES (@NombreRol, @Descripcion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreRol", rol.NombreRol);
                    command.Parameters.AddWithValue("@Descripcion", rol.Descripcion);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ModificarRol(Rol rol)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Roles SET NombreRol = @NombreRol, Descripcion = @Descripcion WHERE RoleID = @RoleID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreRol", rol.NombreRol);
                    command.Parameters.AddWithValue("@Descripcion", rol.Descripcion);
                    command.Parameters.AddWithValue("@RoleID", rol.RoleID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarRol(int roleID)
        {
            if (roleID == 1 || roleID == 2)
                throw new InvalidOperationException("Este rol es esencial para el sistema y no puede eliminarse.");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Roles WHERE RoleID = @RoleID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
