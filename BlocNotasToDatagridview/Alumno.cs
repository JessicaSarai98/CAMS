using System.IO;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    class Alumno
    {
        private string nombreEscuela;
        private string folio;
        private string nombreAlumno;

        public Alumno(string nombreEscuela, string folio, string nombreAlumno)
        {
            this.nombreEscuela = nombreEscuela;
            this.folio = folio;
            this.nombreAlumno = nombreAlumno;
        }
        //setter no son necesarios ya que la informacion para crear a un alumno obligatoriamente será con todos sus atributos
        //getter
        public string getNombreEscuela()
        {
            return this.nombreEscuela;
        }

        public string getFolio()
        {
            return this.folio;
        }

        public string getNombreAlumno()
        {
            return this.nombreAlumno;
        }
    }
}
