﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tov_cafeteria_inventario.Modelo
{
    public class UsuarioReporte
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
    }
}