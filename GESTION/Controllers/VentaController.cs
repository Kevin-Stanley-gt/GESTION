using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using QuestPDF.Fluent;
using System.IO;

namespace GESTION.Controllers
{
    public class VentaController : Controller
    {
        private readonly conexion conn = new conexion();

        // GET: Venta/Crear
        public ActionResult Crear()
        {
            var model = new VentaViewModel
            {
                Encabezado = new VentaEncabezdo { Creado_El = DateTime.Now },
                Detalles = new List<VentaDetalle>(),
                Clientes = ObtenerClientes()
            };
            return View(model);
        }

        // POST: Venta/Crear
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Crear(VentaViewModel model)
        {
            model.Clientes = ObtenerClientes();
            model.Encabezado.Cantidad = model.Detalles.Sum(d => d.Cantidad);
            model.Encabezado.Costo_Total = model.Detalles.Sum(d => d.Cantidad * d.CostoUnitario);

            if (!model.Detalles.Any())
                ModelState.AddModelError("", "Debes agregar al menos un producto.");
            if (model.Encabezado.Costo_Total <= 0)
                ModelState.AddModelError("", "El total debe ser mayor que cero.");

            if (!ModelState.IsValid)
                return View(model);

            SqlConnection sqlConn = null;
            try
            {
                sqlConn = conn.AbrirConexion();
                long ventaId;
                using (var tx = sqlConn.BeginTransaction())
                {
                    // Insertar encabezado
                    var cmdEnc = new SqlCommand(@"
                        INSERT INTO Ventas
                            (Jornada_Id, Cliente_Id, Cantidad, Costo_Total, Creado_El)
                        OUTPUT INSERTED.Id
                        VALUES
                            (1, @Cliente_Id, @Cantidad, @Costo_Total, GETDATE())",
                        sqlConn, tx);

                    cmdEnc.Parameters.AddWithValue("@Cliente_Id", model.Encabezado.Cliente_Id);
                    cmdEnc.Parameters.AddWithValue("@Cantidad", model.Encabezado.Cantidad);
                    cmdEnc.Parameters.AddWithValue("@Costo_Total", model.Encabezado.Costo_Total);

                    ventaId = Convert.ToInt64(cmdEnc.ExecuteScalar());

                    // Insertar detalles
                    var cmdDet = new SqlCommand(@"
                        INSERT INTO VentasDetalle
                            (Venta_Id, Producto_Id, Cantidad, CostoUnitario, Fecha, Comentario)
                        VALUES
                            (@VentaId, @ProductoId, @Cantidad, @CostoUnitario, GETDATE(),@Comentario)",
                        sqlConn, tx);

                    foreach (var d in model.Detalles)
                    {
                        cmdDet.Parameters.Clear();
                        cmdDet.Parameters.AddWithValue("@VentaId", ventaId);
                        cmdDet.Parameters.AddWithValue("@ProductoId", d.Producto_Id);
                        cmdDet.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                        cmdDet.Parameters.AddWithValue("@CostoUnitario", d.CostoUnitario);
                        cmdDet.Parameters.AddWithValue("@Comentario", d.Comentario);
                        cmdDet.ExecuteNonQuery();
                    }

                    tx.Commit();
                }

                // Poblar datos para el PDF
                model.Encabezado.Id = (int)ventaId;

                using (var cn = conn.AbrirConexion())
                using (var cmd = new SqlCommand("SELECT Nombre FROM Clientes WHERE Id = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Id", model.Encabezado.Cliente_Id);
                    var nombreCliente = cmd.ExecuteScalar();
                    model.Encabezado.Cliente_Nombre = nombreCliente != null ? nombreCliente.ToString() : "";
                }

                var productoIds = model.Detalles.Select(d => d.Producto_Id).Distinct().ToList();
                var nombresProductos = new Dictionary<int, string>();
                if (productoIds.Any())
                {
                    var idsParam = string.Join(",", productoIds);
                    using (var cn = conn.AbrirConexion())
                    using (var cmd = new SqlCommand($"SELECT Id, Nombre FROM Producto WHERE Id IN ({idsParam})", cn))
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int id = Convert.ToInt32(rdr["Id"]);
                            string nombre = rdr["Nombre"].ToString();
                            nombresProductos[id] = nombre;
                        }
                    }
                }
                foreach (var d in model.Detalles)
                {
                    if (nombresProductos.ContainsKey(d.Producto_Id))
                        d.Producto_Nombre = nombresProductos[d.Producto_Id];
                }

                // Generar el PDF
                var pdf = new VentaPdf(model);
                byte[] pdfBytes = pdf.GeneratePdf();

                // Guardar el PDF temporalmente
                var fileName = $"Factura_{ventaId}_{Guid.NewGuid()}.pdf";
                var tempPath = Server.MapPath("~/Temp/");
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                var filePath = Path.Combine(tempPath, fileName);
                System.IO.File.WriteAllBytes(filePath, pdfBytes);

                TempData["Success"] = "¡Venta registrada exitosamente!";
                TempData["PdfFile"] = fileName;

                return RedirectToAction("Crear");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                return View(model);
            }
            finally
            {
                if (sqlConn != null) conn.CerrarConexion(sqlConn);
            }
        }

        public ActionResult DescargarFactura(string file)
        {
            var filePath = Server.MapPath("~/Temp/" + file);
            if (!System.IO.File.Exists(filePath))
                return HttpNotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath); // Limpieza opcional
            return File(bytes, "application/pdf", file);
        }

        private IEnumerable<SelectListItem> ObtenerClientes()
        {
            var items = new List<SelectListItem>();
            using (var cn = conn.AbrirConexion())
            using (var cmd = new SqlCommand("SELECT Id, Nombre FROM Clientes", cn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Value = rdr["Id"].ToString(),
                        Text = rdr["Nombre"].ToString()
                    });
                }
            }
            return items;
        }

        public JsonResult BuscarProductos(string term)
        {
            var list = new List<ProductoItem>();
            if (string.IsNullOrWhiteSpace(term) || term.Length < 4)
                return Json(list, JsonRequestBehavior.AllowGet);

            using (var cn = conn.AbrirConexion())
            using (var cmd = new SqlCommand(@"
                SELECT TOP 10 p.Id, p.Nombre, m.Nombre AS Marca, p.Costo AS Precio
                  FROM Producto p
                  JOIN Marca m ON m.Id = p.Marca_Id
                 WHERE p.Nombre LIKE @t OR p.Codigo_Barras LIKE @t", cn))
            {
                cmd.Parameters.AddWithValue("@t", $"%{term}%");
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        list.Add(new ProductoItem
                        {
                            Id = Convert.ToInt64(rdr["Id"]),
                            Nombre = rdr["Nombre"].ToString(),
                            Marca = rdr["Marca"].ToString(),
                            Precio = Convert.ToDecimal(rdr["Precio"])
                        });
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}