using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class Reporte_Ordenes_ProductosController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<OrdenProducto> ObtenerOrdenes()
        {
            List<OrdenProducto> ordenes = new List<OrdenProducto>();

            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las órdenes: " + ex.Message);
            }

            return ordenes;
        }
    }
}
