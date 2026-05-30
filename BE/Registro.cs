using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Registro
    {
        public string Username { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Accion { get; private set; }

        public Registro(string nUsername, DateTime nFecha, string nAccion) 
        {
            this.Username = nUsername;
            this.Fecha = nFecha;
            this.Accion = nAccion;
        }
        public Registro(object[] ob) : this((string)ob[0], (DateTime)ob[1], (string)ob[2]) { }
    }
}
