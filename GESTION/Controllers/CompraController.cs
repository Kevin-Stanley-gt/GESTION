using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class CompraController : Controller
    {
        private conexion conn = new conexion();

        // GET: Compra/Crear
                public ActionResult Crear()
                {
                    var model = new CompraViewModel
                    {
                        Encabezado = new CompraEncabezado { Fecha = DateTime.Now },
                        Detalles = new List<CompraDetalle>(),
                        Proveedores = ObtenerProveedores()
                    };
                    return View(model);

        }

        // POST: Compra/Crear
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Crear(CompraViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Proveedores = ObtenerProveedores();
                return View(model);
            }

            var sqlConn = conn.AbrirConexion();
            var tx = sqlConn.BeginTransaction();
            try
            {
                // Insert encabezado
                var cmdEnc = new SqlCommand(@"
                    INSERT INTO Compra_Encabezado (Numero, Serie, Fecha, ProveedorId)
                    OUTPUT INSERTED.Id
                    VALUES (@Numero, @Serie, @Fecha, @ProvId)", sqlConn, tx);
                cmdEnc.Parameters.AddWithValue("@Numero", model.Encabezado.Numero);
                cmdEnc.Parameters.AddWithValue("@Serie", model.Encabezado.Serie);
                cmdEnc.Parameters.AddWithValue("@Fecha", model.Encabezado.Fecha);
                cmdEnc.Parameters.AddWithValue("@ProvId", model.Encabezado.ProveedorId);
                long compraId = Convert.ToInt64(cmdEnc.ExecuteScalar());

                // Insert detalles
                var cmdDet = new SqlCommand(@"
                    INSERT INTO Compra_Detalle (Compra_Encabezado_Id, Producto_Id, Cantidad, Precio, Comentario)
                    VALUES (@EncId, @ProdId, @Cantidad, @Precio,@Comentario)", sqlConn, tx);

                foreach (var d in model.Detalles)
                {
                    cmdDet.Parameters.Clear();
                    cmdDet.Parameters.AddWithValue("@EncId", compraId);
                    cmdDet.Parameters.AddWithValue("@ProdId", d.Producto_Id);
                    cmdDet.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                    cmdDet.Parameters.AddWithValue("@Precio", d.Precio);
                    cmdDet.Parameters.AddWithValue("@Comentario", d.Comentario);
                    cmdDet.ExecuteNonQuery();
                }

                tx.Commit();
                TempData["Success"] = "¡Compra agregada con éxito!";
                return RedirectToAction("Crear");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ViewBag.Error = "Error al guardar: " + ex.Message;
                model.Proveedores = ObtenerProveedores();
                return View(model);
            }
            finally
            {
                conn.CerrarConexion(sqlConn);
            }
        }

        // GET: Compra/BuscarProductos?term=abcd
        public JsonResult BuscarProductos(string term)
        {
            var lista = new List<ProductoItem>();
            if (string.IsNullOrWhiteSpace(term) || term.Length < 4)
                return Json(lista, JsonRequestBehavior.AllowGet);

            var cn = conn.AbrirConexion();
            using (var cmd = new SqlCommand(@"
                SELECT TOP 10 a.Id, a.Nombre, b.Nombre as Marca, a.Costo as Precio 
                    FROM Producto a
                    inner join Marca b on a.Marca_Id= b.Id
                WHERE a.Nombre LIKE @t OR a.Codigo_Barras LIKE @t", cn))
            {
                cmd.Parameters.AddWithValue("@t", "%" + term + "%");
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        lista.Add(new ProductoItem
                        {
                            Id = Convert.ToInt64(rdr["Id"]),
                            Nombre = rdr["Nombre"].ToString(),
                            Marca = rdr["Marca"].ToString(),
                            Precio = Convert.ToDecimal(rdr["Precio"])
                        });
                    }
                }
            }
            conn.CerrarConexion(cn);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> ObtenerProveedores()
        {
            var lst = new List<SelectListItem>();
            var cn = conn.AbrirConexion();
            using (var cmd = new SqlCommand("SELECT Id, Nombre FROM Proveedores", cn))
            using (var rdr = cmd.ExecuteReader())
                while (rdr.Read())
                    lst.Add(new SelectListItem
                    {
                        Value = rdr["Id"].ToString(),
                        Text = rdr["Nombre"].ToString()
                    });
            conn.CerrarConexion(cn);
            return lst;
        }
    }
}
