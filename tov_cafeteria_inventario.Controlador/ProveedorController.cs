﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class ProveedorController
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Proveedores";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proveedores.Add(new Proveedor
                        {
                            ProveedorID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Cedula = reader.GetString(2),
                            Telefono = reader.GetString(3),
                            Correo = reader.GetString(4),
                            Direccion = reader.GetString(5)
                        });
                    }
                }
            }
            return proveedores;
        }

        public void AgregarProveedorConProductos(Proveedor proveedor, List<string> productos)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Proveedores (Nombre, Cedula, Telefono, Correo, Direccion) OUTPUT INSERTED.ProveedorID VALUES (@Nombre, @Cedula, @Telefono, @Correo, @Direccion)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Cedula", proveedor.Cedula);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@Correo", proveedor.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    proveedor.ProveedorID = (int)cmd.ExecuteScalar();
                }

                foreach (var nombreProducto in productos)
                {
                    string insertProducto = "INSERT INTO Productos (Nombre, Categoria, CantidadDisponible, UnidadMedida, PrecioUnitario, ProveedorID) VALUES (@Nombre, 'Sin categoría', 0, 'N/A', 0, @ProveedorID)";
                    using (SqlCommand prodCmd = new SqlCommand(insertProducto, conn))
                    {
                        prodCmd.Parameters.AddWithValue("@Nombre", nombreProducto.Trim());
                        prodCmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                        prodCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ModificarProveedorConProductos(Proveedor proveedor, List<string> productos)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Proveedores SET Nombre = @Nombre, Cedula = @Cedula, Telefono = @Telefono, Correo = @Correo, Direccion = @Direccion WHERE ProveedorID = @ProveedorID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Cedula", proveedor.Cedula);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@Correo", proveedor.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", proveedor.Direccion);
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                    cmd.ExecuteNonQuery();
                }

                string deleteOld = "DELETE FROM Productos WHERE ProveedorID = @ProveedorID";
                using (SqlCommand delCmd = new SqlCommand(deleteOld, conn))
                {
                    delCmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                    delCmd.ExecuteNonQuery();
                }

                foreach (var nombreProducto in productos)
                {
                    string insertProducto = "INSERT INTO Productos (Nombre, Categoria, CantidadDisponible, UnidadMedida, PrecioUnitario, ProveedorID) VALUES (@Nombre, 'Sin categoría', 0, 'N/A', 0, @ProveedorID)";
                    using (SqlCommand prodCmd = new SqlCommand(insertProducto, conn))
                    {
                        prodCmd.Parameters.AddWithValue("@Nombre", nombreProducto.Trim());
                        prodCmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                        prodCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void EliminarProveedor(int proveedorID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string deleteProductos = "DELETE FROM Productos WHERE ProveedorID = @ProveedorID";
                using (SqlCommand cmdProd = new SqlCommand(deleteProductos, conn))
                {
                    cmdProd.Parameters.AddWithValue("@ProveedorID", proveedorID);
                    cmdProd.ExecuteNonQuery();
                }

                string deleteProveedor = "DELETE FROM Proveedores WHERE ProveedorID = @ProveedorID";
                using (SqlCommand cmdProv = new SqlCommand(deleteProveedor, conn))
                {
                    cmdProv.Parameters.AddWithValue("@ProveedorID", proveedorID);
                    cmdProv.ExecuteNonQuery();
                }
            }
        }
    }
}
