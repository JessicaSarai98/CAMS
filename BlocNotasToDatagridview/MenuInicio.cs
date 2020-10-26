using System;
using System.IO;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    public partial class MenuInicio : Form
    {
        public MenuInicio()
        {
            InitializeComponent();
        }
        string archivo = @"C:\Users\William carmona\Documents\Servicio Social\parejas.txt";

        private void MenuInicio_Load(object sender, EventArgs e)
        {
           if(File.Exists(archivo) )
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
        private void pasarListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(archivo))
            {
                DialogResult respuesta = MessageBox.Show("Ya existe el archivo del pase de lista, ¿desea generar uno nuevo y " +
                    "eliminar el actual?", "Mensaje", MessageBoxButtons.OKCancel , MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
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
                if (respuesta == DialogResult.No)
                {

                }
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
                preguntas.Show();
            }
            else Form1.Activate();
        }

        private void desempateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
