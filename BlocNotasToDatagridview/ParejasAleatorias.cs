using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace BlocNotasToDatagridview
{
    public partial class ParejasAleatorias : Form
    {
        String[] escuelas = new string[10];
        Form1 f; 
        public ParejasAleatorias()
        {
            InitializeComponent();
        }

        private void ParejasAleatorias_Load(object sender, EventArgs e)
        {

            //for(int i=0; i < f.dataGridView1.Rows.Count; i++)
            //{
            //    for(int j=0; j < f.dataGridView1.Columns.Count; j++)
            //    {
            //        if ((f.dataGridView1.Rows[i].Cells[j]) == (dataGridView1.Rows[i].Cells[j]))
            //        {
            //            string b= f.dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //            dataGridView1.Rows.Add(b.ToString());
            //        }
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\Users\Jessica\Documents\CAMS"))
            {
                Directory.CreateDirectory(@"C:\Users\Jessica\Documents\CAMS");
            }

            using (TextWriter tw = new StreamWriter(@"C:\Users\Jessica\Documents\parejas.txt"))
            {
                tw.Write("Participante A:\n");
                for (int i = 0; i < 5; i++)
                {
                    tw.WriteLine($"{TablaParejas.Rows[i].Cells[0].Value.ToString()}");
                }
                //tw.Write("Participante A:\n");
                //tw.WriteLine($"{TablaParejas.Rows[0].Cells[0].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[1].Cells[0].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[2].Cells[0].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[3].Cells[0].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[4].Cells[0].Value.ToString()}");

                tw.Write("\nParticipante B:\n");
                for(int j = 0; j < 5; j++)
                {
                    tw.WriteLine($"{TablaParejas.Rows[j].Cells[1].Value.ToString()}");
                }
                //tw.WriteLine($"{TablaParejas.Rows[0].Cells[1].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[1].Cells[1].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[2].Cells[1].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[3].Cells[1].Value.ToString()}");
                //tw.WriteLine($"{TablaParejas.Rows[4].Cells[1].Value.ToString()}");

                //for (int i = 0; i < TablaParejas.Rows.Count-1; i++)
                //{
                //    tw.Write("Pareja A: ");
                //    for (int j = 0; j < TablaParejas.Columns.Count; j++)
                //    {

                //        tw.WriteLine($"{TablaParejas.Rows[i].Cells[j].Value.ToString()}");
                //        //if (j != TablaParejas.Columns.Count - 1)
                //        //{
                //        //    tw.Write("Pareja B: ");
                //        //}

                //    }
                //    tw.WriteLine();
                //}
            }
            MessageBox.Show("Datos exportados");
        }
    }
}
