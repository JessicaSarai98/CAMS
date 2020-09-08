using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;


namespace BlocNotasToDatagridview
{
    public partial class Form1 : Form
    {
        //Instancia de la clase Leer
        Leer l = new Leer();
        //Alamcena la ruta del archivo .txt
        public string ARCHIVO ="";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //Abre el openFileDialog y captura la ruta del bloc de notas'
        public void cargarArchivo()
        {
            try
            {
                this.openFileDialog1.ShowDialog();

                if (!string.IsNullOrEmpty(this.openFileDialog1.FileName))
                {
                    ARCHIVO = this.openFileDialog1.FileName;
                    l.lecturaArchivo(dataGridView1, ',', ARCHIVO);

                    dataGridView1.Columns[0].HeaderText = "Nombre";
                    dataGridView1.Columns[1].HeaderText = "Escuela";
                    dataGridView1.Columns[2].HeaderText = "Estado";

                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma",9.75F, FontStyle.Bold); 

                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;

                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.HeaderText = "Elegir";
                    dataGridView1.Columns.Add(chk);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargarArchivo();
        }
        int contador = 0;
        string[] nombres = new string[11];
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (contador < 10)
            {
                contador += 1;
                string a = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
                nombres[contador] = a;
            }            
            if(contador == 10)
            {
                MessageBox.Show("Usted ya ha seleccionado 10 participantes.");
                btnTerminar.Visible = true;
            }
        }

        Random rnd = new Random();
        List<int> listaNoRepetidos = new List<int>();
        int IntialCount;
        int contValor = 0;
        ParejasAleatorias Parejas = new ParejasAleatorias();
        int valor1 = 0;
        int valor2;
        private void btnTerminar_Click(object sender, EventArgs e)
        {
            listaNoRepetidos.Clear();
            IntialCount = 1;
            var result = MessageBox.Show("A continuación se generá la lista de parejas con los " +
                "participantes seleccionados. ¿Desea continuar?", "Mensaje", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {                
                while (IntialCount <= 10)
                {
                    //GENERAMOS EL NÚMERO ALEATORIO
                    rnd = new Random();
                    int aux = Convert.ToInt32(rnd.Next(1, contador + 1));
                    //si no esta en la lista, lo anexamos y sí evitamos que salgan nombres
                    // o numero repetidos en el algoritmo
                    if (!listaNoRepetidos.Contains(aux))
                    {
                        listaNoRepetidos.Add(aux);
                        IntialCount++;
                        contValor++;
                        if(contValor == 1)
                        {
                            valor1 = aux;
                        }
                        if(contValor == 2)
                        {
                            valor2 = aux;
                            contValor = 0;
                            Parejas.TablaParejas.Rows.Add(nombres[valor1], nombres[valor2]);
                        }
                    }

                }

                Parejas.Show();
            }
        }
    }
}
