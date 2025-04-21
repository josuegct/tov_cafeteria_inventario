using System;

namespace tov_cafeteria_inventario.Modelo
{
    public class Existencias
    {
        public int ProductoID { get; set; }
        public string Producto { get; set; }
        public string UnidadMedida { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}