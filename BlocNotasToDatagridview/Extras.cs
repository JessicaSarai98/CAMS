using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    public partial class Extras : Form
    {
        System.IO.StreamReader file;
        string linea;
        int i = 1;
        int problemaSeleccioando;
        public Extras()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            file = new System.IO.StreamReader("Orden_rpp_extras.txt");
            i = 1;
            while ((linea = file.ReadLine()) != null)
            {
                if (problemaSeleccioando == i)
                {
                    //creamos los archivos de los problemas y las preguntas
                    string[] valores = linea.Split(',');
                    preguntas FormPreguntas = Owner as preguntas;
                    FormPreguntas.axAcroPDF1.Visible = false;
                    FormPreguntas.funcion1();
                    FormPreguntas.btnIgual.Enabled = false;
                    FormPreguntas.tiempo_pregunta_respuesta(valores[3],valores[0],valores[1],valores[2]);
                    file = null;
                    this.Close();
                    break;
                }
                i++;
            }
        }
        private void Extras_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            DataGridViewCheckBoxColumn chk;
            //toma el archivo para llenar la tabla
            file = new System.IO.StreamReader("Orden_rpp_extras.txt");
            try
            {
                while ((linea = file.ReadLine()) != null)
                {
                    dataGridView1.Rows.Add("PROBLEMA EXTRA "+ i++);                 
                }
                file.Close();
                file = null;
                chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Seleccionado";
                dataGridView1.Columns.Add(chk);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //limpiamos todo y almacenamos el problema seleccioando
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = false;
            }
            problemaSeleccioando = e.RowIndex +1;
        }
    }
}
