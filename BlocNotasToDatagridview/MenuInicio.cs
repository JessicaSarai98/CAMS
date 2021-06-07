using iTextSharp.text;
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
        // Creamos la lista y las variables
        string linea, filas_columnas;
        string[] valores;
        int capacidadTotal = 0, capacidad, i = 0, j = 0;
        double auxAlumno;
        List<NombreEscuelas> Escuelas = new List<NombreEscuelas>();
        List<ListaSalones> Salones = new List<ListaSalones>();
        //Fonts
        iTextSharp.text.Font fontTimes12 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 12, iTextSharp.text.Font.NORMAL);
        //FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK)
        Chunk glue = new Chunk(new VerticalPositionMark());
        private void generarEtiquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.StreamReader filesReader = new System.IO.StreamReader("Archivos-Salones/Salones.csv");
            filesReader.ReadLine();
            List<Salon> salones = new List<Salon>();
            while ((linea = filesReader.ReadLine()) != null)
            {
                valores = linea.Split(',');
                salones.Add(new Salon(valores[0], valores[1], stringToInt(valores[2]), stringToInt(valores[3])));
                //Karina
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
                //Fin
            }

            filesReader = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
            filesReader.ReadLine();
            //Karina
            List<Alumno> alumnos = new List<Alumno>();//Noe
            while ((linea = filesReader.ReadLine()) != null)
            {
                valores = linea.Split(',');
                //Agregamos todos los valores del archivo a la lista
                Escuelas.Add(new NombreEscuelas() { nombre = valores[0] });
                alumnos.Add(new Alumno(valores[0], valores[1], valores[2]));//Noe
            }

            //Karina
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
            //Fin
            tablaAlgoritmo tabla = new tablaAlgoritmo();

            //Karina
            //int cantidadDeEscuelas = 0;//Veamos Noe
            //for (i = 0; i < Escuelas.Count(); i++)
            //{
            //    if (Escuelas[i].capacidadxSalon != null)
            //    {
            //        capacidadTotal = i;
            //        tabla.dataGridView1.Rows.Add(Escuelas[i].nombre, "Partcipantes: " + Escuelas[i].capacidad,
            //        Escuelas[i].capacidadxSalon[0] + "/" + Escuelas[i].capacidadxSalon[1] + "/" +
            //        Escuelas[i].capacidadxSalon[2] + "/" + Escuelas[i].capacidadxSalon[3] + "/" +
            //        Escuelas[i].capacidadxSalon[4] + "/" + Escuelas[i].capacidadxSalon[5] + "/" +
            //        Escuelas[i].capacidadxSalon[6]);
            //        cantidadDeEscuelas++;//Intento Noé
            //    }
            //    else break;
            //}
            tabla.Show();

            Document doc = new Document(PageSize.LETTER);
            PdfWriter.GetInstance(doc, new FileStream("Archivos-Salones/ListaSalones.pdf", FileMode.Create)); // asignamos el nombre de archivo hola.pdf
            doc.Open();
            Paragraph title, tit, titulo;
            //Fin
            //----------------------------------------------------------------------------------------------------------------------------------------------
            List<int> limitesPorSalon = new List<int>();
            int limiteGeneral = alumnos.Count / salones.Count;
            int restantes = alumnos.Count - limiteGeneral * salones.Count;
            for (int i = 0; i < salones.Count; i++)
            {
                if (salones[i].getFilas() * salones[i].getColumnas() >= limiteGeneral)
                {
                    limitesPorSalon.Add(limiteGeneral);
                }
                else
                {
                    limitesPorSalon.Add(salones[i].getFilas() * salones[i].getColumnas());
                    restantes += limiteGeneral - salones[i].getFilas() * salones[i].getColumnas();
                }
            }
            if (restantes > 0)
            {
                for (int i = 0; i < salones.Count; i++)
                {
                    if (limitesPorSalon[i] == limiteGeneral)
                    {
                        int espacioParaAdicionar = salones[i].getFilas() * salones[i].getColumnas() - limiteGeneral;
                        if (espacioParaAdicionar - restantes >= 0)
                        {
                            limitesPorSalon[i] = limiteGeneral + restantes;
                            break;
                        }
                        else
                        {
                            limitesPorSalon[i] = limiteGeneral + espacioParaAdicionar;
                            restantes -= espacioParaAdicionar;
                        }
                    }
                }
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------
            while (alumnos.Count > 0)
            {
                int vueltasSinAsignarSalon = 0;
                for (int s = 0; s < salones.Count; s++)
                {
                    if (!(alumnos.Count > 0))
                    {
                        break;
                    }

                    int vueltasDadasSinAsignar = 0;//Máximo 2, porque sino indica iteraciones infinitas
                    while (salones[s].getAsientosVacios().Count > 0 || !(alumnos.Count > 0))
                    {
                        bool asignado = false;
                        int contadorAuxiliarAlumnos = alumnos.Count;
                        int n = 0;
                        while (n < contadorAuxiliarAlumnos)
                        {
                            if (!(alumnos.Count > 0))
                            {
                                break;
                            }
                            if (limitesPorSalon[s] == 0)
                            {
                                break;
                            }
                            int contadorAuxiliarAsientosLibres = salones[s].getAsientosVacios().Count;
                            for (int m = 0; m < contadorAuxiliarAsientosLibres; m++)
                            {
                                if (limitesPorSalon[s] == 0)
                                {
                                    break;
                                }
                                if (contadorAuxiliarAlumnos == 0)
                                {
                                    break;
                                }
                                if (salones[s].puedoSentarmeAqui_B(alumnos[n], salones[s].getAsientosVacios()[m]))
                                {
                                    int fila = salones[s].getAsientosVacios()[m] / salones[s].getColumnas();
                                    int columna = salones[s].getAsientosVacios()[m] % salones[s].getColumnas();
                                    //if (salones[s].getAsientos().GetLength(0)<fila || salones[s].getAsientos().GetLength(1)<columna)
                                    //{
                                    //    continue;
                                    //}
                                    salones[s].getAsientos()[fila, columna] = new Asiento(alumnos[n], salones[s].getAsientosVacios()[m]);
                                    //m--;
                                    //n--;
                                    asignado = true;
                                    alumnos.RemoveAt(n);
                                    contadorAuxiliarAlumnos--;
                                    contadorAuxiliarAsientosLibres--;
                                    limitesPorSalon[s]--;
                                    vueltasSinAsignarSalon = 0;
                                    break;
                                }
                                if (!(alumnos.Count > 0))
                                {
                                    break;
                                }
                                n++;
                            }
                            if (!asignado)
                            {
                                vueltasDadasSinAsignar++;
                            }
                        }
                        if (vueltasDadasSinAsignar == 2)
                        {
                            vueltasSinAsignarSalon++;
                            break;
                        }
                        if (!(alumnos.Count > 0))
                        {
                            break;
                        }
                        if (limitesPorSalon[s] == 0)
                        {
                            break;
                        }
                    }
                }
                if (!(alumnos.Count > 0) || alumnos.Count == restantes)
                {
                    break;
                }
                if (vueltasSinAsignarSalon == salones.Count)
                {
                    break;
                }
            }
            //Se terminan de asignar los alumnos a los salones, y ahora si se trabaja con el escrito del PDF 

            for (int n = 0; n < salones.Count; n++)
            {
                title = new Paragraph();
                title.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);

                //Orden visual
                tit = new Paragraph();
                tit.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);

                //etiquetas
                titulo = new Paragraph();
                titulo.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, BaseColor.BLACK);
                //LISTA---------------------------------------------------------------------------------------
                title.Add("Lista de alumnos del salón " + salones[n].getNombre());
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

                List<Asiento> listaSalon = salones[n].generarLista();
                for (int m = 0; m < listaSalon.Count; m++)
                {
                    String numero = Convert.ToString(m + 1);
                    salon = new PdfPCell(new Phrase(numero, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);

                    salon = new PdfPCell(new Phrase(listaSalon[m].getAlumno().getFolio(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    //Matricula
                    salon = new PdfPCell(new Phrase(listaSalon[m].getAlumno().getNombreAlumno(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                    //Nombre
                    salon = new PdfPCell(new Phrase("" + (listaSalon[m].getNumeroAsiento() + 1), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)));
                    salon.HorizontalAlignment = 1;
                    table.AddCell(salon);
                }
                //ETIQUETAS---------------------------------------------------------------------------------------
                PdfPTable etiq = new PdfPTable(3);
                for (int m = 0; m < salones[n].getFilas(); m++)
                {
                    for (int l = 0; l < salones[n].getColumnas(); l++)
                    {
                        if (salones[n].getAsientos()[m, l] != null)
                        {
                            Phrase phraseEtiquetasF = new Phrase();
                            phraseEtiquetasF.Add(
                                new Chunk("Folio: \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseEtiquetasF.Add(
                                new Chunk(salones[n].getAsientos()[m, l].getAlumno().getFolio() + "\n", FontFactory.GetFont(FontFactory.TIMES_BOLD, 52f, BaseColor.BLACK))
                            );
                            phraseEtiquetasF.Add(glue);
                            phraseEtiquetasF.Add(
                                new Chunk(" " + (salones[n].getAsientos()[m, l].getNumeroAsiento() + 1), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
                            );
                            salon = new PdfPCell(phraseEtiquetasF);
                            salon.HorizontalAlignment = 1;
                            etiq.AddCell(salon);
                        }
                    }
                }
                //Orden visual
                PdfPTable ordenVisual = new PdfPTable(salones[n].getColumnas());
                Phrase phrase = new Phrase();
                phrase.Add(
                    new Chunk("PIZARRA", FontFactory.GetFont(FontFactory.TIMES_BOLD, 12f, BaseColor.BLACK))
                );
                salon = new PdfPCell(phrase);
                salon.Colspan = salones[n].getColumnas();
                salon.HorizontalAlignment = 1;
                ordenVisual.AddCell(salon);
                for (int m = 0; m < salones[n].getFilas(); m++)
                {
                    for (int l = 0; l < salones[n].getColumnas(); l++)
                    {
                        if (salones[n].getAsientos()[m, l] != null)
                        {
                            Phrase phraseOrden = new Phrase();
                            phraseOrden.Add(
                                new Chunk("Folio: \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseOrden.Add(
                                new Chunk(salones[n].getAsientos()[m, l].getAlumno().getFolio() + " \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK))
                            );
                            phraseOrden.Add(glue);
                            phraseOrden.Add(
                                new Chunk((salones[n].getAsientos()[m, l].getNumeroAsiento() + 1) + " \n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
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
                                new Chunk(" " + (l * l), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, BaseColor.BLACK))
                            );

                            salon = new PdfPCell(phraseOrden);
                            salon.HorizontalAlignment = 1;
                            ordenVisual.AddCell(salon);
                        }
                    }
                }
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

                int pAux = listaSalon.Count;
                while (pAux % 3 != 0)
                {
                    etiq.AddCell(" ");
                    pAux++;
                }
                doc.Add(etiq);
                doc.Add(new Paragraph("  "));
                doc.NewPage();
            }
            MessageBox.Show("Sobraron estos alumnos " + alumnos.Count);
            //Aqui debes crear una página con los alumnos que no pudieron ser asignados
            //La informacion requerida la contiene la lista "alumnos"
            for (int n = 0; n < alumnos.Count; n++)//For donde tendras todos los alumnos 
            {
                MessageBox.Show("Escribo la informacion del alumno " + alumnos[n].getNombreAlumno());//Aqui se debería escribir la información que se quiera mostrar
            }
            doc.Add(new Paragraph(" "));
            doc.Close();
        }


        //FUNCIONES--------------------------------------------------------------------------------------------------------------

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

        public int stringToInt(string value)
        {
            return Convert.ToInt32(value);
        }
    }
}