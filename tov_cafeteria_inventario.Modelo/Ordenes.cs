using System;

namespace tov_cafeteria_inventario.Modelo
{
    public class Orden
    {
        public int OrdenID { get; set; }
        public DateTime FechaOrden { get; set; }
        public string Estado { get; set; }
        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public int ProveedorID { get; set; }
        public string NombreProducto { get; set; }
        public string NombreProveedor { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
