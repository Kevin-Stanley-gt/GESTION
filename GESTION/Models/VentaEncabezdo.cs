using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class VentaEncabezdo
    {
        public int Id { get; set; }
        public int Jornada_Id { get; set; }
        public int Cliente_Id { get; set; }
        public string Cliente_Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo_Total { get; set; }
        public DateTime Creado_El { get; set; }

    }
}