using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tov_cafeteria_inventario.Modelo
{
    public class Inventario
    {
        public int MovimientoID { get; set; }
        public int ProductoID { get; set; }
        public string Producto { get; set; }
        public int ProveedorID { get; set; }
        public string Proveedor { get; set; }
        public string UnidadMedida { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }

}