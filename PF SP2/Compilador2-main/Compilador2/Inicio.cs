using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Compilador2
{
    public partial class Inicio : Form
    {
        private string idUsuario;

        public Inicio()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton1.Checked = true;
            label1.Visible = true;
            label2.Visible = true;
            label5.Visible = false;
            textBox1.Visible = true;
            textBox2.Visible = true;
            comboBox1.Visible = false;
            button1.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label3.Visible = false;
                label4.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
            }
            else{
                label3.Visible = true;
                label4.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string contraseña = encriptar(textBox2.Text);
            string correo = textBox3.Text;
            string num = textBox4.Text;
            BD inicio = new BD();
            if (radioButton1.Checked)
            {
                DataTable datosUsuario = inicio.consulta("select idUsuario from usuarios where nombre='" + usuario + "' and contrasena='" + contraseña + "'");
                if (datosUsuario.Rows.Count !=0)
                {
                    radioButton1.Visible = false;
                    radioButton2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = true;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    textBox3.Visible = false;
                    textBox4.Visible = false;
                    comboBox1.Visible = true;
                    button1.Visible = false;
                    button2.Visible = true;
                    button3.Visible = true;
                    idUsuario = datosUsuario.Rows[0]["idUsuario"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontro el usuario");
                }
            }
            else
            {
                if(inicio.consulta("select nombre from usuarios where nombre='" + usuario + "'").Rows.Count != 0)
                {
                    MessageBox.Show("Ese usuario ya existe");
                }
                else
                {
                    inicio.consulta("insert into usuarios(nombre,contrasena,correo,numero) values ('"+usuario+"','"+contraseña+"','"+correo+"','"+num+"')");
                    MessageBox.Show("Se creo el usuario");
                }
            }

            
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            BD llenar = new BD();
            foreach (DataRow lenguajes in llenar.consulta("select nombre from lenguajes").Rows)
            {
                comboBox1.Items.Add(lenguajes["nombre"]);

            }
            comboBox1.SelectedIndex = 0;

        }

        public static string encriptar(string Cadena)
        {
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            byte[] vectoBytes = System.Text.Encoding.UTF8.GetBytes(Cadena);
            byte[] inArray = SHA1.ComputeHash(vectoBytes);
            SHA1.Clear();
            return Convert.ToBase64String(inArray);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 compilador = new Form1();
            compilador.IdLenguaje = (comboBox1.SelectedIndex + 1).ToString();
            compilador.IdUsuario = idUsuario;
            compilador.Visible = true;
            Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
