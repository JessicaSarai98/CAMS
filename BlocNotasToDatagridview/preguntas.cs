using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

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

        }
        private void button1_Click(object sender, EventArgs e)
        {
            funcion1();
        }
        private void listaPreguntas_SelectedIndexChanged(object sender, EventArgs e)
        {
            funcion1();
        }
        //Instancia de la clase Leer
        Leer l = new Leer();
        //Alamcena la ruta del archivo .txt
        string archivo = @"C:\Users\William carmona\Documents\Servicio Social\parejas.txt";
        public void funcion1()
        {
            listaPreguntas.Enabled = false;
            btnSig.Enabled = false;
            btnPlay.Enabled = true;
            ParticipanteA.Text = "SALMA EK";
            participanteB.Text = "KARINA PRUEBA";
            ronda_pregunta.Text = "Ronda 1 Pregunta 1";
            min = 3;
            seg = 0;
            txtMin.Text = "3";
            txtSeg.Text = "00";
        }
        Stopwatch tiempo = new Stopwatch();
        int min;
        int seg;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            tiempo.Start(); 
            timer1.Enabled = true;
            btnPlay.Enabled = false;
            pictureBox1.Visible = true;
            btnPausa.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (seg == 0 && min > 0)
            {
                min -= 1;
                seg = 60;
            }
            seg -= 1;
            string minutos;
            string segundos;            
            if (min == 0 && seg == 0) { seg = 60; }
            if (min.ToString().Length > 2) { minutos = "0" + min.ToString(); }
            if (seg.ToString().Length > 2) { segundos = "0" + seg.ToString(); }
            if (min == 0 && seg == 0) { 
                timer1.Stop();
                //ejecutar el audio
                btnPausa.Enabled = false;
                btnIgual.Enabled = true;
            }
            txtMin.Text = min.ToString();
            txtSeg.Text = seg.ToString();
        }

        private void btnPausa_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            tiempo.Stop();
            btnIgual.Enabled = true;
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            btnIgual.Enabled = false;
            btnPausa.Enabled = false;
            pictureBox1.ImageLocation = @"C:\Users\William carmona\Documents\Servicio Social\CAMS\BlocNotasToDatagridview\iconos\respuesta.png";
            btnSig.Enabled = true;
            listaPreguntas.Enabled = true;
        }
    }
}
