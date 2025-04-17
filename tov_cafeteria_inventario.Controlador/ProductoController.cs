using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class ProductoController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<Producto> ObtenerProductosPorProveedor(int proveedorID)
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ProductoID, Nombre FROM Productos WHERE ProveedorID = @ProveedorID ORDER BY Nombre";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
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

        public void AgregarProducto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Productos (Nombre, Categoria, UnidadMedida, CantidadDisponible, PrecioUnitario, ProveedorID) " +
                               "VALUES (@Nombre, @Categoria, @UnidadMedida, @CantidadDisponible, @PrecioUnitario, @ProveedorID)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@UnidadMedida", producto.UnidadMedida);
                    cmd.Parameters.AddWithValue("@CantidadDisponible", producto.CantidadDisponible);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@ProveedorID", producto.ProveedorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProductosPorProveedor(int proveedorID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Productos WHERE ProveedorID = @ProveedorID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}