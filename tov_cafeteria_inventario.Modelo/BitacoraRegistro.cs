using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tov_cafeteria_inventario.Modelo
{
    public class BitacoraRegistro
    {
        public int BitacoraID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Accion { get; set; }
    }
}
