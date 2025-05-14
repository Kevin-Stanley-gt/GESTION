using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class UsuarioController : Controller
    {
        private conexion conexion = new conexion();

        // GET: Usuario
        public ActionResult Usuarios()
        {
            List<Usuarios> lista = new List<Usuarios>();

            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand(@"SELECT a.Id, a.Rol_Id, b.Nombres AS Rol, a.Nombre, a.Apellidos, a.Usuario, 
                                                  a.Pw, a.Creado_El
                                                  FROM Usuario a
                                                  INNER JOIN Roles b ON b.Id = a.Rol_Id", conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Usuarios
                    {
                        Id = Convert.ToInt64(dr["Id"]),
                        Rol_Id = Convert.ToInt32(dr["Rol_Id"]),
                        Rol = dr["Rol"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        Usuario = dr["Usuario"].ToString(),
                        Pw = dr["Pw"].ToString(),
                        Creado_El = Convert.ToDateTime(dr["Creado_El"])
                    });
                }
            }

            return View(lista);
        }

        // GET: Usuario/Create
        public ActionResult Crear()
        {
            ViewBag.Roles = ObtenerRoles();
            return View();
        }

        // POST: Usuario/Crear
        [HttpPost]
        public ActionResult Crear(Usuarios u)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    

                    SqlCommand cmd = new SqlCommand("INSERT INTO Usuario (Rol_Id, Nombre, Apellidos, Usuario, Pw, Activo) VALUES (@Rol_Id, @Nombre, @Apellidos, @Usuario, @Pw, 1)", conn);

                    string passwordCifrada = Hashing.HashPassword(u.Pw);
                    byte[] pwBytes = Convert.FromBase64String(passwordCifrada);

                    cmd.Parameters.AddWithValue("@Rol_Id", u.Rol_Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellidos", u.Apellidos);
                    cmd.Parameters.AddWithValue("@Usuario", u.Usuario);
                    cmd.Parameters.Add("@Pw", SqlDbType.VarBinary, 64).Value = pwBytes;

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Usuarios");
            }
            catch
            {
                ViewBag.Roles = ObtenerRoles();
                return View(u);
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Editar(int id)
        {
            Usuarios u = new Usuarios();

            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    u.Id = Convert.ToInt64(dr["Id"]);
                    u.Rol_Id = Convert.ToInt32(dr["Rol_Id"]);
                    u.Nombre = dr["Nombre"].ToString();
                    u.Apellidos = dr["Apellidos"].ToString();
                    u.Usuario = dr["Usuario"].ToString();
                    u.Pw = Convert.ToBase64String((byte[])dr["Pw"]);
                }
            }

            ViewBag.Roles = ObtenerRoles();
            return View(u);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Editar(Usuarios u)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    
                    SqlCommand cmd = new SqlCommand("UPDATE Usuario SET Rol_Id=@Rol_Id, Nombre=@Nombre, Apellidos=@Apellidos, Usuario=@Usuario, Pw=@Pw WHERE Id=@Id", conn);

                    string passwordCifrada = Hashing.HashPassword(u.Pw);
                    byte[] pwBytes = Convert.FromBase64String(passwordCifrada);

                    cmd.Parameters.AddWithValue("@Id", u.Id);
                    cmd.Parameters.AddWithValue("@Rol_Id", u.Rol_Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellidos", u.Apellidos);
                    cmd.Parameters.AddWithValue("@Usuario", u.Usuario);
                    cmd.Parameters.Add("@Pw", SqlDbType.VarBinary, 64).Value = pwBytes;

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Usuarios");
            }
            catch
            {
                ViewBag.Roles = ObtenerRoles();
                return View(u);
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Eliminar(int id)
        {
            Usuarios u = new Usuarios();

            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    u.Id = Convert.ToInt64(dr["Id"]);
                    u.Nombre = dr["Nombre"].ToString();
                    u.Apellidos = dr["Apellidos"].ToString();
                    u.Usuario = dr["Usuario"].ToString();
                }
            }

            return View(u);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Eliminar(Usuarios u)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    
                    SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE Id=@Id", conn);
                    cmd.Parameters.AddWithValue("@Id", u.Id);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Usuarios");
            }
            catch
            {
                return View(u);
            }
        }

        // Método para cargar roles al ViewBag
        private List<SelectListItem> ObtenerRoles()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT Id, Nombres FROM Roles", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = dr["Id"].ToString(),
                        Text = dr["Nombres"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}
