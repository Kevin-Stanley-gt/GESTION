using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
        public int Rol_Id { get; set; } // Rol del usuario (ejemplo: "admin", "user", etc.)
    }
}