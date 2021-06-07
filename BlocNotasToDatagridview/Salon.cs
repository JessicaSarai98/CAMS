using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BlocNotasToDatagridview
{
    class Salon
    {
        private string id;
        private string nombre;
        private int filas;
        private int columnas;
        private Asiento[,] asientos;

        public Salon(string id, string nombre, int filas, int columnas)
        {
            this.id = id;
            this.nombre = nombre;
            this.filas = filas;
            this.columnas = columnas;
            this.asientos = new Asiento[filas, columnas];
        }
        //getter
        public string getId()
        {
            return this.id;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public int getFilas()
        {
            return this.filas;
        }

        public int getColumnas()
        {
            return this.columnas;
        }

        public Asiento[,] getAsientos()
        {
            return this.asientos;
        }

        public List<int> getAsientosVacios()
        {
            List<int> asientosVacios = new List<int>();
            for (int i=0; i<filas; i++)
            {
                for (int j=0; j<columnas; j++)
                {
                    if (asientos[i,j] == null)
                    {
                        int asiento = i * columnas + j;
                        asientosVacios.Add(asiento);
                    }
                }
            }
            return asientosVacios;
        }

        public bool puedoSentarmeAqui_A(Alumno alumno, int numeroDeAsiento)
        {

            return true;
        }

        public bool puedoSentarmeAqui_B(Alumno alumno, int numeroDeAsiento)
        {
            ArrayList asientosARevisar = this.crearAsientosARevisar(numeroDeAsiento);
            for (int i=0; i<asientosARevisar.Count; i++)
            {
                int fila = numeroDeAsiento/columnas;
                int columna = numeroDeAsiento%columnas;
                if (asientos[fila, columna] == null)//No hay nadie sentado
                {
                    return true;
                }
                //Hay alguien sentado
                if (alumno.getNombreEscuela().CompareTo(asientos[fila, columna].getAlumno().getNombreEscuela()) == 0)//Ver que sean de distintas escuelas
                {
                    return false; 
                }
            }
            return true;
        }

        public ArrayList crearAsientosARevisar(int posicionActual)
        {
            ArrayList asientosARevisar = new ArrayList();
            int arriba = posicionActual - columnas;
            int derecha = posicionActual + 1;
            int abajo = posicionActual + columnas;
            int izquierda = posicionActual - 1;

            int[] numeroAsientos = {arriba, derecha, abajo, izquierda};
            for (int i=0; i<numeroAsientos.Length; i++)
            {
                if (!esAsientoExcepcion(numeroAsientos[i], posicionActual))
                {
                    asientosARevisar.Add(numeroAsientos[i]);
                }
            }
            return asientosARevisar;
        }

        private bool esAsientoExcepcion(int numeroDeAsiento, int posicionActual)
        {
            if (asientoFueraDelRango(numeroDeAsiento))
            {
                return true;
            }
            if (asientoALadoDePared(numeroDeAsiento, posicionActual))
            {
                return true;
            }
            return false ; 
        }

        private bool asientoALadoDePared(int numeroDeAsiento, int posicionActual)
        {
            if ((posicionActual + 1)%columnas == 0 && numeroDeAsiento % columnas == 0)//No hay asiento a mi derecha
            {
                return true;
            }
            if (posicionActual%columnas == 0 && (numeroDeAsiento + 1)%columnas == 0)//No hay asiento a mi izquierda
            {
                return true;
            }
            return false;
        }

        private bool asientoFueraDelRango(int numeroDeAsiento)
        {
            if (numeroDeAsiento < 0 || numeroDeAsiento >= filas*columnas)
            {
                return true;
            }
            return false;
        }

        private List<Asiento> crearArrayDeAlumnos()
        {
            List<Asiento> alumnos = new List<Asiento>();
            for (int i=0; i<filas; i++)
            {
                for (int j=0; j<columnas; j++)
                {
                    if (asientos[i, j] != null)
                    {
                        alumnos.Add(asientos[i,j]);
                    }
                }
            }
            return alumnos;
        }

        public List<Asiento> generarLista()
        {
            List<Asiento> lista = crearArrayDeAlumnos();
            for (int i = 0; i <lista.Count; i++)
            {
                for (int j = 0; j < lista.Count - j - 1; j++)
                {
                    String alumno1 = RemoveDiacritics(lista[j].getAlumno().getNombreAlumno());
                    String alumno2 = RemoveDiacritics(lista[j + 1].getAlumno().getNombreAlumno());
                    if (alumno1.CompareTo(alumno2) > 0)
                    {
                        Asiento aux = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = aux;
                    }

                }
            }
            return lista;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
