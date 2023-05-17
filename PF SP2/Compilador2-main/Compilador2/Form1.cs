using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Compilador2
{
    public partial class Form1 : Form
    {
        private string idUsuario;
        private string idLenguaje;
        private string nombreLenguaje;
        private string nombreUsuario;

        public Form1()
        {
            InitializeComponent();
        }
        private int[][] matriz;

        private int contar(StreamReader archivo)
        {
            int lineas = 0;
            while (!archivo.EndOfStream)
            {
                archivo.ReadLine();
                lineas++;
            }
            return lineas;
        }

        private int stringToint(string pf)
        {
            return Convert.ToInt32(pf);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BD iniciar = new BD();
            nombreLenguaje = iniciar.consulta("select nombre from lenguajes where idLenguaje=" + idLenguaje).Rows[0]["nombre"].ToString();
            nombreUsuario = iniciar.consulta("select nombre from usuarios where idUsuario=" + idUsuario).Rows[0]["nombre"].ToString();
            Label8.Text += nombreLenguaje;

            StreamReader cargarMatriz = new StreamReader("datos/"+nombreLenguaje+"/matriz.txt");
            matriz = new int[contar(new StreamReader("datos/"+nombreLenguaje+"/matriz.txt"))][];

            int i = 0;
            while (!cargarMatriz.EndOfStream)
            {
                matriz[i] = Array.ConvertAll(cargarMatriz.ReadLine().Split(','),new Converter<string,int>(stringToint));
                i++;
            }

            StreamReader cargarPr = new StreamReader("datos/"+nombreLenguaje+"/pr.txt");
            while (!cargarPr.EndOfStream)
            {
                ListBox3.Items.Add(cargarPr.ReadLine());
            }
        }

 
        private int renglon;
        private int Estado;
        private string token, PalabraSalida;
        private string Lineaz, direccion;
        private void Compilar_Click(object sender, EventArgs e)
        {
            renglon = 0;
            while (renglon < ListBox1.Items.Count)
            {
                ListBox1.SelectedIndex = renglon;
                Lineaz = ListBox1.Items[renglon].ToString();
                BuscaToken();
                renglon = renglon + 1;
            }
            DateTime ahora = DateTime.Now;
            string fecha = ahora.ToString().Substring(0, 2) + "_" + ahora.ToString().Substring(3, 2) + "_" + ahora.ToString().Substring(6, 4) + "_" + ahora.ToString().Substring(11, 2) + "_" + ahora.ToString().Substring(14, 2) + "_" + ahora.ToString().Substring(17, 2);
            StreamWriter salida = new StreamWriter("salidas/Output"+nombreLenguaje+nombreUsuario+fecha);
            
            for(int i = 0; i < ListBox2.Items.Count;i++)
            {
                ListBox2.SelectedIndex = i;
                salida.WriteLine(ListBox2.SelectedItem.ToString());
            }
            salida.Close();
            BD log = new BD();
            log.consulta("insert into log(IdUsuario,IdLenguaje,fecha,archivo) values("+idUsuario+","+idLenguaje+",'"+ahora.ToString("yyyyMMdd HH:mm:ss")+"','Output"+nombreLenguaje+nombreUsuario+fecha+".txt')");
        }
        private void BuscaId()
        {
            var encontro = false;
            var renglon2 = 0;
            while (((encontro == false) & (renglon2 < ListBox4.Items.Count)))
            {
                if ((token.ToUpper() == ListBox4.Text.ToUpper()))
                {
                    encontro = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }
            if ((encontro == false))
            {
                ListBox4.Items.Add(token);
                direccion = (ListBox4.Items.Count - 1).ToString();
            }
        }
        private void CntesEnteras()
        {
            bool encontro = false; // Declaración con tipo de dato 'bool'
            int renglon2 = 0; // Declaración con tipo de dato 'int'

            encontro = false;
            renglon2 = 0;

            while (((encontro == false) & (renglon2 < ListBox6.Items.Count)))
            {
                ListBox6.SelectedIndex = renglon2;
                if ((token.ToUpper() == ListBox6.Text.ToUpper())) // Reemplazar 'UCase' por 'ToUpper()'
                {
                    encontro = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }

            if (((encontro == false)))
            {
                ListBox6.Items.Add(token);
                direccion = (ListBox6.Items.Count - 1).ToString();
            }
        }
        private void CntesReales()
        {
            bool encontro = false; // Declaración con tipo de dato 'bool'
            int renglon2 = 0; // Declaración con tipo de dato 'int'

            encontro = false;
            renglon2 = 0;

            while (((encontro == false) & (renglon2 < ListBox7.Items.Count)))
            {
                ListBox7.SelectedIndex = renglon2;
                if ((token.ToUpper() == ListBox7.Text.ToUpper())) // Reemplazar 'UCase' por 'ToUpper()'
                {
                    encontro = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }

            if (((encontro == false)))
            {
                ListBox7.Items.Add(token);
                direccion = (ListBox7.Items.Count - 1).ToString();
            }
        }
        private void Strings()
        {
            bool encontro = false;
            int renglon2 = 0;

            encontro = false;
            renglon2 = 0;

            while (((encontro == false) & (renglon2 < ListBox5.Items.Count)))
            {
                ListBox5.SelectedIndex = renglon2;
                if ((token.ToUpper() == ListBox5.Text.ToUpper()))
                {
                    encontro = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }

            if ((encontro == false))
            {
                ListBox5.Items.Add(token);
                direccion = (ListBox5.Items.Count - 1).ToString();
            }
        }
        private int ColumnaN;
        private int Posicion;
        private string Caracter;

        // **************

        public void BuscaToken()
        {
            Estado = 0;
            token = "";
            Posicion = 1;

            while (Posicion <= Lineaz.Length) // mientras que la posición sea menor que la longitud de Lineaz
            {
                Caracter = Lineaz.Substring(Posicion - 1, 1); // extrae de una cadena el carácter indicado en la posición indicada 
                ColumnaX();
                Estado = matriz[Estado][ColumnaN]; // caracter = cadena, inicio, #caracteres a sacar

                if (Estado >= 100)
                {
                    if (token.Length > 0)
                        ReconocimientoToken();
                    else if (token.Length == 0)
                    {
                        token = token + Caracter; // solo lee un carácter 
                        ReconocimientoToken();
                    }
                    Estado = 0;
                    token = "";
                }
                else if (Estado != 0)
                {
                    token = token + Caracter; // añadir a token el carácter (h)
                }

                Posicion = Posicion + 1;
            }

            if (token.Length > 0)
            {
                Caracter = " ";
                ColumnaX();
                Estado = matriz[Estado][ColumnaN];
                ReconocimientoToken();
            }
        }
        public void ColumnaX()
        {
            if (Caracter.ToCharArray()[0] >= 'A' && Caracter.ToCharArray()[0] <= 'Z' || Caracter.ToCharArray()[0] >= 'a' && Caracter.ToCharArray()[0] <= 'z')
                ColumnaN = 0;
            else if (Caracter == " " || Caracter == "")
                ColumnaN = 2;
            else if (Caracter.ToCharArray()[0] >= '0' && Caracter.ToCharArray()[0] <= '9')
                ColumnaN = 1;
            else if (Caracter == "_")
                ColumnaN = 3;
            else if (Caracter == ".")
                ColumnaN = 4;
            else if (Caracter == "\"")
                ColumnaN = 5;
            else if (Caracter == "#")
                ColumnaN = 6;
            else if (Caracter == "+")
                ColumnaN = 7;
            else if (Caracter == "-")
                ColumnaN = 8;
            else if (Caracter == "*")
                ColumnaN = 9;
            else if (Caracter == "/")
                ColumnaN = 10;
            else if (Caracter == ">")
                ColumnaN = 11;
            else if (Caracter == "<")
                ColumnaN = 12;
            else if (Caracter == "=")
                ColumnaN = 13;
        }
        bool Pr;

        public string IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string IdLenguaje { get => idLenguaje; set => idLenguaje = value; }

        private void Button1_Click(object sender, EventArgs e)
        {
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
            ListBox4.Items.Clear();
            ListBox5.Items.Clear();
            ListBox6.Items.Clear();
            ListBox7.Items.Clear();
        }


        public void ReconocimientoToken()
            {
                if (Estado == 100)
                {
                    Pr = false;
                    BuscapalReservada();
                    if (Pr)
                        PalabraSalida = token + "   Pr   " + direccion;
                    else
                    {
                        Buscaidentificadores();
                        PalabraSalida = token + " Id  " + direccion;
                        Posicion = Posicion - 1;
                    }
                }
                if (Estado == 101)
                {
                    CntesEnteras();
                    PalabraSalida = token + " Cnte. Entera" + direccion;
                    Posicion = Posicion - 1; // retrocede para volver a identificar el ultimo caracter 
                }
                else if (Estado == 102)
                {
                    CntesReales();
                    PalabraSalida = token + " Cnte. Real" + direccion;
                    Posicion = Posicion - 1;
                }
                if (Estado == 103)
                {
                    token = token + Caracter; // almacena las ultimas comillas 
                    Strings();
                    PalabraSalida = token + " String" + direccion;
                }
                if (Estado == 105)
                    PalabraSalida = token + " C.Esp";
                else if (Estado == 106)
                    PalabraSalida = token + " C.Esp";
                else if (Estado == 107)
                    PalabraSalida = token + " C.Esp";
                else if (Estado == 108)
                    PalabraSalida = token + " C.Esp";
                else if (Estado == 109)
                {
                    PalabraSalida = token + " C.Esp";
                    Posicion = Posicion - 1;
                }
                else if (Estado == 110)
                {
                    token = token + Caracter; // vacio = vacio+ ultimo caracter

                    PalabraSalida = token + " C.Esp";
                }
                else if (Estado == 111)
                {
                    token = token + Caracter;
                    PalabraSalida = token + " C.Esp";
                }
                else if (Estado == 112)
                {
                    PalabraSalida = token + " C.Esp";
                    Posicion = Posicion - 1;
                }
                else if (Estado == 113)
                {
                    token = token + Caracter;
                    PalabraSalida = token + " C.Esp";
                }
                else if (Estado == 114)
                {
                    token = token + Caracter;
                    PalabraSalida = token + " C.Esp";
                }
                else if (Estado >= 300)
            {
                PalabraSalida = "Error " + Estado;
            }


                ListBox2.Items.Add(PalabraSalida);
            }

        private void Cargar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();

            string archivo = openFileDialog1.FileName;
            System.IO.StreamReader fileReader;
            try
            {
                fileReader = new System.IO.StreamReader(archivo);
                string stringReader;
                while (!(fileReader.EndOfStream))
                {
                    stringReader = fileReader.ReadLine();
                    ListBox1.Items.Add(stringReader);
                }
                fileReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al abrir el archivo: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exportar log = new Exportar();
            log.Visible = true;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void Buscaidentificadores()
        {
            bool encontro = false;
            int renglon2 = 0;

            while (!encontro && renglon2 < ListBox4.Items.Count)
            {
                ListBox4.SelectedIndex = renglon2;
                if (token.ToUpper() == ListBox4.Text.ToUpper())
                {
                    encontro = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }

            if (!encontro)
            {
                ListBox4.Items.Add(token);
                direccion = (ListBox4.Items.Count - 1).ToString();
            }
        }

        public void BuscapalReservada()
        {
            int renglon2 = 0;
            direccion = (-1).ToString();
            bool Pr = false; // Agregar esta línea para declarar la variable Pr

            while (!Pr && renglon2 < ListBox3.Items.Count)
            {
                ListBox3.SelectedIndex = renglon2;
                if (token.ToUpper() == ListBox3.Text.ToUpper())
                {
                    Pr = true;
                    direccion = renglon2.ToString();
                }
                renglon2 = renglon2 + 1;
            }
        }
    }
}










