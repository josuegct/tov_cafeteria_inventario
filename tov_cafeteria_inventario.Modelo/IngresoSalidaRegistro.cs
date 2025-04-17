using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tov_cafeteria_inventario.Modelo
{
    public class IngresoSalidaRegistro
    {
        public int BitacoraID { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Accion { get; set; }
    }
}