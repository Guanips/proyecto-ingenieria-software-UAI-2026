using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Bitacora : IObserver
    {
        private List<Registro> ListaRegistros { get; set; }
        private RepositorioBitacora RepoBitacora { get; set; }

        public Bitacora ()
        {
            this.ListaRegistros = new List<Registro> ();
            this.RepoBitacora = new RepositorioBitacora ();
            //ListaRegistros = RepoBitacora.ListarRegistros();
        }

        public List<Registro> ConsultarBitacora ()
        {
            return this.ListaRegistros;
        }

        public void Update(string username, string action)
        {
            Registro nRegistro = new Registro(username, DateTime.Now, action);
            ListaRegistros.Add (nRegistro);
            RepoBitacora.AlmacenarRegistro (nRegistro);
        }
    }
}
