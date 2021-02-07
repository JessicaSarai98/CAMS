using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BlocNotasToDatagridview
{
    public partial class Form1 : Form
    {
        int limite;
        //Instancia de la clase Leer
        Leer l = new Leer();
        //Alamcena la ruta del archivo .txt
        public string ARCHIVO = "";       

        public void setLimite(int limite)
        {
            this.limite = limite;
        }

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

                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;

                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.HeaderText = "Elegir";
                    dataGridView1.Columns.Add(chk);

                    this.dataGridView1.Columns[0].ReadOnly = true;
                    this.dataGridView1.Columns[1].ReadOnly = true;
                    this.dataGridView1.Columns[2].ReadOnly = true;
                    this.dataGridView1.Columns[3].ReadOnly = true;
                    this.dataGridView1.Columns[4].ReadOnly = true;



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        public void cargarArchivo2()
        {
            try
            {

                if (!string.IsNullOrEmpty(this.openFileDialog1.FileName))
                {
                    l.lecturaArchivo(dataGridView1, ',', "parejas2.txt");

                    dataGridView1.Columns[0].HeaderText = "Nombre";
                    dataGridView1.Columns[1].HeaderText = "Escuela";
                    dataGridView1.Columns[2].HeaderText = "Estado";

                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;

                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.HeaderText = "Elegir";
                    dataGridView1.Columns.Add(chk);

                    this.dataGridView1.Columns[0].ReadOnly = true;
                    this.dataGridView1.Columns[1].ReadOnly = true;
                    this.dataGridView1.Columns[2].ReadOnly = true;
                    this.dataGridView1.Columns[3].ReadOnly = true;
                    this.dataGridView1.Columns[4].ReadOnly = true;
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargarArchivo();
        }

        int contador = 0;
        string[] nombres = new string[20];

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = dataGridView1.Columns.GetColumnCount(DataGridViewElementStates.None) - 1;

            Object obj = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            String nombre = (String)obj;
            int posicion = -1;
            bool encontradoNombre = false;
            if (contador > 0)
            {
                for (int i = 0; i < contador; i++)
                {
                    if (nombres[i].Equals(nombre))
                    {
                        nombres[i] = null;
                        contador--;
                        //MessageBox.Show("" + contador);
                        encontradoNombre = true;
                        posicion = i;
                    }
                }
            }
            if (encontradoNombre == false)
            {
                //MessageBox.Show("nombres[" + contador + "] = " + nombre);
                nombres[contador] = nombre;
                contador++;
            }
            if (encontradoNombre)
            {
                for (int j = posicion; j < contador; j++)
                {
                    if (posicion != 19 && nombres[j + 1] != null)
                    {
                        String aux = nombres[j + 1];
                        nombres[j] = aux;
                    }
                }
            }

            if(contador>limite && encontradoNombre == false)
            {
                int i = 0; 
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (nombres[limite-1].Equals(row.Cells[0].Value.ToString()))
                    {
                        row.Cells[5].Value = false;
                    }
                    i++; 
                }
                nombres[limite-1] = nombre;
                contador--;
                MessageBox.Show("Se ha desmarcado el anterior candidato");
            }

            if (contador == limite)
            {
                btnTerminar.Visible = true;
                if (limite == 2)
                {
                    btnTerminar.Visible = false;
                }
            }
            else
            {
                btnTerminar.Visible = false;
            }
            if (contador > limite && encontradoNombre == false)
            {
                dataGridView1.CurrentRow.Cells[5].Value = false;
            }


        }


        Random rnd = new Random();
        List<int> listaNoRepetidos = new List<int>();
        int IntialCount;
        int contValor = 0;
        ParejasAleatorias Parejas;
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
                if (Parejas == null)
                {
                    Parejas = new ParejasAleatorias();
                    Parejas.Owner = this;
                    Parejas.FormClosed += Parejas_FormClosed;
                }
                else Parejas.Activate();
                while (IntialCount <= limite)
                {
                    //GENERAMOS EL NÚMERO ALEATORIO
                    rnd = new Random();
                    int aux = Convert.ToInt32(rnd.Next(0, contador));
                    //si no esta en la lista, lo anexamos y sí evitamos que salgan nombres
                    // o numero repetidos en el algoritmo
                    if (!listaNoRepetidos.Contains(aux))
                    {
                        listaNoRepetidos.Add(aux);
                        IntialCount++;
                        contValor++;
                        if (contValor == 1)
                        {
                            valor1 = aux;
                        }
                        if (contValor == 2)
                        {
                            valor2 = aux;
                            contValor = 0;
                            Parejas.TablaParejas.Rows.Add(nombres[valor1], nombres[valor2]);

                        }
                    }

                }
                if (Parejas != null)
                {
                    Parejas.Show();
                    Parejas.setForm1(this); 
                }

            }



        }

        private void dataGridView1_CellValeChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Parejas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Parejas = null;
        }

        private bool encontrado(String[] nombres, String nombre, int contador)
        {
            if (contador != 0)
            {
                for (int i = 0; i < nombres.Length; i++)
                {
                    if (nombres[i].Equals(nombre))
                    {
                        nombres[i] = null;
                        MessageBox.Show("Eliminando a " + nombre);
                        organizar(nombres, i);
                        return true;
                    }
                }
            }
            nombres[contador] = nombre;
            MessageBox.Show("Agregando a " + nombre);
            return false;
        }

        private void organizar(String[] nombres, int i)
        {
            for (int j = i; j < nombres.Length; j++)
            {
                if (i != 9 && nombres[i + 1] != null)
                {
                    String aux = nombres[i + 1];
                    nombres[i] = aux;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Array.Clear(nombres, 0, 10);
            btnTerminar.Visible = false;
            contador = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[5].Value = false;
            }
        }

        public String escuela(String name)
        {
            String esc = null; 
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (name.Equals(row.Cells[0].Value.ToString()))
                {
                    return row.Cells[1].Value.ToString(); 
                }
            }
            return esc; 
        }

        public float prom(String name)
        {
            float g = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (name.Equals(row.Cells[0].Value.ToString()))
                {
                    return float.Parse(row.Cells[4].Value.ToString());
                }
            }
            return g; 
        }

        public float calif(String name)
        {
            float g = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (name.Equals(row.Cells[0].Value.ToString()))
                {
                    return float.Parse(row.Cells[3].Value.ToString());
                }
            }
            return g;
        }

        public String estado(String name)
        {
            String est = null; 
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (name.Equals(row.Cells[0].Value.ToString()))
                {
                    return row.Cells[2].Value.ToString(); 
                }
            }
            return est; 
        }

    }
}

