using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Modelo
{
    public static class ConexionBD
    {
        public static string ObtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["CafeteriaDB"].ConnectionString;
        }
    }
}
