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
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CafeteriaDB"].ConnectionString;

        public DataTable ObtenerMovimientosIndividuales()
        {
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    i.MovimientoID,
                    i.ProductoID,
                    ISNULL(p.Nombre, 'N/A') AS NombreProducto,
                    ISNULL(pr.ProveedorID, 0) AS ProveedorID,
                    ISNULL(i.UnidadMedida, 'N/A') AS UnidadMedida,
                    i.TipoMovimiento,
                    i.Cantidad,
                    i.PrecioUnitario,
                    i.PrecioTotal,
                    i.FechaMovimiento
                FROM Inventario i
                LEFT JOIN Productos p ON i.ProductoID = p.ProductoID
                LEFT JOIN Proveedores pr ON p.ProveedorID = pr.ProveedorID
                ORDER BY i.FechaMovimiento DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    conn.Open();
                    adapter.Fill(table);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar movimientos desde InventarioController: " + ex.Message, ex);
            }
            return table;
        }

        public List<Producto> ObtenerProductos()
        {
            var lista = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProductoID, Nombre FROM Productos ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        ProductoID = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }

            return lista;
        }

        public List<Proveedor> ObtenerProveedores()
        {
            var lista = new List<Proveedor>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProveedorID, Nombre FROM Proveedores ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Proveedor
                    {
                        ProveedorID = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }

            return lista;
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
                        'N/A' AS Usuario,
                        p.UnidadMedida
                    FROM Productos p
                    LEFT JOIN Inventario i ON p.ProductoID = i.ProductoID
                    GROUP BY p.ProductoID, p.Nombre, p.UnidadMedida
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
                            Usuario = reader.GetString(4),
                            UnidadMedida = reader.GetString(5)
                        });
                    }
                }
            }

            return lista;
        }
        public List<Inventario> ObtenerStockFiltrado(int? productoID, int? proveedorID, DateTime desde, DateTime hasta)
        {
            var lista = new List<Inventario>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
        SELECT 
            i.MovimientoID,
            i.ProductoID,
            p.Nombre AS Producto,
            pr.ProveedorID,
            pr.Nombre AS Proveedor,
            i.UnidadMedida,
            i.TipoMovimiento,
            i.Cantidad,
            i.PrecioUnitario,
            i.PrecioTotal,
            i.FechaMovimiento,
            u.Nombre AS Usuario
        FROM Inventario i
        LEFT JOIN Productos p ON i.ProductoID = p.ProductoID
        LEFT JOIN Proveedores pr ON p.ProveedorID = pr.ProveedorID
        LEFT JOIN Usuarios u ON i.UsuarioID = u.UsuarioID
        WHERE i.FechaMovimiento BETWEEN @Desde AND @Hasta";

                if (productoID.HasValue)
                    query += " AND p.ProductoID = @ProductoID";
                if (proveedorID.HasValue)
                    query += " AND pr.ProveedorID = @ProveedorID";

                query += " ORDER BY i.FechaMovimiento DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Desde", desde);
                    cmd.Parameters.AddWithValue("@Hasta", hasta);

                    if (productoID.HasValue)
                        cmd.Parameters.AddWithValue("@ProductoID", productoID.Value);
                    if (proveedorID.HasValue)
                        cmd.Parameters.AddWithValue("@ProveedorID", proveedorID.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Inventario
                            {
                                MovimientoID = reader.GetInt32(0),
                                ProductoID = reader.GetInt32(1),
                                Producto = reader.GetString(2),
                                ProveedorID = reader.GetInt32(3),
                                Proveedor = reader.GetString(4),
                                UnidadMedida = reader.GetString(5),
                                TipoMovimiento = reader.GetString(6),
                                Cantidad = reader.GetInt32(7),
                                PrecioUnitario = reader.GetDecimal(8),
                                PrecioTotal = reader.GetDecimal(9),
                                Fecha = reader.GetDateTime(10),
                                Usuario = reader.GetString(11)
                            });
                        }
                    }
                }
            }

            return lista;
        }


    }
}