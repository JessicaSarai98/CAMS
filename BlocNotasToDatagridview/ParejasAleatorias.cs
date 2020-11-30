using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace BlocNotasToDatagridview
{
    public partial class ParejasAleatorias : Form
    {

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

        [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
        private static extern int GetMenuItemCount(IntPtr hmenu);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

        [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
        private static extern int DrawMenuBar(IntPtr hwnd);

        private const int MF_BYPOSITION = 0x0400;
        private const int MF_DISABLED = 0x0002;

        // ...

        String[] escuelas = new string[10];
        public Form1 f; 
        public ParejasAleatorias()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            using (TextWriter tw = new StreamWriter("impresion.txt"))
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
            using (TextWriter w = new StreamWriter("parejas.txt"))
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
                    w.Write("\n");
                }
            }
                MessageBox.Show("Datos exportados");
        }
        public void setForm1(Form1 tabla)
        {
            this.f = tabla; 
        }

        private void ParejasAleatorias_Load_1(object sender, EventArgs e)
        {
            IntPtr hmenu = GetSystemMenu(this.Handle, 0);
            int cnt = GetMenuItemCount(hmenu);

            // remove 'close' action
            RemoveMenu(hmenu, cnt - 1, MF_DISABLED | MF_BYPOSITION);

            // remove extra menu line
            RemoveMenu(hmenu, cnt - 2, MF_DISABLED | MF_BYPOSITION);

            DrawMenuBar(this.Handle);
        }
    }
}
