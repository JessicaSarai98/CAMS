using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    public partial class MenuInicio : Form
    {
        public MenuInicio()
        {
            InitializeComponent();
        }
        string orden = "CAMS/orden_rpp.txt";
        string archivo = "parejas.txt";
        preguntas preg = new preguntas();
        System.IO.StreamReader file = null;

        private void MenuInicio_Load(object sender, EventArgs e)
        {
            if (File.Exists(archivo))
            {
                desempateToolStripMenuItem.Enabled = true;
                problemasToolStripMenuItem.Enabled = true;
            }
            else
            {
                desempateToolStripMenuItem.Enabled = false;
                problemasToolStripMenuItem.Enabled = false;
            }
        }
        Form1 Form1;
        preguntas preguntas;
        preguntas2 p;
        private void pasarListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(archivo))
            {
                DialogResult respuesta = MessageBox.Show("Ya existe el archivo del pase de lista, ¿desea generar uno nuevo y " +
                    "eliminar el actual?", "Mensaje", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.OK)
                {
                    if (Form1 == null)
                    {
                        Form1 = new Form1();
                        Form1.setLimite(10);
                        Form1.Owner = this;
                        Form1.button2.Visible = false;
                        Form1.FormClosed += Form1_FormClosed;
                        Form1.Show();
                    }
                    else Form1.Activate();
                }
            }
            else
            {
                if (Form1 == null)
                {
                    Form1 = new Form1();
                    Form1.Owner = this;
                    Form1.FormClosed += Form1_FormClosed;
                    Form1.Show();
                }
                else Form1.Activate();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 = null;
            preguntas = null;
        }

        private void problemasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (preguntas == null)
            {
                preguntas = new preguntas();
                preguntas.Owner = this;
                preguntas.FormClosed += Form1_FormClosed;
                StreamReader lector = File.OpenText(orden);
                preg.participanteB.Text = lector.ReadToEnd();
                preguntas.Show();
            }
            else Form1.Activate();
        }

        private void desempateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Form1 == null || p == null)
            {
                Form1 = new Form1();
                Form1.setLimite(2);
                Form1.Owner = this;
                Form1.FormClosed += Form1_FormClosed;
                Form1.btnLimpiar.Visible = false;
                Form1.button1.Visible = false;
                Form1.btnTerminar.Visible = false;
                Form1.cargarArchivo2();
                Form1.Show();
            }
            else Form1.Activate();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(archivo))
            {
                desempateToolStripMenuItem.Enabled = true;
                problemasToolStripMenuItem.Enabled = true;
            }
            else
            {
                desempateToolStripMenuItem.Enabled = false;
                problemasToolStripMenuItem.Enabled = false;
            }
        }
        //*****     AQUI COMIENZA TODO EL PROCESO PARA HACER LAS ETIQUETAS *****
        //creamos la clase escuela y la de salones
        class NombreEscuelas
        {
            public string nombre { get; set; }
            public int capacidad { get; set; }
            public string[] nombreAlumnos { get; set; }
            //PARA SABER SI TODAVÍA SE ASIGNAN ALUMNOS O YA ESTÁN TODOS.
            public int capacidadAUX { get; set; }
            public int estado { get; set; }
            public double[] capacidadxSalon { get; set; }
        }
        class ListaSalones
        {
            public string id { get; set; }
            public string nombre { get; set; }
            public string filas_columnas { get; set; }
            public int capacidad { get; set; }
            //Para saber si ya está ocupado todo el salón o tiene espacio
            public int capacidadAUX { get; set; }
        }

        // Creamos la lista y las variables
        string linea, filas_columnas, filas, columnas;
        string[] valores;
        int[] AlumnosPorEscuelaPorSalon;
        int capacidadTotal = 0, capacidad, i=0, j=0;
        double auxAlumno;
        List<NombreEscuelas> Escuelas = new List<NombreEscuelas>();
        List<ListaSalones> Salones = new List<ListaSalones>();
        private void generarEtiquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Vemos cuantas líneas tiene el archivo para declarar el arreglo
            System.IO.StreamReader file = new System.IO.StreamReader("Archivos-Salones/Salones.csv");
            file.ReadLine();
            //procesamos cada fila para saber la capacidad del mismo
            while ( (linea = file.ReadLine()) != null)
            {
                valores = linea.Split(',');
                filas_columnas = valores[2] + ";" + valores[3];
                capacidad = Convert.ToInt32(valores[2]) * Convert.ToInt32(valores[3]);
                capacidadTotal += capacidad;
                Salones.Add(new ListaSalones() { id = valores[0], nombre = valores[1], capacidad = capacidad, capacidadAUX = capacidad,
                filas_columnas = filas_columnas});
            }

            //Leemos el archivo "Entrada" para saber cuantos alumnos hay de cada escuela y agruparlos
            file = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
            file.ReadLine();
            //Se lee la siguiente línea y se compara hasta que se termine de leer el archivo
            while ((linea = file.ReadLine()) != null) { 
                valores = linea.Split(',');
                //Agregamos todos los valores del archivo a la lista
                Escuelas.Add(new NombreEscuelas() { nombre = valores[0] });
            }


            //Vemos cuantas escuelas hay en total, cuantos alumnos tienen cada una
            //y van de mayor a menor
            var query = from escuela in Escuelas group escuela  by escuela.nombre into nuevoGrupo
                        orderby nuevoGrupo.Count() descending select nuevoGrupo;
            foreach (var group in query)
            {
                Escuelas[i].nombre = group.Key;
                Escuelas[i].capacidad = Escuelas[i].capacidadAUX = group.Count();
                Escuelas[i].estado = 0;
                Escuelas[i].capacidadxSalon = new double[Salones.Count()];
                for (j = 0; j < Salones.Count(); j++) {
                    auxAlumno = Math.Round((double)Escuelas[i].capacidad / capacidadTotal *Salones[j].capacidad );
                    //si todavía hay alumnos de esa escuela, se asigna
                    if (Escuelas[i].capacidadAUX >= auxAlumno)
                    {
                        //Preguntamos si todavía hay espacio en ese salón
                        if (Salones[j].capacidadAUX > auxAlumno)
                        {
                            //preguntamos si la escuela sólo tiene un participante
                            if (Escuelas[i].capacidad == 1)
                            {//de ser así, se asigna al primer salón y se pasa al siguiente
                                Escuelas[i].capacidadxSalon[j] = 1;
                                Salones[j].capacidadAUX = Salones[j].capacidadAUX - 1;
                                capacidadTotal = capacidadTotal - 1 ;
                                Escuelas[i].capacidadAUX = Escuelas[i].capacidadAUX - 1;
                                break;
                            }
                            Escuelas[i].capacidadxSalon[j] = auxAlumno;
                            capacidadTotal = capacidadTotal - (int)auxAlumno;
                            Escuelas[i].capacidadAUX = Escuelas[i].capacidadAUX - (int)auxAlumno;
                            Salones[j].capacidadAUX = Salones[j].capacidadAUX - (int)auxAlumno;
                            //MessageBox.Show("Escuela "+Escuelas[i].capacidad+((double)Salones[j].capacidad / capacidadTotal * Escuelas[i].capacidad).ToString());
                        }
                    }
                }
                i++;
            }

                    AlumnosPorEscuelaPorSalon = new int[i];
            //prueba para ver que todo este saliendo de acuerdo al plan
            tablaAlgoritmo tabla = new tablaAlgoritmo();
            for (i=0; i < Escuelas.Count(); i++)
            {
                if (Escuelas[i].capacidadxSalon != null)
                {
                    capacidadTotal = i;
                    tabla.dataGridView1.Rows.Add(Escuelas[i].nombre, "Partcipantes: "+Escuelas[i].capacidad,
                    Escuelas[i].capacidadxSalon[0] + "/" + Escuelas[i].capacidadxSalon[1] + "/" +
                    Escuelas[i].capacidadxSalon[2] + "/" + Escuelas[i].capacidadxSalon[3] + "/" +
                    Escuelas[i].capacidadxSalon[4] + "/" + Escuelas[i].capacidadxSalon[5] + "/" +
                    Escuelas[i].capacidadxSalon[6]);
                } else  break;
            }
            tabla.Show();
            // Inicializamos el documento PDF
            Document doc = new Document(PageSize.LETTER);
            PdfWriter.GetInstance(doc, new FileStream("Archivos-Salones/ListaSalones.pdf", FileMode.Create)); // asignamos el nombre de archivo hola.pdf
            // Importante Abrir el documento
            doc.Open();
            // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
            Paragraph title;
            

            //asignamos lugares en los salones, primero los separamos por salon 
            //son 7 salones
            for (i=0; i < Salones.Count(); i++ )
            {
                //checamos cuantas filas y columnas tiene para saber quien puede ir
                filas = Salones[i].filas_columnas.Split(';')[0];
                columnas = Salones[i].filas_columnas.Split(';')[1];
                int capac = Int32.Parse(filas)*Int32.Parse(columnas);
                //MessageBox.Show("%d"+capac);
                title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLACK);
                title.Add("Lista de alumnos del salón "+Salones[i].nombre);
                doc.Add(title);
                // Agregamos un parrafo vacio como separacion.
                doc.Add(new Paragraph(" "));

                // Empezamos a crear la tabla, definimos una tabla de 6 columnas
                PdfPTable table = new PdfPTable(3);
                // Comenzamos a llenar las filas
                table.AddCell("Folio");
                table.AddCell("Nombre");
                table.AddCell("lugar");
                //asignamos cuantos alumnos hay de cada escuela en cada salón
                //for (j=0; j < AlumnosPorEscuelaPorSalon.Length; j++)
                //{
                //    AlumnosPorEscuelaPorSalon[j] = (int)Escuelas[j].capacidadxSalon[i];
                //    //MessageBox.Show("Capacidad: %i"+AlumnosPorEscuelaPorSalon[j]);
                //}
                //for (j=0; j < AlumnosPorEscuelaPorSalon.Length; j++)
                //{
                int k = 0;
                    //si tiene al menos un alumno se ingres
                //if (capac!= 0 && AlumnosPorEscuelaPorSalon[j] !=0)
                //    {
                        //leemos de nuevo el archivo para acceder a la información
          
               
                file = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
                file.ReadLine();
                while ((linea = file.ReadLine()) != null)
                {
                    for (j = 0; j < AlumnosPorEscuelaPorSalon.Length; j++)
                    {
                        AlumnosPorEscuelaPorSalon[j] = (int)Escuelas[j].capacidadxSalon[i];
                        if(capac!=0 && AlumnosPorEscuelaPorSalon[j] != 0) { 

                        //MessageBox.Show(valores[0] + "/"+ Escuelas[j].nombre+ "  -valores[0]");
                             valores = linea.Split(',');
                            if (valores[0] == Escuelas[j].nombre)
                            {
                                table.AddCell(valores[1]);
                                table.AddCell(valores[2]);
                                table.AddCell((j + k + 1).ToString());
                                k++;
                                capac--;

                                AlumnosPorEscuelaPorSalon[j] = AlumnosPorEscuelaPorSalon[j] - 1;
                                //MessageBox.Show(valores[0] + valores[1] + valores[2]);

                                break;
                            }
                        }
                    }
                }

                 //   }
               // }
                

                // Agregamos un parrafo vacio como separacion.
                doc.Add(new Paragraph(" "));
                // Agregamos la tabla al documento
                doc.Add(table);
            }


            // Ceramos el documento
            doc.Close();
        }
    }
}
