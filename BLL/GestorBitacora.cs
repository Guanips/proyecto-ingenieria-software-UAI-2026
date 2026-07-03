using BE;
using DAL;

namespace BLL
{
    public class GestorBitacora : IObserver
    {
        private RepositorioBitacora RepoBitacora { get; set; }

        public GestorBitacora()
        {
            this.RepoBitacora = new RepositorioBitacora();
        }

        public List<Registro> ConsultarBitacora()
        {
            return RepoBitacora.ListarRegistros();
        }

        public void Update(Usuario usuarioInvolucrado, string action)
        {
            Registro nRegistro = new Registro(usuarioInvolucrado.Username, DateTime.Now, action);
            RepoBitacora.AlmacenarRegistro(nRegistro, usuarioInvolucrado.Id.ToString());
        }
    }
}
