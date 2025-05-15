using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class Proveedores
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Razon_Social { get; set; }
        public int Moneda_Id { get; set; }
        public string Moneda { get; set; }
        public DateTime Creado_El { get; set; }
    }
}