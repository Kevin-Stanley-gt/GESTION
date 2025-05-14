using GESTION.Models;
using System.Web.Mvc;
using GESTION.Clases; // Clase de encriptación (Hashing)
using System.Data.SqlClient;
using System;

namespace POS_FARMATODO.Controllers
{
    public class LoginController : Controller
    {
        private conexion conexion = new conexion(); // Conexión a la base de datos

        // Vista de Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login (Validación de usuario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var datosUsuario = ObtenerUsuario(usuario);

                if (datosUsuario != null)
                {
                    // Guardar en sesión
                    Session["Id"] = datosUsuario.Id;
                    Session["Usuario"] = datosUsuario.NombreUsuario;
                    Session["Rol"] = datosUsuario.Rol_Id;

                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña incorrectos.";
                }
            }

            return View(usuario);
        }

        // Método para obtener los datos del usuario si las credenciales son válidas
        private Usuario ObtenerUsuario(Usuario usuario)
        {
            Usuario resultado = null;
            string hashContrasena = Hashing.HashPassword(usuario.Contrasena); 

            using (var conn = conexion.AbrirConexion())
            {
                string query = "SELECT Id, Usuario, Rol_Id FROM Usuario WHERE UPPER(Usuario) = @usuario AND Pw = @clave";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario.NombreUsuario.ToUpper());
                    cmd.Parameters.AddWithValue("@clave", Convert.FromBase64String(hashContrasena)); // Convertir Base64 a byte[]
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultado = new Usuario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NombreUsuario = reader["Usuario"].ToString(),
                                Rol_Id =Convert.ToInt32(reader["Rol_Id"])
                            };
                        }
                    }
                }
            }

            return resultado;
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
