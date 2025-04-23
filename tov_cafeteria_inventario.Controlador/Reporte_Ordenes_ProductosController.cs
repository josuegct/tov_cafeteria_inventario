using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_Ordenes_ProductosController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CafeteriaDB"].ConnectionString;

        public List<OrdenProducto> ObtenerOrdenes()
        {
            List<OrdenProducto> ordenes = new List<OrdenProducto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        o.OrdenID,
                        p.Nombre AS Producto,
                        pr.Nombre AS Proveedor,
                        u.Nombre AS Usuario,
                        o.Estado,
                        o.FechaOrden
                    FROM Ordenes o
                    INNER JOIN Productos p ON o.ProductoID = p.ProductoID
                    INNER JOIN Proveedores pr ON o.ProveedorID = pr.ProveedorID
                    INNER JOIN Usuarios u ON o.UsuarioID = u.UsuarioID
                    ORDER BY o.FechaOrden DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordenes.Add(new OrdenProducto
                        {
                            OrdenID = reader.GetInt32(0),
                            Producto = reader.GetString(1),
                            Proveedor = reader.GetString(2),
                            Usuario = reader.GetString(3),
                            Estado = reader.GetString(4),
                            FechaOrden = reader.GetDateTime(5)
                        });
                    }
                }
            }

            return ordenes;
        }




        public List<Producto> ObtenerProductos()
        {
            var lista = new List<Producto>();

            lista.Add(new Producto { ProductoID = 0, Nombre = "-- Seleccione --" });

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProductoID, Nombre FROM Productos";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            ProductoID = reader.GetInt32(0),
                            Nombre = LimpiarTexto(reader.GetString(1))
                        });
                    }
                }
            }

            return lista;
        }

        private string LimpiarTexto(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? "" :
                new string(texto.Where(c => !char.IsControl(c)).ToArray()).Trim();
        }




        public List<OrdenProducto> ObtenerOrdenesFiltradas(int? productoID, int? proveedorID, DateTime desde, DateTime hasta)
        {
            var lista = new List<OrdenProducto>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
        SELECT o.OrdenID, p.Nombre AS Producto, pr.Nombre AS Proveedor, u.Nombre AS Usuario, 
               o.Estado, o.FechaOrden
        FROM Ordenes o
        INNER JOIN Productos p ON o.ProductoID = p.ProductoID
        INNER JOIN Proveedores pr ON o.ProveedorID = pr.ProveedorID
        INNER JOIN Usuarios u ON o.UsuarioID = u.UsuarioID
        WHERE o.FechaOrden BETWEEN @Desde AND @Hasta";

                if (productoID.HasValue)
                    query += " AND o.ProductoID = @ProductoID";

                if (proveedorID.HasValue)
                    query += " AND o.ProveedorID = @ProveedorID";

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
                            lista.Add(new OrdenProducto
                            {
                                OrdenID = reader.GetInt32(0),
                                Producto = reader.GetString(1),
                                Proveedor = reader.GetString(2),
                                Usuario = reader.GetString(3),
                                Estado = reader.GetString(4),
                                FechaOrden = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<Proveedor> ObtenerProveedores()
        {
            var lista = new List<Proveedor>();

            // Opción visual inicial
            lista.Add(new Proveedor { ProveedorID = 0, Nombre = "-- Seleccione --" });

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProveedorID, RTRIM(LTRIM(Nombre)) AS Nombre FROM Proveedores";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Proveedor
                        {
                            ProveedorID = reader.GetInt32(0),
                            Nombre = reader.GetString(1).Trim()
                        });
                    }
                }
            }

            return lista;
        }

    }
}
