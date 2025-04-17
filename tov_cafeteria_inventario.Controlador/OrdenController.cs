using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class OrdenController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<Orden> ObtenerOrdenes()
        {
            List<Orden> ordenes = new List<Orden>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT o.OrdenID, o.FechaOrden, o.Estado, o.UsuarioID,
                   u.Usuario AS NombreUsuario,
                   p.ProductoID, p.Nombre AS NombreProducto,
                   pr.ProveedorID, pr.Nombre AS NombreProveedor,
                   ISNULL(od.Precio, 0) AS PrecioUnitario,
                   ISNULL(od.Cantidad * od.Precio, 0) AS PrecioTotal
            FROM Ordenes o
            INNER JOIN Usuarios u ON o.UsuarioID = u.UsuarioID
            INNER JOIN Productos p ON o.ProductoID = p.ProductoID
            INNER JOIN Proveedores pr ON o.ProveedorID = pr.ProveedorID
            LEFT JOIN OrdenDetalles od ON o.OrdenID = od.OrdenID AND od.ProductoID = p.ProductoID";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordenes.Add(new Orden
                        {
                            OrdenID = reader.GetInt32(0),
                            FechaOrden = reader.GetDateTime(1),
                            Estado = reader.GetString(2),
                            UsuarioID = reader.GetInt32(3),
                            ProductoID = reader.GetInt32(5),
                            NombreProducto = reader.GetString(6),
                            ProveedorID = reader.GetInt32(7),
                            NombreProveedor = reader.GetString(8),
                            PrecioUnitario = reader.GetDecimal(9),
                            PrecioTotal = reader.GetDecimal(10)
                        });
                    }
                }
            }

            return ordenes;
        }

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProveedorID, Nombre FROM Proveedores";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proveedores.Add(new Proveedor
                        {
                            ProveedorID = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }
                }
            }

            return proveedores;
        }

        public List<Producto> ObtenerProductosPorProveedor(int proveedorID)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID, Nombre FROM Productos WHERE ProveedorID = @ProveedorID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProveedorID", proveedorID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                ProductoID = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return productos;
        }

        public void AgregarOrden(Orden orden)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Ordenes (FechaOrden, Estado, UsuarioID, ProductoID, ProveedorID) VALUES (@FechaOrden, @Estado, @UsuarioID, @ProductoID, @ProveedorID)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaOrden", orden.FechaOrden);
                    command.Parameters.AddWithValue("@Estado", orden.Estado);
                    command.Parameters.AddWithValue("@UsuarioID", orden.UsuarioID);
                    command.Parameters.AddWithValue("@ProductoID", orden.ProductoID);
                    command.Parameters.AddWithValue("@ProveedorID", orden.ProveedorID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ModificarOrden(Orden orden)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Ordenes SET FechaOrden = @FechaOrden, Estado = @Estado, UsuarioID = @UsuarioID, ProductoID = @ProductoID, ProveedorID = @ProveedorID WHERE OrdenID = @OrdenID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaOrden", orden.FechaOrden);
                    command.Parameters.AddWithValue("@Estado", orden.Estado);
                    command.Parameters.AddWithValue("@UsuarioID", orden.UsuarioID);
                    command.Parameters.AddWithValue("@ProductoID", orden.ProductoID);
                    command.Parameters.AddWithValue("@ProveedorID", orden.ProveedorID);
                    command.Parameters.AddWithValue("@OrdenID", orden.OrdenID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarOrden(int ordenID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string eliminarDetalles = "DELETE FROM OrdenDetalles WHERE OrdenID = @OrdenID";
                using (SqlCommand cmdDetalles = new SqlCommand(eliminarDetalles, conn))
                {
                    cmdDetalles.Parameters.AddWithValue("@OrdenID", ordenID);
                    cmdDetalles.ExecuteNonQuery();
                }

                string eliminarOrden = "DELETE FROM Ordenes WHERE OrdenID = @OrdenID";
                using (SqlCommand cmdOrden = new SqlCommand(eliminarOrden, conn))
                {
                    cmdOrden.Parameters.AddWithValue("@OrdenID", ordenID);
                    cmdOrden.ExecuteNonQuery();
                }
            }
        }
    }
}