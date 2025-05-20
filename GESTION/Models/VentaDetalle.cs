using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int Venta_Id { get; set; }
        public int Producto_Id { get; set; }
        public string Producto_Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }

    }
}