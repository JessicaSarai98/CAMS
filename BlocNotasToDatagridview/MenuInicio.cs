using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.fonts;
using System;
using System.Collections;
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
        class AlumnoInfo
        {
            private String escuela;
            private String nombre;
            private String matricula;
            
            public AlumnoInfo(String escuela, String nombre, String matricula)
            {
                this.escuela = escuela;
                this.nombre = nombre;
                this.matricula = matricula;
            }

            public String getEscuela()
            {
                return this.escuela;
            }

            public String getNombre()
            {
                return this.nombre;
            }
            public String getMatricula()
            {
                return this.matricula;
            }
        }
        class AlumnoInfo_Asiento
        {
            private AlumnoInfo alumno;
            private int asiento;
            public AlumnoInfo_Asiento(AlumnoInfo alumno, int asiento)
            {
                this.alumno = alumno;
                this.asiento = asiento;
            }

            public AlumnoInfo getAlumnoInfo()
            {
                return alumno;
            }

            public int getAsiento()
            {
                return asiento;
            }
        }

        // Creamos la lista y las variables
        string linea, filas_columnas, filas, columnas;
        string[] valores;
        int[] AlumnosPorEscuelaPorSalon;
        int capacidadTotal = 0, capacidad, i = 0, j = 0;
        double auxAlumno;
        List<NombreEscuelas> Escuelas = new List<NombreEscuelas>();
        List<ListaSalones> Salones = new List<ListaSalones>();
        int maxDeSalones = 64;//Variables temporal en lo que se calcula el verdadero numero de salones
        private void generarEtiquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Vemos cuantas líneas tiene el archivo para declarar el arreglo
            System.IO.StreamReader file = new System.IO.StreamReader("Archivos-Salones/Salones.csv");
            file.ReadLine();
            //procesamos cada fila para saber la capacidad del mismo
            int numSalones = 0;
            while ((linea = file.ReadLine()) != null)
            {
                valores = linea.Split(',');
                filas_columnas = valores[2] + ";" + valores[3];
                capacidad = Convert.ToInt32(valores[2]) * Convert.ToInt32(valores[3]);
                capacidadTotal += capacidad;
                Salones.Add(new ListaSalones()
                {
                    id = valores[0],
                    nombre = valores[1],
                    capacidad = capacidad,
                    capacidadAUX = capacidad,
                    filas_columnas = filas_columnas
                });
                numSalones++;//CALCULO DE SALONES
            }

            //Leemos el archivo "Entrada" para saber cuantos alumnos hay de cada escuela y agruparlos
            file = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
            file.ReadLine();
            //Se lee la siguiente línea y se compara hasta que se termine de leer el archivo
            while ((linea = file.ReadLine()) != null)
            {
                valores = linea.Split(',');
                //Agregamos todos los valores del archivo a la lista
                Escuelas.Add(new NombreEscuelas() { nombre = valores[0] });
            }


            //Vemos cuantas escuelas hay en total, cuantos alumnos tienen cada una
            //y van de mayor a menor
            var query = from escuela in Escuelas
                        group escuela by escuela.nombre into nuevoGrupo
                        orderby nuevoGrupo.Count() descending
                        select nuevoGrupo;
            foreach (var group in query)
            {
                Escuelas[i].nombre = group.Key;
                Escuelas[i].capacidad = Escuelas[i].capacidadAUX = group.Count();
                Escuelas[i].estado = 0;
                Escuelas[i].capacidadxSalon = new double[Salones.Count()];
                for (j = 0; j < Salones.Count(); j++)
                {
                    auxAlumno = Math.Round((double)Escuelas[i].capacidad / capacidadTotal * Salones[j].capacidad);
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
                                capacidadTotal = capacidadTotal - 1;
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
            int cantidadDeEscuelas = 0;
            for (i = 0; i < Escuelas.Count(); i++)
            {
                if (Escuelas[i].capacidadxSalon != null)
                {
                    capacidadTotal = i;
                    tabla.dataGridView1.Rows.Add(Escuelas[i].nombre, "Partcipantes: " + Escuelas[i].capacidad,
                    Escuelas[i].capacidadxSalon[0] + "/" + Escuelas[i].capacidadxSalon[1] + "/" +
                    Escuelas[i].capacidadxSalon[2] + "/" + Escuelas[i].capacidadxSalon[3] + "/" +
                    Escuelas[i].capacidadxSalon[4] + "/" + Escuelas[i].capacidadxSalon[5] + "/" +
                    Escuelas[i].capacidadxSalon[6]);
                    cantidadDeEscuelas++;
                }
                else break;
            }
            tabla.Show();
            // Inicializamos el documento PDF
            Document doc = new Document(PageSize.LETTER);
            PdfWriter.GetInstance(doc, new FileStream("Archivos-Salones/ListaSalones.pdf", FileMode.Create)); // asignamos el nombre de archivo hola.pdf
            // Importante Abrir el documento
            doc.Open();
            // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
            Paragraph title,tit, titulo;
            AlumnoInfo[,] listaDeAlumnosPorSalon = new AlumnoInfo[Salones.Count(), maxDeSalones];

            //FUNCION DE PROGRAMA-----------------------------------------------------------------------------------------------------
            //LISTADO DE ESTUDIANTES POR SALON CON LA CANTIDAD CORRESPONDIENTE-----------------------------------------------------------------------------------------------------
            for (int m = 0; m < cantidadDeEscuelas; m++)
            {
                System.IO.StreamReader listaAlumnos = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
                listaAlumnos.ReadLine();
                for (int n = 0; n < numSalones; n++)
                {
                    int alumnosEnLalistaDelSalon = 0;
                    //Se valida si hay alumnos antes para ingresar después del último
                    while (listaDeAlumnosPorSalon[n, alumnosEnLalistaDelSalon] != null)
                    {
                        alumnosEnLalistaDelSalon++;
                    }
                    //Variable que indica cuantos de esa escuela ya han sido seleccionados.
                    int seleccionados = 0;
                    while ((Escuelas[m].capacidadxSalon[n] != 0) && ((linea = listaAlumnos.ReadLine()) != null))
                    {
                        valores = linea.Split(',');
                        if (valores[0] == Escuelas[m].nombre)
                        {
                            listaDeAlumnosPorSalon[n, seleccionados + alumnosEnLalistaDelSalon] = new AlumnoInfo(valores[0], valores[2], valores[1]); ;
                            seleccionados++;
                        }
                        if (Escuelas[m].capacidadxSalon[n] == seleccionados)
                        {
                            break;
                        }
                    }
                }
            }
            //ALGORTIMO PARA ACOMODAR A LOS ESTUDIANTES SIN QUE SEAN DE LA MISMA ESCUELA----------------------------------------------
            //Array que contiene la informacion Folio/AlumnoNombre/AsientoEnElSalon
            AlumnoInfo_Asiento[,] asientosInfo = new AlumnoInfo_Asiento[Salones.Count(), maxDeSalones];
            System.IO.StreamReader fileSalon = new System.IO.StreamReader("Archivos-Salones/Salones.csv");
            fileSalon.ReadLine();
            //SALONES--------------------------------------------
            for (int n = 0; n < numSalones; n++)
            {
                //MessageBox.Show("Salon " + n);
                int alumnoPosicion = 0;//Posicion en la lista de alumnos del salon
                int asientosPosicion = 0;//Posicion en los asientos del salon
                linea = fileSalon.ReadLine();
                String[] salonesInfo = linea.Split(',');

                int cantidadDeAlumnosEnElSalon = 0;//Variable para saber cuantos alumnos hay en el salon actual.
                while (listaDeAlumnosPorSalon[n, alumnoPosicion] != null)
                {
                    cantidadDeAlumnosEnElSalon++;
                    alumnoPosicion++;
                }
                alumnoPosicion = 0;
                int numDeVuletasRepetidas = 0;
                int auxAsientosPosicion = 0;
                int posicionAux = 0;
                ArrayList posicionesSaltadas = new ArrayList();

                while (asientosPosicion != cantidadDeAlumnosEnElSalon)
                {
                    int[] puntosCardinales;
                    bool auxUsado = false;
                    //Obtenemos lo puntos cardinales a la posicion del asiento(arriba, derecha, abajo, izquierda)
                    if (posicionesSaltadas.Count == 0 && auxAsientosPosicion == 0)
                    {
                        puntosCardinales = crearPuntosCardinales(asientosPosicion, Convert.ToInt32(salonesInfo[3]));
                    }
                    else
                    {
                        puntosCardinales = crearPuntosCardinales((int)posicionesSaltadas[0] + auxAsientosPosicion, Convert.ToInt32(salonesInfo[3]));
                        posicionAux = (int)posicionesSaltadas[0] + auxAsientosPosicion;
                        auxUsado = true;
                    }

                    //Obtenermos si estan ocupados, si lo están saber de que escuela es
                    String[] nombresDeEscuelasCardinales = new string[4];
                    //SE ASIGNAN LOS NOMBRES DE LAS ESCUELAS CARDINALES AL ASIENTO------------------------------------------------------------------------
                    for (int m = 0; m < 4; m++)
                    {
                        if (puntosCardinales[m] >= 0)//Indica que el asiento existe
                        {
                            if (asientosInfo[n, puntosCardinales[m]] != null)//Hay alguien en los puntos cardinales
                            {
                                nombresDeEscuelasCardinales[m] = asientosInfo[n, puntosCardinales[m]].getAlumnoInfo().getEscuela();
                            }
                            else
                            {
                                nombresDeEscuelasCardinales[m] = " ";
                            }
                            if (m == 1 && puntosCardinales[m] > 0 && ((puntosCardinales[m]) % Convert.ToInt32(salonesInfo[3]) == 0))
                            {
                                nombresDeEscuelasCardinales[m] = " ";
                            }
                            if (m == 3 && (puntosCardinales[m] + 1) % Convert.ToInt32(salonesInfo[3]) == 0)
                            {
                                nombresDeEscuelasCardinales[m] = " ";
                            }
                        }
                        else//El asiento no existe
                        {
                            nombresDeEscuelasCardinales[m] = " ";
                        }
                    }

                    //ASIGNACIÓN DE LOS ASIENTOS----------------------------------------------------------------------------------------------
                    //SE VALIDAN PRIMERO SI SON DISTINTAS ESCUELAS LAS CARDINALES------------------------------------------------------------- 
                    int validaciones = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        if (sonDistintaEscuela(nombresDeEscuelasCardinales[k], listaDeAlumnosPorSalon[n, alumnoPosicion].getEscuela()))
                        {
                            validaciones++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (validaciones == 4)
                    //LAS ESCUELAS CARDINALES SON DISTINTAS, AHORA VER SI NO FUE ASIGNADO CON ANTERIORIDAD------------------------------------
                    {
                        int validacionEnLista = 0;
                        for (int k = 0; k < asientosPosicion; k++)//asientosPosicion
                        {
                            if (listaDeAlumnosPorSalon[n, alumnoPosicion].getNombre() != asientosInfo[n, k].getAlumnoInfo().getNombre())
                            {
                                validacionEnLista++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (auxUsado)
                        {
                            for (int k = asientosPosicion; k < posicionAux; k++)
                            {
                                if (asientosInfo[n, k] == null)
                                {
                                    if (k == posicionAux)
                                    {
                                        validacionEnLista++;
                                    }
                                    break;
                                }
                                if (listaDeAlumnosPorSalon[n, alumnoPosicion].getNombre() != asientosInfo[n, k].getAlumnoInfo().getNombre())
                                {
                                    validacionEnLista++;
                                }
                            }
                        }
                        if (validacionEnLista == asientosPosicion)
                        {
                            if (auxUsado)
                            {
                                if (auxAsientosPosicion != 0)
                                {
                                    // MessageBox.Show("Agregando con salto " + auxAsientosPosicion);
                                    asientosInfo[n, (int)posicionesSaltadas[0] + auxAsientosPosicion] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], ((int)posicionesSaltadas[0] + auxAsientosPosicion + 1));
                                    auxAsientosPosicion = 0;
                                    asientosPosicion++;
                                    numDeVuletasRepetidas = 0;
                                }
                                else
                                {
                                    //MessageBox.Show("Agregando en la posicion saltada");
                                    asientosInfo[n, (int)posicionesSaltadas[0]] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], ((int)posicionesSaltadas[0] + 1));
                                    posicionesSaltadas.Remove(posicionesSaltadas[0]);
                                    asientosPosicion++;
                                    numDeVuletasRepetidas = 0;
                                }
                            }
                            else
                            {
                                asientosInfo[n, asientosPosicion] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], (asientosPosicion + 1));
                                //MessageBox.Show("Se ha agregado a " + asientosInfo[n, asientosPosicion].getAlumnoInfo().getNombre());
                                asientosPosicion++;
                                numDeVuletasRepetidas = 0;
                            }
                        }
                    }

                    alumnoPosicion++;
                    //Se reinicia la lista para que se recorra de nuevo
                    if (alumnoPosicion == cantidadDeAlumnosEnElSalon)
                    {
                        numDeVuletasRepetidas++;
                        alumnoPosicion = 0;
                        if (n == 4 && asientosPosicion == 23)
                        {
                        }
                    }
                    if (numDeVuletasRepetidas > 1)
                    {
                        auxAsientosPosicion++;
                        //Se agrega la posicion saltada a un array
                        posicionesSaltadas.Add(asientosPosicion);
                        numDeVuletasRepetidas = 0;
                    }
                }
            }
            //CREANDO LA LISTA DE ESTUDIANTES POR SALON(PDF)--------------------------------------------------------------------------
            for (int n = 0; n < numSalones; n++)
            {
                filas = Salones[n].filas_columnas.Split(';')[0];
                columnas = Salones[n].filas_columnas.Split(';')[1];
                int capac = Int32.Parse(filas) * Int32.Parse(columnas);
                title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLACK);

                tit = new Paragraph();
                tit.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLACK);

                titulo = new Paragraph();
                titulo.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLACK);

                title.Add("Lista de alumnos del salón " + Salones[n].nombre);
                doc.Add(title);
                doc.Add(new Paragraph(" "));
                PdfPTable table = new PdfPTable(3);

                table.AddCell("Folio");
                table.AddCell("Nombre");
                table.AddCell("Lugar");

                

                PdfPTable eti = new PdfPTable(7);
             
                PdfPTable etiq = new PdfPTable(3);
                PdfPCell salon = new PdfPCell(new Phrase("PIZARRA"));
                salon.Colspan = 7;
                salon.HorizontalAlignment = 1;
                eti.AddCell(salon);
                etiq.HorizontalAlignment = 1;

                int alumnoPosicion = 0;
                while (listaDeAlumnosPorSalon[n, alumnoPosicion] != null)
                {
                    alumnoPosicion++;
                }
                int alumnos = alumnoPosicion;
                int saltosAsientos = 0;
                for (int p = 0; p < alumnos; p++)
                {
                    while (asientosInfo[n, p + saltosAsientos] == null)
                    {
                        saltosAsientos++;
                    }
                    table.AddCell(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getMatricula());
                    table.AddCell(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getNombre());
                    table.AddCell("" + asientosInfo[n, p + saltosAsientos].getAsiento());
                    //Chunk lug = new Chunk(" " + asientosInfo[n, p + saltosAsientos].getAsiento());
                    eti.AddCell(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getMatricula());
                    //lug.SetTextRise(7);
                    //eti.AddCell()
                    etiq.AddCell(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getMatricula());
                    alumnoPosicion++;
                }

               

                doc.Add(table);
                doc.Add(new Paragraph(" "));
                doc.NewPage(); 
                tit.Add(" " + Salones[n].nombre + " - Orden visual");
                doc.Add(tit);
                doc.Add(new Paragraph(" "));
                eti.TotalWidth = 600f;
                eti.WidthPercentage = 100;
                eti.LockedWidth = true;
                

                for (int b = 0; b < 22; b++)
                {
                    eti.AddCell("VACIO");
                }
                doc.Add(eti);
                
                doc.Add(new Paragraph("  "));

                titulo.Add(" " + Salones[n].nombre + " - etiquetas");
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));
                etiq.TotalWidth = 600f;
                etiq.WidthPercentage = 100;
                etiq.LockedWidth = true;
                for(int c = 0; c < 4; c++)
                {
                    etiq.AddCell(" ");
                    
                }
                doc.Add(etiq);
                doc.Add(new Paragraph("  "));
                doc.NewPage();
            }
            doc.Add(new Paragraph(" "));
            doc.Close();
           // MessageBox.Show("pdf terminado");
        }
//FUNCIONES--------------------------------------------------------------------------------------------------------------

        public Boolean sonDistintaEscuela(String escuelaCardinal, String escuelaActual)
        {
            if (escuelaActual != escuelaCardinal)
            {
                return true;
            }
            return false;
        }

        public int[] crearPuntosCardinales(int alumnoPosicion, int columnas)
        {
            int[] puntosCardinales = new int[4];
            puntosCardinales[0] = alumnoPosicion - columnas;
            puntosCardinales[1] = alumnoPosicion + 1;
            puntosCardinales[2] = alumnoPosicion + columnas;
            puntosCardinales[3] = alumnoPosicion - 1;
            return puntosCardinales;
        }
    }
}
