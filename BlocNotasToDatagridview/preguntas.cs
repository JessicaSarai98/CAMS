using System;
using System.Windows.Forms;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Media;

namespace BlocNotasToDatagridview
{
    public partial class preguntas : Form
    {
        public preguntas()
        {
            InitializeComponent();
        }

        private void preguntas_Load(object sender, EventArgs e)
        {
            //toma el archivo para llenar el combobox de las preguntas
            int counter = 0;
            System.IO.StreamReader file =
                new System.IO.StreamReader("CAMS/orden_rpp.txt");
            while ((file.ReadLine()) != null)
            {
                counter++;
                listaPreguntas.Items.Add(counter);
            }
            file.Close();
            file = null;
        }
        int i = 0;
        System.IO.StreamReader file = null;
        System.IO.StreamReader file2 = null;
        private void button1_Click(object sender, EventArgs e)
        {
            axAcroPDF1.Visible = false;
            //se habilitan y se deshabilitan los botones correspondientes
            funcion1();

            //toma el archivo orden_rpp 
            if (file2 == null)
                file2 = new System.IO.StreamReader("CAMS/orden_rpp.txt");
            if (!file2.EndOfStream)
            {
                //se lee todo el archivo orden_rpp y se dividen las lineas por los comas
                string linea = file2.ReadLine();
                char delimitador = ',';
                string[] valores = linea.Split(delimitador);
                //encontramos la pareja de esa pregunta
                EncontrarParticipantes(Convert.ToInt32(valores[1]));
                //se lee el num de pregunta y de ronda del archivo            
                ronda_pregunta.Text = "Ronda " + valores[0] + "\n Pregunta " + valores[2];
                //se asigna el tiempo al cronometro y se asigna la pregunta que se lee del pdf
                tiempo_pregunta_respuesta(valores[4],valores[5],valores[6],valores[7]);
            }
            else
            {
                MessageBox.Show("Fin del documento");
                btnPlay.Enabled = false;
                file2.Close();
            }

        }

        PdfReader reader = null;
        Document sourceDocument = null;
        PdfCopy pdfCopyProvider = null;
        PdfImportedPage importedPage = null;
        public void RespuestasPDF(int startPage, int endPage)
        {
            string sourcePdfPath = "CAMS/Programas.pdf";
            string outputPdfPath = "respuesta.pdf";
            if (File.Exists(outputPdfPath))
            {
                System.IO.File.Delete(outputPdfPath);
            }
            try
            {
                reader = new PdfReader(sourcePdfPath);
                sourceDocument = new Document(reader.GetPageSizeWithRotation(startPage));
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));
                sourceDocument.Open();
                for (int i = startPage; i <= endPage; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PreguntasPDF(int page)
        {
            string sourcePdfPath = "CAMS/Programas.pdf";
            string outputPdfPath = "pregunta.pdf";
            if (File.Exists(outputPdfPath))
            {
                System.IO.File.Delete(outputPdfPath);
            }
            try {
                reader = new PdfReader(sourcePdfPath);
                // se conserva el mismo tamaño de la página
                sourceDocument = new Document(reader.GetPageSizeWithRotation(page));
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));
                sourceDocument.Open();
                importedPage = pdfCopyProvider.GetImportedPage(reader, page);
                pdfCopyProvider.AddPage(importedPage);
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string linea, lineaPareja;
        string[] valores;
        public void EncontrarParticipantes(int pareja)
        {
            //leemos el nombre de la pareja
            i = 1;
            if (file == null)
                file = new System.IO.StreamReader("parejas.txt");
            while ((lineaPareja = file.ReadLine()) != null)
            {
                if (pareja == i)
                {
                    //muestra los nombres de parejas.txt
                    valores = lineaPareja.Split(',', ';');
                    ParticipanteA.Text = valores[0];
                    participanteB.Text = valores[4];
                    file = null;
                    break;
                }
                i++;
            }
        }
        private void listaPreguntas_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcion1();
            i = 1;
            int pregunta = Convert.ToInt32(listaPreguntas.SelectedItem);
            //toma el archivo orden_rpp 
            file2 = new System.IO.StreamReader("CAMS/orden_rpp.txt");
            while ((linea = file2.ReadLine()) != null)
            {
                if (i == pregunta)
                {
                    valores = linea.Split(',');
                    EncontrarParticipantes(Convert.ToInt32(valores[1]));
                    break;
                }
                i++;
            }
            char delimitador = ',';
            valores = linea.Split(delimitador);
            //se lee el num de pregunta y de ronda del archivo            
            ronda_pregunta.Text = "Ronda " + valores[0] + "\n Pregunta " + valores[2];
            //se asigna el tiempo al cronometro Y se asigna la pregunta que se lee del pdf
            tiempo_pregunta_respuesta(valores[4], valores[5], valores[6], valores[7]);          
        }       
        public void tiempo_pregunta_respuesta(string tiempo, string pregunta, 
            string respuestaInicio, string respuestaFinal)
        {
            string[] tiempoTodo = tiempo.Split('.');
            min = Convert.ToInt32(tiempoTodo[0]);
            seg = Convert.ToInt32(tiempoTodo[1]);
            txtMin.Text = tiempoTodo[0];
            txtSeg.Text = tiempoTodo[1];
            PreguntasPDF(Convert.ToInt32(pregunta));
            RespuestasPDF(Convert.ToInt32(respuestaInicio), Convert.ToInt32(respuestaFinal));
        }
        public void funcion1()
        {
            listaPreguntas.Enabled = false;
            btnSig.Enabled = false;
            btnPlay.Enabled = true;
        }
        Stopwatch tiempo = new Stopwatch();
        int min;
        int seg;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            tiempo.Start();
            timer1.Enabled = true;
            btnPlay.Enabled = false;
            btnPausa.Enabled = true;
            axAcroPDF1.Visible = true;
            axAcroPDF1.LoadFile("pregunta.pdf");

        }
        SoundPlayer sonido;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (seg == 0 && min > 0)
            {
                min -= 1;
                seg = 60;
            }
            seg -= 1;        
            if (min.ToString().Length < 2) { txtMin.Text = "0" + min.ToString(); }
                else { txtMin.Text = min.ToString(); }
            if (seg.ToString().Length < 2) { txtSeg.Text = "0" + seg.ToString(); }
                else { txtSeg.Text = seg.ToString(); }
            if (min == 0 && seg == 0) { 
                timer1.Stop();
                //ejecutar el audio
                sonido = new SoundPlayer(Application.StartupPath+@"/CAMS/SONIDO-TIEMPO.wav" );
                sonido.Play();
                //cambia los botones
                btnPausa.Enabled = false;
                btnIgual.Enabled = true;
            }
        }

        private void btnPausa_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            tiempo.Stop(); 
            Extras extras = new Extras();
            //le decimos que es un form hijo para poder recibir la información y cambiar
            //los valores de este form
            AddOwnedForm(extras);
            extras.SeleccionarTipoForm(1);
            extras.Show();
            extras.btnAcepDes.Visible = false;
            btnIgual.Enabled = true;
            btnPausa.Enabled = false;
            
        }        
        private void btnIgual_Click(object sender, EventArgs e)
        {
            btnIgual.Enabled = false;
            btnPausa.Enabled = false;
            btnSig.Enabled = true;
            listaPreguntas.Enabled = true;
            //se muestra el pdf con las respuestas
            axAcroPDF1.Visible = true;
            axAcroPDF1.LoadFile("respuesta.pdf");
        }
    }
}
