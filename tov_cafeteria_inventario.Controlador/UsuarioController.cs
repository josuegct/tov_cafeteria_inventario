using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class UsuarioController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<UsuarioSistema> ObtenerUsuarios()
        {
            var usuarios = new List<UsuarioSistema>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT u.UsuarioID, u.Nombre, u.Usuario, u.Correo, r.NombreRol, u.RoleID, u.Estado FROM Usuarios u JOIN Roles r ON u.RoleID = r.RoleID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(new UsuarioSistema
                    {
                        UsuarioID = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Usuario = reader.GetString(2),
                        Correo = reader.GetString(3),
                        RolNombre = reader.GetString(4),
                        RoleID = reader.GetInt32(5),
                        Estado = reader.GetBoolean(6)
                    });
                }

                conn.Close();
            }

            return usuarios;
        }

        public List<Rol> ObtenerRoles()
        {
            var roles = new List<Rol>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT RoleID, NombreRol FROM Roles";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(new Rol
                    {
                        RoleID = reader.GetInt32(0),
                        NombreRol = reader.GetString(1),
                        Descripcion = ""
                    });
                }

                conn.Close();
            }

            return roles;
        }

        public void AgregarUsuario(UsuarioSistema usuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception("El nombre de usuario ya está en uso. Elija uno diferente.");
                    }
                }

                string insertQuery = "INSERT INTO Usuarios (Nombre, Usuario, Correo, RoleID, Estado, PasswordHash) VALUES (@Nombre, @Usuario, @Correo, @RoleID, @Estado, @PasswordHash)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                    cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@RoleID", usuario.RoleID);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ModificarUsuario(UsuarioSistema usuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Usuarios SET Nombre = @Nombre, Usuario = @Usuario, Correo = @Correo, RoleID = @RoleID, Estado = @Estado, PasswordHash = @PasswordHash WHERE UsuarioID = @UsuarioID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@RoleID", usuario.RoleID);
                cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void EliminarUsuario(int usuarioID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Ordenes WHERE UsuarioID = @UsuarioID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new ApplicationException("No se puede eliminar este usuario porque tiene órdenes asociadas. Elimine o reasigne esas órdenes primero.");
                    }
                }

                string deleteQuery = "DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    deleteCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        public byte[] ObtenerPasswordActual(int usuarioID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT PasswordHash FROM Usuarios WHERE UsuarioID = @UsuarioID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                conn.Open();
                return (byte[])cmd.ExecuteScalar();
            }
        }

        public byte[] EncriptarContraseña(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
            }
        }
    }
}
