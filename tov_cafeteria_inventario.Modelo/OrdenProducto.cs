using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tov_cafeteria_inventario.Modelo
{
    public class OrdenProducto
    {
        public int OrdenID { get; set; }
        public string Producto { get; set; }
        public string Proveedor { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }
        public DateTime FechaOrden { get; set; }
    }
}
