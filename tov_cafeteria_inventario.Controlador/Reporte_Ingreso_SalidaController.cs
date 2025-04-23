using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_Ingreso_SalidaController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<IngresoSalidaRegistro> ObtenerIngresosYSalidasFiltrado(int? usuarioID, DateTime fechaInicio, DateTime fechaFin)
        {
            List<IngresoSalidaRegistro> lista = new List<IngresoSalidaRegistro>();

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
                WHERE 
                    (b.Accion LIKE '%Inicio de sesión%' OR b.Accion LIKE '%cerró sesión%')
                    AND b.FechaRegistro BETWEEN @Inicio AND @Fin";

                    if (usuarioID.HasValue)
                        query += " AND b.UsuarioID = @UsuarioID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Inicio", fechaInicio);
                        command.Parameters.AddWithValue("@Fin", fechaFin);

                        if (usuarioID.HasValue)
                            command.Parameters.AddWithValue("@UsuarioID", usuarioID.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new IngresoSalidaRegistro
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar ingresos y salidas: " + ex.Message);
            }

            return lista;
        }

        public List<IngresoSalidaRegistro> ObtenerIngresosYSalidas()
        {
            List<IngresoSalidaRegistro> lista = new List<IngresoSalidaRegistro>();

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
                        WHERE b.Accion LIKE '%Inicio de sesión%' OR b.Accion LIKE '%cerró sesión%'
                        ORDER BY b.FechaRegistro DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new IngresoSalidaRegistro
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
                throw new Exception("Error al obtener ingresos y salidas: " + ex.Message);
            }

            return lista;
        }
    }
}
