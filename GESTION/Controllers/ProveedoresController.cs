using GESTION.Clases;
using GESTION.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GESTION.Controllers
{
    public class ProveedoresController : Controller
    {
        private conexion conexion = new conexion();

        // GET: Proveedores
        public ActionResult Proveedores()
        {
            List<Proveedores> lista = new List<Proveedores>();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand(@"SELECT a.Id, a.Nombre, a.Direccion, a.Telefono, a.Razon_Social, 
                                                   b.Id AS Moneda_Id, b.Nombre AS Moneda, a.Creado_El
                                                   FROM Proveedores a
                                                   INNER JOIN Monedas b ON b.Id = a.Moneda_Id", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Proveedores
                    {
                        Id = Convert.ToInt64(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        Telefono = dr["Telefono"].ToString(),
                        Razon_Social = dr["Razon_Social"].ToString(),
                        Moneda_Id = Convert.ToInt32(dr["Moneda_Id"]),
                        Moneda = dr["Moneda"].ToString(),
                        Creado_El = Convert.ToDateTime(dr["Creado_El"])
                    });
                }
            }
            return View(lista);
        }

        // GET: Proveedores/Details/5
        

        // GET: Proveedores/Create
        public ActionResult Crear()

        {
            ViewBag.Monedas = ObtenerMonedas();
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        public ActionResult Crear(Proveedores P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Proveedores (Nombre, Direccion, Telefono, Razon_Social, Moneda_Id) VALUES (@Nombre, @Direccion, @Telefono, @Razon_Social, @Moneda_Id)", conn);
                    cmd.Parameters.AddWithValue("@Nombre", P.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", P.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", P.Telefono);
                    cmd.Parameters.AddWithValue("@Razon_Social", P.Razon_Social);
                    cmd.Parameters.AddWithValue("@Moneda_Id", P.Moneda_Id);
                    // cmd.Parameters.AddWithValue("@Creado_El", DateTime.Now); // Asignar la fecha actual
                    cmd.ExecuteNonQuery();
                }
                // TODO: Add insert logic here

                return RedirectToAction("Proveedores");
            }
            catch
            {
                return View(P);
            }
        }

        // GET: Proveedores/Edit/5
        public ActionResult Editar(int Id)
        {
            Proveedores P = new Proveedores();

            using (var conn = conexion.AbrirConexion()) 
            {
                SqlCommand cmd = new SqlCommand("SELECT a.Id, a.Nombre, a.Direccion, a.Telefono, a.Razon_Social, b.Id AS Moneda_Id, b.Nombre AS Moneda FROM Proveedores a INNER JOIN Monedas b ON b.Id = a.Moneda_Id WHERE a.Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    P.Id = Convert.ToInt64(dr["Id"]);
                    P.Nombre = dr["Nombre"].ToString();
                    P.Direccion = dr["Direccion"].ToString();
                    P.Telefono = dr["Telefono"].ToString();
                    P.Razon_Social = dr["Razon_Social"].ToString();
                    P.Moneda_Id = Convert.ToInt32(dr["Moneda_Id"]);
                    P.Moneda = dr["Moneda"].ToString();
                }
                
            }
            ViewBag.Monedas = ObtenerMonedas();
            return View(P);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        public ActionResult Editar(Proveedores P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Proveedores SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Razon_Social = @Razon_Social, Moneda_Id = @Moneda_Id WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", P.Id);
                    cmd.Parameters.AddWithValue("@Nombre", P.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", P.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", P.Telefono);
                    cmd.Parameters.AddWithValue("@Razon_Social", P.Razon_Social);
                    cmd.Parameters.AddWithValue("@Moneda_Id", P.Moneda_Id);
                    // cmd.Parameters.AddWithValue("@Creado_El", DateTime.Now); // Asignar la fecha actual
                    // cmd.Parameters.AddWithValue("@Id", P.Id); // Asignar el ID del proveedor a actualizar
                    // cmd.Parameters.AddWithValue("@Creado_El", DateTime.Now); // Asignar la fecha actual
                    cmd.ExecuteNonQuery();
                }


                // TODO: Add update logic here

                return RedirectToAction("Proveedores");
            }
            catch
            {
                ViewBag.Monedas = ObtenerMonedas();
                return View(P);
            }
        }





        // GET: Proveedores/Delete/5
        public ActionResult Eliminar(int Id)
        {
            Proveedores P = new Proveedores();
            using (var conn = conexion.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proveedores WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    P.Id = Convert.ToInt64(dr["Id"]);
                    P.Nombre = dr["Nombre"].ToString();
                    P.Direccion = dr["Direccion"].ToString();
                    P.Telefono = dr["Telefono"].ToString();
                    P.Razon_Social = dr["Razon_Social"].ToString();
                    P.Moneda_Id = Convert.ToInt32(dr["Moneda_Id"]);
                }
            }
            return View(P);
        }

        // POST: Proveedores/Delete/5
        [HttpPost]
        public ActionResult Eliminar(Proveedores P)
        {
            try
            {
                using (var conn = conexion.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Proveedores WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", P.Id);
                    cmd.ExecuteNonQuery();
                }
                    // TODO: Add delete logic here

                    return RedirectToAction("Proveedores");
            }
            catch
            {
                return View(P);
            }
        }

        // Método para obtener la lista de monedas
        private List<SelectListItem> ObtenerMonedas()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            using (var conn = conexion.AbrirConexion())
            {

                SqlCommand cmd = new SqlCommand("SELECT Id, Nombre FROM Monedas", conn);
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
