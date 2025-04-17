using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_Movimientos_SistemaController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<MovimientoRegistro> ObtenerMovimientos()
        {
            List<MovimientoRegistro> lista = new List<MovimientoRegistro>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            b.BitacoraID,
                            u.Nombre AS NombreUsuario,
                            b.FechaRegistro,
                            b.Accion
                        FROM Bitacoras b
                        INNER JOIN Usuarios u ON b.UsuarioID = u.UsuarioID
                        ORDER BY b.FechaRegistro DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MovimientoRegistro
                            {
                                BitacoraID = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                FechaRegistro = reader.GetDateTime(2),
                                Accion = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los movimientos: " + ex.Message);
            }

            return lista;
        }
    }
}