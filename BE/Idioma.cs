using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idioma
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public Idioma(string codigo, string nombre)
        {
            Codigo = codigo;
            Nombre = nombre;
        }

        // Para que el ComboBox muestre el nombre lindo automáticamente
        public override string ToString() => Nombre;
    }
}
