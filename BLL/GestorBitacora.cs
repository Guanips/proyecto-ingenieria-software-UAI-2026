using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GestorBitacora : IObserver
    {
        private RepositorioBitacora RepoBitacora { get; set; }

        public GestorBitacora ()
        {
            this.RepoBitacora = new RepositorioBitacora ();
        }

        public List<object> ConsultarBitacora ()
        {
            return (from a in RepoBitacora.ListarRegistros() select new { Usuario = a.Username,Fecha=a.Fecha,Accion=a.Accion}).ToList<object>();
        }

        public void Update(Usuario usuarioInvolucrado, string action)
        {
            Registro nRegistro = new Registro(usuarioInvolucrado.Username, DateTime.Now, action);
            RepoBitacora.AlmacenarRegistro (nRegistro, usuarioInvolucrado.Id.ToString());
        }
    }
}
