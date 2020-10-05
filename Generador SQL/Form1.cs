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

namespace Adaptador_de_marcados_biometricos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader objInput = new StreamReader("H:\\2425-09.dat", System.Text.Encoding.Default);
            string contents = objInput.ReadToEnd().Trim();

            string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
            string consulta = "";

            for (int i = 0; i<split.Length; i+=7)
            {
                string fecha_hora = split[i + 1] + " " + split[i + 2];
                TimeSpan t = Convert.ToDateTime(fecha_hora) - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                consulta = "INSERT INTO dbo.BS (UserID, eventTime, eventCode, tnaEvent, Code, IP) VALUES (" + split[i] + ", " + secondsSinceEpoch  + ", 55, 0, '" + split[i] + "', 'Imp. manual')";
                Console.WriteLine(consulta);
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
            /*StreamReader objInput = new StreamReader("H:\\empleados.dat", System.Text.Encoding.Default);
            string contents = objInput.ReadToEnd().Trim();
            string[] lines = contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            Console.WriteLine(lines.Length);

            string[] split = System.Text.RegularExpressions.Regex.Split(contents, "\\s+", RegexOptions.None);
            string consulta = "";

            for (int i = 0; i < split.Length; i += 7)
            {
                string[] arreglo_complemento;
                string complemento = "";
                string correo_electronico = "><";
                string fecha_creacion = "2020-09-29 09:00:00";
                int genero = 0;
                int nro_documento = 0;
                string primer_apellido = "";
                string segundo_apellido = "";
                string primer_nombre = "";
                string segundo_nombre = "";

                if (split[i].Contains("-") == true)
                {
                    arreglo_complemento = split[i].Split('-');
                    complemento = arreglo_complemento[1];
                }

                if (split[i + 1] == "M")
                {
                    genero = 1;
                }
                else if (split[i + 1] == "F")
                {
                    genero = 2;
                }
                nro_documento = Int32.Parse(split[i + 2]);
                primer_apellido = split[i + 3];
                if (split[i + 4] != "1" && split[i + 4] != "2")
                {
                    segundo_apellido = split[i + 4];
                }

                if (split[i + 4] == "1" || split[i + 4] == "2")
                {
                    primer_nombre = split[i + 5];
                    try
                    {
                        segundo_nombre = split[i + 6];
                    }
                    catch (Exception ex)
                    {
                        // Hacer nada
                    }
                }

                if (split[i + 5] == "1" || split[i + 5] == "2")
                {
                    primer_nombre = split[i + 6];
                    try
                    {
                        segundo_nombre = split[i + 7];
                    }
                    catch (Exception ex)
                    {
                        // Hacer nada
                    }
                }

                consulta = "INSERT INTO directorio.persona(complemento, correoelectronico, estado_registro, fecha_creacion, genero, host_creacion, nrodocumento, primerapellido, primernombre, segundoapellido, segundonombre, usuario_creacion, id_directorio_telefonico) VALUES ('" + complemento + "', '" + correo_electronico + "', true, '" + fecha_creacion + "', " + genero + ", '127.0.0.1', " + nro_documento + ", '" + primer_apellido + "', '" + primer_nombre + "', '" + segundo_apellido + "', '" + segundo_nombre + "', 'aalvarez', 0);";
                Console.WriteLine(consulta);

                continue;
            }*/

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
    }
}
