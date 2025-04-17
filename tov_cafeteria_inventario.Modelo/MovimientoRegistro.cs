using System;

namespace tov_cafeteria_inventario.Modelo
{
    public class MovimientoRegistro
    {
        public int BitacoraID { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Accion { get; set; }
    }
}
