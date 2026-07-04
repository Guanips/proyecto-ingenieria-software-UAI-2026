using BE;
using DAL;
using System.Data;

namespace BLL
{
    public class GestorHistorialUsuario
    {
        private RepositorioHistorialUsuario repoHistorialUsuario;

        public GestorHistorialUsuario()
        {
            repoHistorialUsuario = new RepositorioHistorialUsuario();
        }

        public CaretakerMementoUsuario ObtenerCaretakerUsuario(Usuario usuario)
        {
            CaretakerMementoUsuario caretaker = new CaretakerMementoUsuario();

            DataRow[] filasHistorial = repoHistorialUsuario.ObtenerHistorialPorUsuario(usuario.Id);

            foreach (DataRow fila in filasHistorial)
            {
                IMementoUsuario memento = HistorialUsuarioMapper.MapToMemento(fila);
                caretaker.AgregarMemento(memento);
            }

            return caretaker;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();
            return usuarios;
        }
    }
}
