using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GESTION.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public int Tipo_Id { get; set; }
        public string Tipo { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]

        public string Nombre { get; set; }
        // Estas tres pueden quedar en blanco:
        //[Required(AllowEmptyStrings = true)]
        public string Nit { get; set; }
        //[Required(AllowEmptyStrings = true)]
        public string Direccion { get; set; }
        //[Required(AllowEmptyStrings = true)]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }
        public DateTime Creado_El { get; set; }
        public bool Activo { get; set; }
    }
}