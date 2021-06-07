using System.IO;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    class Asiento
    {
        private Alumno alumno;
        private int numeroAsiento;

        public Asiento(Alumno alumno, int numeroAsiento)
        {
            this.alumno = alumno;
            this.numeroAsiento = numeroAsiento;
        }
        //getter
        public Alumno getAlumno()
        {
            return this.alumno;
        }

        public int getNumeroAsiento()
        {
            return this.numeroAsiento;
        }
    }
}
