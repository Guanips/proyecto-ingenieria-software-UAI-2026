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
        //private List<Registro> ListaRegistros { get; set; }
        private RepositorioBitacora RepoBitacora { get; set; }

        public Bitacora ()
        {
            //this.ListaRegistros = new List<Registro> ();
            this.RepoBitacora = new RepositorioBitacora ();
            //ListaRegistros = RepoBitacora.ListarRegistros();
        }

        public List<object> ConsultarBitacora ()
        {
            return (from a in RepoBitacora.ListarRegistros() select new { Usuario = a.Username,Fecha=a.Fecha,Accion=a.Accion}).ToList<object>();
        }

        public void Update(string username, string action)
        {
            Registro nRegistro = new Registro(username, DateTime.Now, action);
            //ListaRegistros.Add (nRegistro);
            RepoBitacora.AlmacenarRegistro (nRegistro);
        }
    }
}
