using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GESTION.Models
{
    public class CompraViewModel
    {
      
            public CompraEncabezado Encabezado { get; set; }
            public List<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();

            public IEnumerable<SelectListItem> Proveedores { get; set; }
            public IEnumerable<SelectListItem> Productos { get; set; }
            public List<ProductoItem> ProductosLista { get; set; }

    }

}