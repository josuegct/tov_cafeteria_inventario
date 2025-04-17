using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace tov_cafeteria_inventario.Vista
{
    public static class ConexionBD
    {
        public static string ObtenerCadena()
        {
            var config = ConfigurationManager.ConnectionStrings["CafeteriaDB"];
            if (config == null)
                throw new Exception("⚠️ No se encontró la cadena de conexión 'CafeteriaDB' en App.config.");
            return config.ConnectionString;
        }
    }
}
