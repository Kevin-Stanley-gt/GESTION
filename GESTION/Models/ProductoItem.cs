using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class ProductoItem
    {
        public long Id { get; set; }
        
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Codigo_Barras { get; set; }
        public decimal Precio { get; set; }
    }
}