using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GESTION.Clases
{
    public class Bitacora
    {
        private conexion conexion = new conexion();

        // Método para registrar una acción en la bitácora
        public void RegistrarAccion(int usuarioId, string rolUsuario, string accion, string tablaAfectada, int registroAfectado)
        {
            using (SqlConnection conn = conexion.AbrirConexion())
            {
                string query = @"
                    INSERT INTO Bitacora (Usuario_Id, Rol_Usuario, Accion, Tabla_Afectada, Registro_Afectado, Fecha)
                    VALUES (@Usuario_Id, @Rol_Usuario, @Accion, @Tabla_Afectada, @Registro_Afectado, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario_Id", usuarioId);
                cmd.Parameters.AddWithValue("@Rol_Usuario", rolUsuario);
                cmd.Parameters.AddWithValue("@Accion", accion);
                cmd.Parameters.AddWithValue("@Tabla_Afectada", tablaAfectada);
                cmd.Parameters.AddWithValue("@Registro_Afectado", registroAfectado);

                cmd.ExecuteNonQuery();
            }
        }
    }
}