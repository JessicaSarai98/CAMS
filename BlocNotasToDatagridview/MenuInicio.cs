using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BlocNotasToDatagridview
{
    public partial class MenuInicio : Form
    {
        public MenuInicio()
        {
            InitializeComponent();
        }
        string orden = "orden_rpp.txt";
        string archivo = "parejas.txt";
        preguntas preg = new preguntas();
        System.IO.StreamReader file = null;

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
                if (respuesta == DialogResult.OK)
                {
                    if (Form1 == null)
                    {
                        Form1 = new Form1();
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
            if (Form1 == null)
            {
                Form1 = new Form1();
                Form1.Owner = this;
                Form1.FormClosed += Form1_FormClosed;
                Form1.btnLimpiar.Visible = false;
                Form1.button1.Visible = false;
                DataTable dt = new DataTable();

                
                String[] lines = System.IO.File.ReadAllLines("parejas2.txt");
                if (lines.Length > 0)
                {
                    
                    String firts = lines[0];
                    String[] header = firts.Split(',',';');
                    foreach(string headerWord in header)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));

                    }
                    for(int i = 1; i<lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',',';');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach(string headerWord in header)
                        {
                            dr[headerWord] = data[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    
                    Form1.dataGridView1.DataSource = dt;
                    Form1.dataGridView1.Columns[3].Visible = false;
                    
                 //   Form1.dataGridView1.Columns[7].Visible = false;
                }
                
                Form1.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

                Form1.dataGridView1.Columns[0].HeaderText = "Nombre";
                Form1.dataGridView1.Columns[1].HeaderText = "Escuela";
                Form1.dataGridView1.Columns[2].HeaderText = "Estado";
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Elegir";
                Form1.dataGridView1.Columns.Add(chk);

                Form1.dataGridView1.Columns[0].ReadOnly = true;
                Form1.dataGridView1.Columns[1].ReadOnly = true;
                Form1.dataGridView1.Columns[2].ReadOnly = true;
                Form1.dataGridView1.Columns[3].ReadOnly = true;
                //Form1.dataGridView1.Columns[4].ReadOnly = true;
                
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
    }
}
