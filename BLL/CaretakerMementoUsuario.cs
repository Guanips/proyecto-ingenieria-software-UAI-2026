using BE;

namespace BLL
{
    public class CaretakerMementoUsuario
    {
        private List<IMementoUsuario> historialCambios;

        public CaretakerMementoUsuario()
        {
            historialCambios = new List<IMementoUsuario>();
        }

        public void AgregarMemento(IMementoUsuario memento)
        {
            historialCambios.Add(memento);
        }

        public List<IMementoUsuario> ObtenerHistorial()
        {
            return historialCambios;
        }
    }
}
