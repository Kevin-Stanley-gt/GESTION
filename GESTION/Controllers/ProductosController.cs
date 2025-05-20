using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class ProductosController : Controller
    {
        private readonly conexion _conexion = new conexion();

        // GET: Productos — listado
        public ActionResult Productos()
        {
            var lista = new List<Productos>();
            using (var conn = _conexion.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT 
                    a.Id,
                    a.Proveedor_Id,
                    a.Nombre,
                    a.Descripcion,
                    a.Costo,
                    a.Activo,
                    a.Codigo_Barras,
                    b.Nombre AS Marca,
                    c.Nombre AS Proveedor
                  FROM Producto a
                 INNER JOIN Marca b       ON b.Id = a.Marca_Id
                 INNER JOIN Proveedores c ON c.Id = a.Proveedor_Id", conn))
            using (var dr = cmd.ExecuteReader())
                while (dr.Read())
                    lista.Add(new Productos
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Proveedor_Id = Convert.ToInt32(dr["Proveedor_Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Costo = Convert.ToDecimal(dr["Costo"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        Barras = dr["Codigo_Barras"].ToString(),
                        Marca = dr["Marca"].ToString(),
                        Proveedor = dr["Proveedor"].ToString(),
                    });

            // Lleva el mensaje al ViewBag si existe
            if (TempData["SuccessProd"] != null)
                ViewBag.Success = TempData["SuccessProd"];

            return View(lista);
        }

        // GET: Productos/Eliminar/5 — muestra confirmación
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            Productos P = null;
            using (var conn = _conexion.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT 
                    a.Id,
                    a.Nombre,
                    a.Codigo_Barras AS Barras,
                    b.Nombre        AS Marca,
                    c.Nombre        AS Proveedor
                  FROM Producto a
                 INNER JOIN Marca b       ON b.Id = a.Marca_Id
                 INNER JOIN Proveedores c ON c.Id = a.Proveedor_Id
                 WHERE a.Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (var dr = cmd.ExecuteReader())
                    if (dr.Read())
                        P = new Productos
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            Barras = dr["Barras"].ToString(),
                            Marca = dr["Marca"].ToString(),
                            Proveedor = dr["Proveedor"].ToString()
                        };
            }

            if (P == null)
                return HttpNotFound();

            return View(P);
        }

        // POST: Productos/Eliminar/5 — borra y redirige
        [HttpPost, ActionName("Eliminar"), ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmed(int id)
        {
            try
            {
                using (var conn = _conexion.AbrirConexion())
                using (var cmd = new SqlCommand("DELETE FROM Producto WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                TempData["SuccessProd"] = "Producto eliminado con éxito.";
                return RedirectToAction("Productos");
            }
            catch (Exception ex)
            {
                // Si falla, vuelve a la confirmación con mensaje de error
                ViewBag.Error = "No se pudo eliminar: " + ex.Message;
                return View("Eliminar", new Productos { Id = id });
            }
        }
    }
}
