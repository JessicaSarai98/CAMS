﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf.fonts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
        //Fonts
        iTextSharp.text.Font fontTimes12 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 12, iTextSharp.text.Font.NORMAL);
        //FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)
        iTextSharp.text.Font fontTimes52 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 52, iTextSharp.text.Font.NORMAL);
        iTextSharp.text.Font fontTimes9 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 9, iTextSharp.text.Font.NORMAL);
        Chunk glue = new Chunk(new VerticalPositionMark());
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
            Paragraph title, tit, titulo;
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
            int[,] salonesFilasColumnas = new int[numSalones, 2];
            for (int n = 0; n < numSalones; n++)
            {
                int alumnoPosicion = 0;//Posicion en la lista de alumnos del salon
                int asientosPosicion = 0;//Posicion en los asientos del salon
                linea = fileSalon.ReadLine();
                String[] salonesInfo = linea.Split(',');
                salonesFilasColumnas[n, 0] = Convert.ToInt32(salonesInfo[2]);
                salonesFilasColumnas[n, 1] = Convert.ToInt32(salonesInfo[3]);

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
                                    asientosInfo[n, (int)posicionesSaltadas[0] + auxAsientosPosicion] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], ((int)posicionesSaltadas[0] + auxAsientosPosicion + 1));
                                    auxAsientosPosicion = 0;
                                    asientosPosicion++;
                                    numDeVuletasRepetidas = 0;
                                }
                                else
                                {
                                    asientosInfo[n, (int)posicionesSaltadas[0]] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], ((int)posicionesSaltadas[0] + 1));
                                    posicionesSaltadas.Remove(posicionesSaltadas[0]);
                                    asientosPosicion++;
                                    numDeVuletasRepetidas = 0;
                                }
                            }
                            else
                            {
                                asientosInfo[n, asientosPosicion] = new AlumnoInfo_Asiento(listaDeAlumnosPorSalon[n, alumnoPosicion], (asientosPosicion + 1));
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

                //Lista salon
                title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);

                //Orden visual
                tit = new Paragraph();
                tit.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);

                //etiquetas

                titulo = new Paragraph();
                titulo.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);
                //var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);

                title.Add("Lista de alumnos del salón " + Salones[n].nombre);
                doc.Add(title);
                doc.Add(new Paragraph(" "));

                //Cambiar tamaño
                PdfPTable table = new PdfPTable(4);
                table.SetTotalWidth(new float[] { 10f, 10f, 40f, 10f });
                PdfPCell salon = new PdfPCell(new Phrase("No.", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)));
                salon.HorizontalAlignment = 1;
                table.AddCell(salon);
                salon = new PdfPCell(new Phrase("Folio", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)));
                salon.HorizontalAlignment = 1;
                table.AddCell(salon);
                salon = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)));
                salon.HorizontalAlignment = 1;
                table.AddCell(salon);
                salon = new PdfPCell(new Phrase("Lugar", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)));
                salon.HorizontalAlignment = 1;
                table.AddCell(salon);

                PdfPTable etiq = new PdfPTable(3);

                int alumnoPosicion = 0;
                while (listaDeAlumnosPorSalon[n, alumnoPosicion] != null)
                {
                    alumnoPosicion++;
                }
                int alumnos = alumnoPosicion;
                int saltosAsientos = 0;

                //Unicamente la Lista de Alumnos del salon
                AlumnoInfo_Asiento[] listaOrdenadaDeAlumnos = new AlumnoInfo_Asiento[alumnos];

                for (int p = 0; p < alumnos; p++)
                {
                    while (asientosInfo[n, p + saltosAsientos] == null)
                    {
                        saltosAsientos++;
                    }
                    listaOrdenadaDeAlumnos[p] = asientosInfo[n, p + saltosAsientos];

                    //Escribiendo lista


                    //String numero = Convert.ToString(p + 1);
                    //salon = new PdfPCell(new Phrase(numero, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    //salon.HorizontalAlignment = 1;
                    //table.AddCell(salon);
                    
                    //salon = new PdfPCell(new Phrase(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getMatricula(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    //salon.HorizontalAlignment = 1;
                    //table.AddCell(salon);
                    
                    //salon = new PdfPCell(new Phrase(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getNombre(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    //salon.HorizontalAlignment = 1;
                    //table.AddCell(salon);
                    
                    //salon = new PdfPCell(new Phrase("" + asientosInfo[n, p + saltosAsientos].getAsiento(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    //salon.HorizontalAlignment = 1;
                    //table.AddCell(salon);
                    

                    //Fin de lista 

                    //Etiquetas
                    //Cambiar tamaño en fontTimes__ 
                    Phrase phraseEtiquetasF = new Phrase();
                    phraseEtiquetasF.Add(
                        new Chunk("Folio: \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                    );
                    phraseEtiquetasF.Add(
                        new Chunk(asientosInfo[n, p + saltosAsientos].getAlumnoInfo().getMatricula() + "\n", FontFactory.GetFont(FontFactory.TIMES_BOLD, 52f, BaseColor.BLACK))
                    );
                    phraseEtiquetasF.Add(glue);
                    phraseEtiquetasF.Add(
                        new Chunk(" " + asientosInfo[n, p + saltosAsientos].getAsiento(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
                    );
                    salon = new PdfPCell(phraseEtiquetasF);
                    salon.HorizontalAlignment = 1;
                    etiq.AddCell(salon);
                    alumnoPosicion++;
                    //Fin etiquetas
                }
                
                for (int a = 0; a<listaOrdenadaDeAlumnos.Length; a++)
                {
                    for (int b = 0; b<listaOrdenadaDeAlumnos.Length-a-1; b++)
                    {
                        String alumno1 = RemoveDiacritics(listaOrdenadaDeAlumnos[b].getAlumnoInfo().getNombre());
                        String alumno2 = RemoveDiacritics(listaOrdenadaDeAlumnos[b + 1].getAlumnoInfo().getNombre());
                        if (alumno1.CompareTo(alumno2)>0)
                        {
                            AlumnoInfo_Asiento aux = listaOrdenadaDeAlumnos[b];
                            listaOrdenadaDeAlumnos[b] = listaOrdenadaDeAlumnos[b + 1];
                            listaOrdenadaDeAlumnos[b + 1] = aux;
                        }

                    }
                }
                for (int p = 0; p<alumnos; p++)
                {
                    //Escribiendo lista
                    String numero = Convert.ToString(p + 1);
                    salon = new PdfPCell(new Phrase(numero, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    
                    salon = new PdfPCell(new Phrase(listaOrdenadaDeAlumnos[p].getAlumnoInfo().getMatricula(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    //Matricula
                    salon = new PdfPCell(new Phrase(listaOrdenadaDeAlumnos[p].getAlumnoInfo().getNombre(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    //Nombre
                    salon = new PdfPCell(new Phrase("" + listaOrdenadaDeAlumnos[p].getAsiento(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    //Número de asiento 
                    //Fin de lista 
                }

                //Unicamente el orden visual del salon ------------------------------------------------------------------------------------
                PdfPTable ordenVisual = new PdfPTable(salonesFilasColumnas[n, 1]);
                Phrase phrase = new Phrase();
                phrase.Add(
                    new Chunk("PIZARRA",  FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK))
                );
                salon = new PdfPCell(phrase);
                salon.Colspan = salonesFilasColumnas[n, 1];
                salon.HorizontalAlignment = 1;
                ordenVisual.AddCell(salon);
                for (int l = 0; l<salonesFilasColumnas[n,0]; l++)
                {
                    for (int m = 0; m<salonesFilasColumnas[n,1]; m++)
                    {
                        if (asientosInfo[n, l*salonesFilasColumnas[n, 1] + m] != null)
                        {
                            Phrase phraseOrden = new Phrase();
                            phraseOrden.Add(
                                new Chunk("Folio: \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseOrden.Add(
                                new Chunk(asientosInfo[n, l * salonesFilasColumnas[n, 1] + m].getAlumnoInfo().getMatricula() + " \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseOrden.Add(glue);
                            phraseOrden.Add(
                                new Chunk(asientosInfo[n, l * salonesFilasColumnas[n, 1] + m].getAsiento() + " \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
                            );

                            salon = new PdfPCell(phraseOrden);
                            salon.HorizontalAlignment = 1;
                            ordenVisual.AddCell(salon);
                        }
                        else
                        {
                            Phrase phraseOrden = new Phrase();
                            Chunk glue = new Chunk(new VerticalPositionMark());
                            phraseOrden.Add(
                                new Chunk("Lugar vacío \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseOrden.Add(
                                new Chunk(" \n", fontTimes12)
                            );
                            phraseOrden.Add(glue);
                            phraseOrden.Add(
                                new Chunk(" " + (l * salonesFilasColumnas[n, 1] + m + 1), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
                            );

                            salon = new PdfPCell(phraseOrden);
                            salon.HorizontalAlignment = 1;
                            ordenVisual.AddCell(salon);
                        }
                    }
                }
                //Fin de orden visual------------------------------------------------------------------------------------------------------
                doc.Add(table);
                doc.Add(new Paragraph(" "));

                //Inicio de una nueva página
                doc.NewPage(); 
                tit.Add(" " + Salones[n].nombre + " - Orden visual");
                doc.Add(tit);
                doc.Add(new Paragraph(" "));
                ordenVisual.TotalWidth = 500f;
                ordenVisual.WidthPercentage = 100;
                ordenVisual.LockedWidth = true;
                doc.Add(ordenVisual);
                
                //Unicamente Etiquetas
                doc.Add(new Paragraph("  "));
                doc.NewPage();
                titulo.Add(" " + Salones[n].nombre + " - etiquetas");
                doc.Add(titulo);
                doc.Add(new Paragraph(" "));
                etiq.TotalWidth = 500f;
                etiq.WidthPercentage = 100;
                etiq.LockedWidth = true;

                int pAux = alumnos;
                while (pAux%3 != 0)
                {
                    etiq.AddCell(" ");
                    pAux++;
                }
                doc.Add(etiq);
                doc.Add(new Paragraph("  "));
                doc.NewPage();
            }
            doc.Add(new Paragraph(" "));
            doc.Close();
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

        public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}