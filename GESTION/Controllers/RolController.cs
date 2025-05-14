using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using GESTION.Clases;

namespace POS_FARMATODO.Controllers
{
    public class RolController : Controller
    {
        private conexion conexion = new conexion(); // Clase que contiene la cadena de conexión

        // LISTAR
        public ActionResult Rol()
        {
            var roles = new List<Rol>();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Roles", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(new Rol
                    {
                        Id = (int)reader["Id"],
                        Nombres = reader["Nombres"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Creado_El = Convert.ToDateTime(reader["Creado_El"])
                    });
                }
            }

            return View(roles);
        }

        // DETALLES
        public ActionResult Detalles(int id)
        {
            Rol rol = null;
            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Roles WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rol = new Rol
                    {
                        Id = (int)reader["Id"],
                        Nombres = reader["Nombres"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Creado_El = Convert.ToDateTime(reader["Creado_El"])
                    };
                }
            }

            if (rol == null)
                return HttpNotFound();

            return View(rol);
        }

        // CREAR
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Rol rol)
        {
            if (ModelState.IsValid)
            {
                using (var conn = conexion.AbrirConexion())
                {
                    
                    SqlCommand cmd = new SqlCommand("INSERT INTO Roles (Nombres, Descripcion, Creado_El) VALUES (@n, @d, @c)", conn);
                    cmd.Parameters.AddWithValue("@n", rol.Nombres);
                    cmd.Parameters.AddWithValue("@d", rol.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@c", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Rol");
            }

            return View(rol);
        }

        // EDITAR
        public ActionResult Editar(int id)
        {
            Rol rol = null;
            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Roles WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rol = new Rol
                    {
                        Id = (int)reader["Id"],
                        Nombres = reader["Nombres"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Creado_El = Convert.ToDateTime(reader["Creado_El"])
                    };
                }
            }

            if (rol == null)
                return HttpNotFound();

            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Rol rol)
        {
            if (ModelState.IsValid)
            {
                using (var conn = conexion.AbrirConexion())
                {
                   
                    SqlCommand cmd = new SqlCommand("UPDATE Roles SET Nombres = @n, Descripcion = @d WHERE Id = @id", conn);
                    cmd.Parameters.AddWithValue("@n", rol.Nombres);
                    cmd.Parameters.AddWithValue("@d", rol.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@id", rol.Id);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Rol");
            }

            return View(rol);
        }

        // ELIMINAR
        public ActionResult Eliminar(int id)
        {
            Rol rol = null;
            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Roles WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rol = new Rol
                    {
                        Id = (int)reader["Id"],
                        Nombres = reader["Nombres"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Creado_El = Convert.ToDateTime(reader["Creado_El"])
                    };
                }
            }

            if (rol == null)
                return HttpNotFound();

            return View(rol);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            using (var conn = conexion.AbrirConexion())
            {
                
                SqlCommand cmd = new SqlCommand("DELETE FROM Roles WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Rol");
        }
    }
}
