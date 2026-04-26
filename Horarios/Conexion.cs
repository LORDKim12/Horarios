using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Horarios
{
    public class Conexion
    {
        // ATENCIÓN: Cambia "tu_contraseña" por la contraseña de tu base de datos MySQL (por ejemplo, si usas XAMPP suele estar vacía "").
        private string cadenaConexion = "Server=127.0.0.1;Database=SistemaHorarios;Uid=root;Pwd=admin12*;";

        // Método para obtener y abrir la conexión
        public MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);
            return conexion;
        }
    }
}
    