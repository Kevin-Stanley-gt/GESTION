using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public int Proveedor_Id { get; set; }
        public string Proveedor { get; set; }
        public int Marca_Id { get; set; }
        public string Marca { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public bool Activo { get; set; }
        public string Barras { get; set; }

    }
}