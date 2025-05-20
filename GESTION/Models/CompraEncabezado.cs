using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class CompraEncabezado
    {
        public long Id { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public long ProveedorId { get; set; }
        public string ProveedorNombre { get; set; }

    }
}