
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class Usuarios
    {
        public long Id { get; set; }
        public int Rol_Id { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Pw { get; set; } // Almacenado como Base64
        public DateTime Creado_El { get; set; }
    }
}


