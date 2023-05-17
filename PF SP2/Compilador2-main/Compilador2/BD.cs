using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Compilador2
{
    
    class BD
    {
        private string nombre;
        private string servidor;
        private SqlConnection conexion;

        public BD()
        {
            StreamReader config = new StreamReader("config.txt");
            nombre = config.ReadLine();
            servidor = config.ReadLine();
            conexion = new SqlConnection("Server=" + servidor + ";Database=" + nombre + ";Trusted_Connection=True;");
        }

        public DataTable consulta(string consulta)
        {
            conexion.Open();
            SqlCommand ejecuta = new SqlCommand(consulta,conexion);
            SqlDataReader lee = ejecuta.ExecuteReader();
            DataTable datos = new DataTable();
            datos.Load(lee);
            conexion.Close();
            return datos;
        }
    }
}
