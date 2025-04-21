using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Controlador
{
    public class ExistenciasController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CafeteriaDB"].ConnectionString;

        public List<Existencias> ObtenerExistenciasActuales()
        {
            var lista = new List<Existencias>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
        SELECT 
            p.ProductoID,
            p.Nombre AS Producto,
            MAX(i.UnidadMedida) AS UnidadMedida,
            SUM(
                CASE 
                    WHEN i.TipoMovimiento = 'Ingreso' THEN i.Cantidad
                    WHEN i.TipoMovimiento IN ('Salida', 'Devolucion') THEN -i.Cantidad
                    WHEN i.TipoMovimiento = 'Ajuste' THEN i.Cantidad
                    ELSE 0
                END
            ) AS Cantidad,
            MAX(i.FechaMovimiento) AS Fecha
        FROM Productos p
        LEFT JOIN Inventario i ON p.ProductoID = i.ProductoID
        GROUP BY p.ProductoID, p.Nombre
        HAVING 
            SUM(
                CASE 
                    WHEN i.TipoMovimiento = 'Ingreso' THEN i.Cantidad
                    WHEN i.TipoMovimiento IN ('Salida', 'Devolucion') THEN -i.Cantidad
                    WHEN i.TipoMovimiento = 'Ajuste' THEN i.Cantidad
                    ELSE 0
                END
            ) > 0
        ORDER BY p.Nombre;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Existencias
                        {
                            ProductoID = reader.GetInt32(0),
                            Producto = reader.GetString(1),
                            UnidadMedida = reader.IsDBNull(2) ? "N/A" : reader.GetString(2),
                            Cantidad = reader.GetInt32(3),
                            Fecha = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return lista;
        }

    }
}
