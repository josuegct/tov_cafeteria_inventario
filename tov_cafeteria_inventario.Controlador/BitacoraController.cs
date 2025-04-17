using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class BitacoraController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<BitacoraRegistro> ObtenerBitacora(string filtro = "")
        {
            List<BitacoraRegistro> registros = new List<BitacoraRegistro>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT BitacoraID, UsuarioID, FechaRegistro, Accion FROM Bitacoras";

                    if (filtro == "IngresosSalidas")
                    {
                        query += " WHERE Accion LIKE '%Inicio de sesión%' OR Accion LIKE '%cerró sesión%'";
                    }
                    else if (filtro == "MovimientosSistema")
                    {
                        query += " WHERE Accion NOT LIKE '%Inicio de sesión%' AND Accion NOT LIKE '%cerró sesión%'";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            registros.Add(new BitacoraRegistro
                            {
                                BitacoraID = reader.GetInt32(0),
                                UsuarioID = reader.GetInt32(1),
                                FechaRegistro = reader.GetDateTime(2),
                                Accion = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los registros de bitácora: " + ex.Message);
            }

            return registros;
        }

        public void RegistrarAccion(int usuarioID, string accion)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Bitacoras (UsuarioID, FechaRegistro, Accion) VALUES (@UsuarioID, GETDATE(), @Accion)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioID", usuarioID);
                        command.Parameters.AddWithValue("@Accion", accion);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar en la bitácora: " + ex.Message);
            }
        }

    }
}
