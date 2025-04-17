using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class InventarioController
    {
        private string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";
        /*using tov_cafeteria_inventario.Vista;
         string connectionString = ConexionBD.ObtenerCadena();*/
        public DataTable ObtenerMovimientosIndividuales()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    i.MovimientoID,
                    p.Nombre AS NombreProducto,
                    i.TipoMovimiento,
                    i.Cantidad,
                    i.PrecioUnitario,
                    i.PrecioTotal,
                    i.FechaMovimiento
                FROM Inventario i
                INNER JOIN Productos p ON i.ProductoID = p.ProductoID
                ORDER BY i.FechaMovimiento DESC;";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public bool EliminarMovimiento(int movimientoID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Inventario WHERE MovimientoID = @MovimientoID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MovimientoID", movimientoID);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public List<Inventario> ObtenerStockActual()
        {
            var lista = new List<Inventario>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        p.ProductoID,
                        p.Nombre AS Producto,
                        SUM(CASE 
                                WHEN i.TipoMovimiento = 'Ingreso' THEN i.Cantidad 
                                WHEN i.TipoMovimiento = 'Salida' THEN -i.Cantidad 
                                ELSE 0 
                            END) AS Cantidad,
                        MAX(i.FechaMovimiento) AS Fecha,
                        'N/A' AS Usuario
                    FROM Productos p
                    LEFT JOIN Inventario i ON p.ProductoID = i.ProductoID
                    GROUP BY p.ProductoID, p.Nombre
                    HAVING SUM(CASE 
                                WHEN i.TipoMovimiento = 'Ingreso' THEN i.Cantidad 
                                WHEN i.TipoMovimiento = 'Salida' THEN -i.Cantidad 
                                ELSE 0 
                            END) > 0
                    ORDER BY p.Nombre;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inventario
                        {
                            ProductoID = reader.GetInt32(0),
                            Producto = reader.GetString(1),
                            Cantidad = reader.GetInt32(2),
                            Fecha = reader.GetDateTime(3),
                            Usuario = reader.GetString(4)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
