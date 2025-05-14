using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GESTION.Clases
{
    public class conexion
    {
        // Cadena de conexión (reemplaza los valores con los de tu base de datos)
        private string cadenaConexion = "Server=192.168.1.151; Database=GestionFarmatodo;User Id=sa; Password=Farmagt2021;";

        // Método para abrir la conexión
        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectar a la base de datos: " + ex.Message);
            }
            return conexion;
        }

        // Método para cerrar la conexión
        public void CerrarConexion(SqlConnection conexion)
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }
    }
}