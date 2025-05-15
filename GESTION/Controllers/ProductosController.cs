using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GESTION.Controllers
{
    public class ProductosController : Controller
    {
        private conexion conexion = new conexion();

        // GET: Productos
        public ActionResult Productos()
        {
            List<Productos> lista = new List<Productos>();

            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand(@"  
                    select a.Id, Proveedor_Id, b.Nombre as Proveedor , a.Marca_Id , c.Nombre as Marca,
                           a.Nombre, a.Descripcion, a.Costo, a.Activo ,a.Codigo_Barras
                    from Producto a 
                    inner join Proveedores b on b.Id= a.Proveedor_Id
                    inner join Marca c on c.Id= a.Marca_Id", conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Productos
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Proveedor_Id = Convert.ToInt32(dr["Proveedor_Id"]),
                        Proveedor = dr["Proveedor"].ToString(),
                        Marca_Id = Convert.ToInt32(dr["Marca_Id"]),
                        Marca = dr["Marca"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Costo = Convert.ToDecimal(dr["Costo"]),
                        Activo = Convert.ToBoolean(dr["Activo"]),
                        Barras = Convert.ToString(dr["Codigo_Barras"])
                    });
                }
            }

            return View(lista);
        }

        // GET: Productos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Productos/Create
        public ActionResult Crear()
        {
            ViewBag.Marca = ObtenerMarca();
            ViewBag.Proveedores = ObtenerProveedores();
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        public ActionResult Crear(Productos P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Producto (Proveedor_Id, Marca_Id, Nombre, Descripcion, Costo, Activo ,Codigo_Barras) VALUES (@Proveedor_Id, @Marca_Id, @Nombre, @Descripcion, @Costo, @Activo,@Barras)", conn);
                    cmd.Parameters.AddWithValue("@Proveedor_Id", P.Proveedor_Id);
                    cmd.Parameters.AddWithValue("@Marca_Id", P.Marca_Id);
                    cmd.Parameters.AddWithValue("@Nombre", P.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", P.Descripcion);
                    cmd.Parameters.AddWithValue("@Costo", P.Costo);
                    cmd.Parameters.AddWithValue("@Activo", P.Activo);
                    cmd.Parameters.AddWithValue("@Barras", P.Barras);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Productos");
            }
            catch
            {
                ViewBag.Marca = ObtenerMarca();
                ViewBag.Proveedores = ObtenerProveedores();
                return View(P);
            }
        }

        // GET: Productos/Edit/5
        public ActionResult Editar(int id)
        {
            Productos P = new Productos();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    P.Id = Convert.ToInt32(dr["Id"]);
                    P.Proveedor_Id = Convert.ToInt32(dr["Proveedor_Id"]);
                    P.Marca_Id = Convert.ToInt32(dr["Marca_Id"]);
                    P.Nombre = dr["Nombre"].ToString();
                    P.Descripcion = dr["Descripcion"].ToString();
                    P.Costo = Convert.ToDecimal(dr["Costo"]);
                    P.Activo = Convert.ToBoolean(dr["Activo"]);
                    P.Barras = dr["Codigo_Barras"].ToString();
                }
            }
            ViewBag.Marca = ObtenerMarca();
            ViewBag.Proveedores = ObtenerProveedores();
            return View(P);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        public ActionResult Editar(Productos P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Producto SET Proveedor_Id = @Proveedor_Id, Marca_Id = @Marca_Id, Nombre = @Nombre, Descripcion = @Descripcion, Costo = @Costo, Activo = @Activo , Codigo_Barras=@Barras WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", P.Id);
                    cmd.Parameters.AddWithValue("@Proveedor_Id", P.Proveedor_Id);
                    cmd.Parameters.AddWithValue("@Marca_Id", P.Marca_Id);
                    cmd.Parameters.AddWithValue("@Nombre", P.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", P.Descripcion);
                    cmd.Parameters.AddWithValue("@Costo", P.Costo);
                    cmd.Parameters.AddWithValue("@Activo", P.Activo);
                    cmd.Parameters.AddWithValue("@Barras", P.Barras);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Productos");
            }
            catch
            {
                ViewBag.Marca = ObtenerMarca();
                ViewBag.Proveedores = ObtenerProveedores();
                return View(P);
            }
        }

        // GET: Productos/Delete/5
        public ActionResult Eliminar(int id)
        {
            Productos P = new Productos();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    P.Id = Convert.ToInt32(dr["Id"]);
                    P.Proveedor_Id = Convert.ToInt32(dr["Proveedor_Id"]);
                    P.Marca_Id = Convert.ToInt32(dr["Marca_Id"]);
                    P.Nombre = dr["Nombre"].ToString();
                    P.Descripcion = dr["Descripcion"].ToString();
                    P.Costo = Convert.ToDecimal(dr["Costo"]);
                    P.Activo = Convert.ToBoolean(dr["Activo"]);
                    P.Barras = dr["Codigo_Barras"].ToString();
                }
            }
            return View(P);
        }

        // POST: Productos/Delete/5
        [HttpPost]
        public ActionResult Eliminar(Productos P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", P.Id);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Productos");
            }
            catch
            {
                return View(P);
            }
        }

        private List<SelectListItem> ObtenerProveedores()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Nombre FROM Proveedores", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = dr["Id"].ToString(),
                        Text = dr["Nombre"].ToString()
                    });
                }
            }
            return lista;
        }

        private List<SelectListItem> ObtenerMarca()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Nombre FROM Marca", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = dr["Id"].ToString(),
                        Text = dr["Nombre"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}
