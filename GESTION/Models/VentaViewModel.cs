using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GESTION.Models
{
    public class VentaViewModel
    {
        public VentaEncabezdo Encabezado { get; set; }
        public List<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();

        public IEnumerable<SelectListItem> Productos { get; set; }
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public List<ProductoItem> ProductosLista { get; set; }

    }
}