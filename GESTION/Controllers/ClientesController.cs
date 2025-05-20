using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class ClientesController : Controller
    {
        private readonly conexion _conn = new conexion();

        // GET: Clientes
        public ActionResult Clientes()
        {
            var lista = new List<GESTION.Models.Clientes>();
            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT 
                    c.Id,
                    c.Tipo_Id,
                    t.Descripcion AS Tipo,
                    c.Nombre,
                    c.Nit,
                    c.Direccion,
                    c.Correo,
                    c.Creado_El,
                    c.Activo
                  FROM Clientes c
             LEFT JOIN TipoCliente t ON t.Id = c.Tipo_Id", cn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new Clientes
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Tipo_Id = Convert.ToInt32(dr["Tipo_Id"]),
                        Tipo = Convert.ToString(dr["Tipo"]),
                        Nombre = Convert.ToString(dr["Nombre"]),
                        Nit = dr["Nit"] is DBNull ? null : Convert.ToString(dr["Nit"]),
                        Direccion = dr["Direccion"] is DBNull ? null : Convert.ToString(dr["Direccion"]),
                        Correo = dr["Correo"] is DBNull ? null : Convert.ToString(dr["Correo"]),
                        Creado_El = Convert.ToDateTime(dr["Creado_El"]),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    });
                }
            }
            return View(lista);
        }

        // GET: Clientes/Crear
        public ActionResult Crear()
        {
            ViewBag.Tipos = ObtenerTipos();
            return View(new Clientes { Creado_El = DateTime.Now, Activo = true });
        }

        // POST: Clientes/Crear
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Crear(Clientes model)
        {
            ViewBag.Tipos = ObtenerTipos();
            if (!ModelState.IsValid)
                return View(model);

            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                INSERT INTO Clientes
                    (Tipo_Id, Nombre, Nit, Direccion, Correo, Creado_El, Activo)
                VALUES
                    (@TipoId, @Nombre, @Nit, @Direccion, @Correo, GETDATE(), @Activo)", cn))
            {
                cmd.Parameters.AddWithValue("@TipoId", model.Tipo_Id);
                cmd.Parameters.AddWithValue("@Nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@Nit", (object)model.Nit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)model.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)model.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", model.Activo);
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Cliente creado con éxito.";
            return RedirectToAction("Clientes");
        }

        // GET: Clientes/Editar/5
        public ActionResult Editar(int id)
        {
            Clientes model = null;
            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT 
                    Id, Tipo_Id, Nombre, Nit, Direccion, Correo, Creado_El, Activo
                  FROM Clientes
                 WHERE Id = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new Clientes
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Tipo_Id = Convert.ToInt32(dr["Tipo_Id"]),
                            Nombre = Convert.ToString(dr["Nombre"]),
                            Nit = dr["Nit"] is DBNull ? null : Convert.ToString(dr["Nit"]),
                            Direccion = dr["Direccion"] is DBNull ? null : Convert.ToString(dr["Direccion"]),
                            Correo = dr["Correo"] is DBNull ? null : Convert.ToString(dr["Correo"]),
                            Creado_El = Convert.ToDateTime(dr["Creado_El"]),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        };
                    }
                }
            }
            if (model == null) return HttpNotFound();

            ViewBag.Tipos = ObtenerTipos();
            return View(model);
        }

        // POST: Clientes/Editar/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Editar(Clientes model)
        {
            ViewBag.Tipos = ObtenerTipos();
            if (!ModelState.IsValid)
                return View(model);

            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                UPDATE Clientes SET
                    Tipo_Id   = @TipoId,
                    Nombre    = @Nombre,
                    Nit       = @Nit,
                    Direccion = @Direccion,
                    Correo    = @Correo,
                    Activo    = @Activo
                 WHERE Id = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@TipoId", model.Tipo_Id);
                cmd.Parameters.AddWithValue("@Nombre", model.Nombre);
                cmd.Parameters.AddWithValue("@Nit", (object)model.Nit ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)model.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)model.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", model.Activo);
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Cliente actualizado con éxito.";
            return RedirectToAction("Clientes");
        }

        // GET: Clientes/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            Clientes model = null;
            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT Id, Nombre, Nit
                  FROM Clientes
                 WHERE Id = @Id", cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new Clientes
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = Convert.ToString(dr["Nombre"]),
                            Nit = dr["Nit"] is DBNull ? null : Convert.ToString(dr["Nit"])
                        };
                    }
                }
            }
            if (model == null) return HttpNotFound();
            return View(model);
        }

        // POST: Clientes/Eliminar/5
        [HttpPost, ActionName("Eliminar"), ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmed(int id)
        {
            try
            {
                using (var cn = _conn.AbrirConexion())
                using (var cmd = new SqlCommand("DELETE FROM Clientes WHERE Id = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                TempData["Success"] = "Cliente eliminado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar: " + ex.Message;
            }
            return RedirectToAction("Clientes");
        }

        // Helper: lista de tipos para dropdown
        private List<SelectListItem> ObtenerTipos()
        {
            var items = new List<SelectListItem>();
            using (var cn = _conn.AbrirConexion())
            using (var cmd = new SqlCommand("SELECT Id, Descripcion FROM TipoCliente", cn))
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Value = Convert.ToString(dr["Id"]),
                        Text = Convert.ToString(dr["Descripcion"])
                    });
                }
            }
            items.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione Tipo --" });
            return items;
        }
    }
}
