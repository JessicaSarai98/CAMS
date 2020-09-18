using System;
using System.Windows.Forms;
using System.IO; 

namespace BlocNotasToDatagridview
{
    public partial class ParejasAleatorias : Form
    {
        String[] escuelas = new string[10];
        public Form1 f; 
        public ParejasAleatorias()
        {
            InitializeComponent();
        }

        private void ParejasAleatorias_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {   //C:\Users\Jessica\Documents\CAMS
            //karina C:\Users\William carmona\Documents\Servicio Social
            if (!Directory.Exists(@"C:\Users\Jessica\Documents\CAMS"))
            {
                Directory.CreateDirectory(@"C:\Users\Jessica\Documents\CAMS");
            }

            using (TextWriter tw = new StreamWriter(@"C:\Users\Jessica\Documents\impresion_parejas.txt"))
            {
                tw.Write("Participante A:\n");
                for (int i = 0; i < 5; i++)
                {
                    String escuela = this.f.escuela(TablaParejas.Rows[i].Cells[0].Value.ToString());
                    tw.WriteLine($"{TablaParejas.Rows[i].Cells[0].Value.ToString()}"+","+escuela);
                }
            

                tw.Write("\nParticipante B:\n");
                for(int j = 0; j < 5; j++)
                {
                    String esc = this.f.escuela(TablaParejas.Rows[j].Cells[1].Value.ToString());
                    tw.WriteLine($"{TablaParejas.Rows[j].Cells[1].Value.ToString()}"+","+esc);
                }
                
            }
            using (TextWriter w = new StreamWriter(@"C:\Users\Jessica\Documents\parejas.txt"))
            {
                for(int k=0; k < TablaParejas.Rows.Count-1; k++)
                {
                    for (int m = 0; m < TablaParejas.Columns.Count; m++)
                    {
                        string es = this.f.escuela(TablaParejas.Rows[k].Cells[m].Value.ToString());
                        string estado = this.f.estado(TablaParejas.Rows[k].Cells[m].Value.ToString());
                        float cal = this.f.calif(TablaParejas.Rows[k].Cells[m].Value.ToString());

                        w.Write($"{TablaParejas.Rows[k].Cells[m].Value.ToString()}" + "," + es + "," + estado+","+cal);
                        if (m != TablaParejas.Columns.Count - 1)
                        {
                            w.Write(";");
                        }
                    }
                    w.WriteLine("\n"); 
                }
            }
                MessageBox.Show("Datos exportados");
        }
        public void setForm1(Form1 tabla)
        {
            this.f = tabla; 
        }
    }
}
