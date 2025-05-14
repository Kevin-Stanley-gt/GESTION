using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GESTION.Clases
{
    public class Hashing
    {
        // Método para generar el hash de la contraseña
        public static string HashPassword(string password)
        {
            // Creamos el objeto SHA256
            using (var sha256 = SHA256.Create())
            {
                // Convertimos la contraseña a un arreglo de bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Calculamos el hash de la contraseña
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convertimos el hash a una cadena Base64 para almacenarlo en la base de datos
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Método para verificar si una contraseña coincide con un hash almacenado
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            // Calculamos el hash de la contraseña proporcionada por el usuario
            string hashOfInput = HashPassword(password);

            // Comparamos el hash generado con el hash almacenado
            return hashedPassword == hashOfInput;
        }
    }
}
