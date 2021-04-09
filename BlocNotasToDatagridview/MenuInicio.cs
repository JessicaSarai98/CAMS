using System;
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
        preguntas2 p;
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
            if (Form1 == null ||  p== null)
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
            //PARA SABER SI TODAVÍA SE ASIGNAN ALUMNOS O YA ESTÁN TODOS.
            public int capacidadAUX { get; set; }
            public int estado { get; set; }
            public double[] capacidadxSalon { get; set; }
        }
        class ListaSalones
        {
            public string id { get; set; }
            public string nombre { get; set; }
            public int capacidad { get; set; }
            //Para saber si ya está ocupado todo el salón o tiene espacio
            public int capacidadAUX { get; set; }
        }

        // Creamos la lista y las variables
        string linea;
        string[] valores;
        int capacidadTotal = 0, capacidad, i=0;
        double auxAlumno;
        List<NombreEscuelas> Escuelas = new List<NombreEscuelas>();
        List<ListaSalones> Salones = new List<ListaSalones>();
        private void generarEtiquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Leemos el archivo "Entrada" para saber cuantos alumnos hay de cada escuela y agruparlos
            System.IO.StreamReader file = new System.IO.StreamReader("Archivos-Salones/Entrada.csv");
            file.ReadLine();
            //Se lee la siguiente línea y se compara hasta que se termine de leer el archivo
            while ((linea = file.ReadLine()) != null) { 
                valores = linea.Split(',');
                //Agregamos todos los valores del archivo a la lista
                Escuelas.Add(new NombreEscuelas() { nombre = valores[0] });
            }

            //Vemos cuantas líneas tiene el archivo para declarar el arreglo
            file = new System.IO.StreamReader("Archivos-Salones/Salones.csv");
            file.ReadLine();
            //procesamos cada fila para saber la capacidad del mismo
            while ( (linea = file.ReadLine()) != null)
            {
                valores = linea.Split(',');
                capacidad = Convert.ToInt32(valores[2]) * Convert.ToInt32(valores[3]);
                capacidadTotal += capacidad;
                Salones.Add(new ListaSalones() { id = valores[0], nombre = valores[1], capacidad = capacidad, capacidadAUX = capacidad });
            }

            //Vemos cuantas escuelas hay en total, cuantos alumnos tienen cada una
            //y van de mayor a menor
            var query = from escuela in Escuelas group escuela  by escuela.nombre into nuevoGrupo
                        orderby nuevoGrupo.Count() descending select nuevoGrupo;
            foreach (var group in query)
            {
                Escuelas[i].nombre = group.Key;
                Escuelas[i].capacidad = Escuelas[i].capacidadAUX = group.Count();
                Escuelas[i].estado = 0;
                Escuelas[i].capacidadxSalon = new double[Salones.Count()];
                for (int j = 0; j < Salones.Count(); j++) {
                   auxAlumno = Math.Round((double)Escuelas[i].capacidad / capacidadTotal *Salones[j].capacidad );
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
                                capacidadTotal = capacidadTotal - 1 ;
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

            //asignamos lugares en los salones, primero los separamos por salon
            for (i=0; i < Escuelas.Count(); i++)
            {
                if (Escuelas[i].capacidadxSalon != null)
                {
                    dataGridView1.Rows.Add(Escuelas[i].nombre, "Partcipantes:"+Escuelas[i].capacidad,
                    Escuelas[i].capacidadxSalon[0] + "/" + Escuelas[i].capacidadxSalon[1] + "/" +
                    Escuelas[i].capacidadxSalon[2] + "/" + Escuelas[i].capacidadxSalon[3] + "/" +
                    Escuelas[i].capacidadxSalon[4] + "/" + Escuelas[i].capacidadxSalon[5] + "/" +
                    Escuelas[i].capacidadxSalon[6]);
                }
                else Escuelas.RemoveRange(i,1);
            }
            //query.OrderBy(x => capacidad);



        }
    }
}
