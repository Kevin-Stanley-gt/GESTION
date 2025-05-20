using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class CompraDetalle
    {
        public long Id { get; set; }
        public long Compra_Id { get; set; }
        public long Producto_Id { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string Comentario { get; set; }
    }
}