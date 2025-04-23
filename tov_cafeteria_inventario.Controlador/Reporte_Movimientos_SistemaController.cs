using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;
using System.Data;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_Movimientos_SistemaController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public DataTable ObtenerUsuarios()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UsuarioID, Nombre FROM Usuarios ORDER BY Nombre";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            // Insertar opción "Todos" al principio (opcional)
            DataRow fila = table.NewRow();
            fila["UsuarioID"] = DBNull.Value;
            fila["Nombre"] = "Todos";
            table.Rows.InsertAt(fila, 0);

            return table;
        }


        public List<MovimientoRegistro> ObtenerMovimientos(int? usuarioID = null, DateTime? desde = null, DateTime? hasta = null)
        {
            List<MovimientoRegistro> lista = new List<MovimientoRegistro>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT b.BitacoraID, u.Nombre AS NombreUsuario, b.FechaRegistro, b.Accion
                FROM Bitacoras b
                INNER JOIN Usuarios u ON b.UsuarioID = u.UsuarioID
                WHERE (@UsuarioID IS NULL OR b.UsuarioID = @UsuarioID)
                  AND (@Desde IS NULL OR b.FechaRegistro >= @Desde)
                  AND (@Hasta IS NULL OR b.FechaRegistro <= @Hasta)
                ORDER BY b.FechaRegistro DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioID", (object)usuarioID ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Desde", (object)desde ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Hasta", (object)hasta ?? DBNull.Value);

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
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los movimientos: " + ex.Message);
            }

            return lista;
        }

    }
}