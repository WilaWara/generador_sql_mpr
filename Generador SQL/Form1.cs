using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Generador_SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog cuadro_busqueda = new OpenFileDialog();
            cuadro_busqueda.InitialDirectory = @"Documentos";
            cuadro_busqueda.Title = "Buscar facturas...";
            cuadro_busqueda.Filter = "DAT (*.dat)|*.dat";
            
            if (cuadro_busqueda.ShowDialog() == DialogResult.OK)
            {
                StreamReader objInput = new StreamReader(cuadro_busqueda.FileName, System.Text.Encoding.Default);
                string contents = objInput.ReadToEnd().Trim();

                string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
                string consulta = "";


                textBox1.Text = "---------------------Marcados: " + cuadro_busqueda.FileName + "---------------------";
                //Console.WriteLine("-------------------------Marcados: " + cuadro_busqueda.FileName + "-------------------------");
                for (int i = 0; i < split.Length; i += 7)
                {
                    string fecha_hora = split[i + 1] + " " + split[i + 2];
                    TimeSpan t = Convert.ToDateTime(fecha_hora) - new DateTime(1970, 1, 1);
                    int secondsSinceEpoch = (int)t.TotalSeconds;

                    consulta = "INSERT INTO dbo.BS (UserID, eventTime, eventCode, tnaEvent, Code, IP) VALUES (" + split[i] + ", " + secondsSinceEpoch + ", 55, 0, '" + split[i] + "', 'Imp. manual')";
                    //Console.WriteLine(consulta);
                    textBox1.Text += consulta + "\r\n";
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader objInput = new StreamReader("D:\\asistencias_completas.dat", System.Text.Encoding.Default);
            string contents = objInput.ReadToEnd().Trim();

            string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
            string consulta = "";

            string fecha = "";
            string hora = "";

            string timestamp = "";
            string[] arreglo_timestamp;

            int nro_documento = 0;

            for (int i = 0; i < split.Length; i += 2)
            {
                /*********************************************/
                double tiempo_epoch = Int64.Parse(split[i]);
                DateTime inicio_epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                inicio_epoch = inicio_epoch.AddSeconds(tiempo_epoch); // Agregando los segundos

                // Dividiendo el timestamp en fecha y hora separados
                timestamp = inicio_epoch.ToString();
                arreglo_timestamp = Regex.Split(timestamp, "\\s+", RegexOptions.None);
                fecha = arreglo_timestamp[0];
                hora = arreglo_timestamp[1];

                // Para numero de documento
                nro_documento = Int32.Parse(split[i + 1]);

                consulta = "INSERT INTO reloj.marcados (fecha, hora, ipmarcador_biometrico, nro_documento, estado_registro, fecha_creacion, host_creacion, usuario_creacion, idmarcador) VALUES ('" + fecha + "', '" + hora + "', '192.168.15.241', " + nro_documento + ", true, '2020-10-02 09:00:00', '192.168.15.241', 'aalvarez', 7);";
                Console.WriteLine(consulta);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamReader input_doc1 = new StreamReader("H:\\organigrama_detalle.dat", System.Text.Encoding.Default);
            string contenido_doc1 = input_doc1.ReadToEnd().Trim();
            string[] lineas_doc1 = contenido_doc1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            StreamReader input_doc2 = new StreamReader("H:\\lista_empleados.dat", System.Text.Encoding.Default);
            string contenido_doc2 = input_doc2.ReadToEnd().Trim();
            string[] lineas_doc2 = contenido_doc2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            string temp1, temp2;
            string consulta = "";

            for (int i = 0; i < lineas_doc1.Length; i++)
            {
                string[] valores_lineas_doc1 = lineas_doc1[i].Split(new string[] { " t " }, StringSplitOptions.None);
                temp1 = valores_lineas_doc1[1].ToLower();

                for (int u = 0; u < lineas_doc2.Length; u++)
                {
                    string[] valores_lineas_doc2 = lineas_doc2[u].Split(new string[] { " t " }, StringSplitOptions.None);
                    temp2 = valores_lineas_doc2[1].ToLower();

                    //Console.WriteLine("temp1: " + temp1 + " - temp2: " + temp2);
                    if (temp1 == temp2)
                    {                        
                        consulta = "INSERT INTO directorio.empleado_organigrama_detalle (idpersona, idorganigramadetalle) VALUES (" + valores_lineas_doc2[0] + ", " + valores_lineas_doc1[0] + ");";
                        Console.WriteLine(consulta);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StreamReader objInput = new StreamReader("D:\\empleados.dat", System.Text.Encoding.Default);
            string contents = objInput.ReadToEnd().Trim();
            string[] lines = contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            Console.WriteLine(lines.Length);

            /************************************************************/
            for (int i = 0; i < lines.Length; i++)
            {
                string[] split = Regex.Split(lines[i], "\\s+", RegexOptions.None);
                string consulta = "";

                string[] arreglo_complemento;
                string complemento = "";
                string correo_electronico = "><";
                //string fecha_creacion = "2020-09-29 09:00:00";
                int genero = 0;
                int nro_documento = 0;
                string primer_apellido = "";
                string segundo_apellido = "";
                string primer_nombre = "";
                string segundo_nombre = "";

                if (split[0].Contains("-") == true)
                {
                    arreglo_complemento = split[0].Split('-');
                    complemento = arreglo_complemento[1];
                }

                if (split[0 + 1] == "M")
                {
                    genero = 1;
                }
                else if (split[0 + 1] == "F")
                {
                    genero = 2;
                }

                nro_documento = int.Parse(split[0 + 2]);
                primer_apellido = split[0 + 3];
                if (split[0 + 4] != "1" && split[0 + 4] != "2")
                {
                    segundo_apellido = split[0 + 4];
                }

                if (split[0 + 4] == "1" || split[0 + 4] == "2")
                {
                    primer_nombre = split[0 + 5];
                    try
                    {
                        segundo_nombre = split[0 + 6];
                    }
                    catch (Exception ex)
                    {
                        // Hacer nada
                    }
                }

                if (split[0 + 5] == "1" || split[0 + 5] == "2")
                {
                    primer_nombre = split[0 + 6];
                    try
                    {
                        segundo_nombre = split[0 + 7];
                    }
                    catch (Exception ex)
                    {
                        // Hacer nada
                    }
                }

                consulta = "INSERT INTO nucleo.persona(complemento, correo_electronico, genero, nombres, nro_documento, primer_apellido, segundo_apellido) VALUES ('" + complemento + "', '" + correo_electronico + "', " + genero + ", '" + primer_nombre + " " + segundo_nombre + "', " + nro_documento + ", '" + primer_apellido + "', '" + segundo_apellido + "');";
                if (consulta.Contains("INSERT INTO") == true)
                {
                    Console.WriteLine(consulta);
                    continue;
                }
            }
        }

        private void Empleados_Click(object sender, EventArgs e)
        {
            StreamReader input_doc1 = new StreamReader("D:\\nrodocumento_fechanac.dat", System.Text.Encoding.Default);
            string contenido_doc1 = input_doc1.ReadToEnd().Trim();
            string[] lineas_doc1 = contenido_doc1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            StreamReader input_doc2 = new StreamReader("D:\\idpersona_nrodocumento.dat", System.Text.Encoding.Default);
            string contenido_doc2 = input_doc2.ReadToEnd().Trim();
            string[] lineas_doc2 = contenido_doc2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            string temp1, temp2;
            string consulta = "";

            for (int i = 0; i < lineas_doc1.Length; i++)
            {
                /*string[] valores_lineas_doc1 = lineas_doc1[i].Split(new string[] { " t " }, StringSplitOptions.None);
                temp1 = valores_lineas_doc1[1].ToLower();*/

                string[] valores_lineas_doc1 = Regex.Split(lineas_doc1[i], "\\s+", RegexOptions.None);
                temp1 = valores_lineas_doc1[0].ToLower();
                
                for (int u = 0; u < lineas_doc2.Length; u++)
                {
                    /*string[] valores_lineas_doc2 = lineas_doc2[u].Split(new string[] { " t " }, StringSplitOptions.None);
                    temp2 = valores_lineas_doc2[1].ToLower();*/

                    string[] valores_lineas_doc2 = Regex.Split(lineas_doc2[u], "\\s+", RegexOptions.None);
                    temp2 = valores_lineas_doc2[1].ToLower();



                    if (temp1 == temp2)
                    {
                        consulta = "INSERT INTO rrhh.empleado (celular, direccion, fecha_nacimiento, telefono, idpersona) VALUES (0, '', '" + valores_lineas_doc1[1] + "', 0, " + valores_lineas_doc2[0] + ");";
                        Console.WriteLine(consulta);
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StreamReader input_doc1 = new StreamReader("D:\\biometrica_empleado_antiguo.dat", System.Text.Encoding.Default);
            string contenido_doc1 = input_doc1.ReadToEnd().Trim();
            string[] lineas_doc1 = contenido_doc1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            StreamReader input_doc2 = new StreamReader("D:\\idpersona_nrodocumento.dat", System.Text.Encoding.Default);
            string contenido_doc2 = input_doc2.ReadToEnd().Trim();
            string[] lineas_doc2 = contenido_doc2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            int temp1, temp2;
            string consulta = "";

            for (int i = 0; i < lineas_doc1.Length; i++)
            {
                /*string[] valores_lineas_doc1 = lineas_doc1[i].Split(new string[] { " t " }, StringSplitOptions.None);
                temp1 = valores_lineas_doc1[1].ToLower();*/

                /*string[] valores_lineas_doc1 = Regex.Split(lineas_doc1[i], "\\s+", RegexOptions.None);
                temp1 = valores_lineas_doc1[0].ToLower();*/

                string[] auxiliar1 = Regex.Split(lineas_doc1[i], "'aalvarez',", RegexOptions.None);
                string auxiliar2 = auxiliar1[1].Replace(");", "");

                string[] valores_lineas_doc1 = Regex.Split(auxiliar2, ",", RegexOptions.None);
                temp1 = int.Parse(valores_lineas_doc1[1]);

                //5, 2384130);

                for (int u = 0; u < lineas_doc2.Length; u++)
                {
                    /*string[] valores_lineas_doc2 = lineas_doc2[u].Split(new string[] { " t " }, StringSplitOptions.None);
                    temp2 = valores_lineas_doc2[1].ToLower();*/

                    string[] valores_lineas_doc2 = Regex.Split(lineas_doc2[u], "\\s+", RegexOptions.None);
                    temp2 = int.Parse(valores_lineas_doc2[1]);
                    
                    if (temp1 == temp2)
                    {
                        consulta = auxiliar1[0] + "'aalvarez', " + valores_lineas_doc2[0] + ", " + temp2.ToString() + ");";
                        Console.WriteLine(consulta);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog cuadro_busqueda = new OpenFileDialog();
            cuadro_busqueda.InitialDirectory = @"Documentos";
            cuadro_busqueda.Title = "Buscar archivo...";
            cuadro_busqueda.Filter = "DAT (*.dat)|*.dat";

            if (cuadro_busqueda.ShowDialog() == DialogResult.OK)
            {
                StreamReader objInput = new StreamReader(cuadro_busqueda.FileName, System.Text.Encoding.Default);
                string contents = objInput.ReadToEnd().Trim();

                string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
                string consulta = "";

                Console.WriteLine("-------------------------Marcados: " + cuadro_busqueda.FileName + "-------------------------");
                for (int i = 0; i < split.Length; i += 10)
                {
                    string fecha_hora = split[i + 8] + " " + split[i + 9];
                    TimeSpan t = Convert.ToDateTime(fecha_hora) - new DateTime(1970, 1, 1);
                    int secondsSinceEpoch = (int)t.TotalSeconds;

                    consulta = "INSERT INTO dbo.BS (UserID, eventTime, eventCode, tnaEvent, Code, IP) VALUES (" + split[i + 3] + ", " + secondsSinceEpoch + ", 55, 0, '" + split[i + 3] + "', 'R Illimani')";
                    Console.WriteLine(consulta);
                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(textBox1.Text);

                MessageBox.Show("Copiado al portapapeles");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
