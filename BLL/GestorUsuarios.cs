using BE;
using DAL;
using servicios;

namespace BLL
{
    public class GestorUsuarios : ISujeto
    {
        private List<IObserver> ObserversAttached = new List<IObserver>();

        public GestorUsuarios()
        {
            Attach(GestorBitacora.GetInstance);
        }

        public void Attach(IObserver observer)
        {
            ObserversAttached.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            IObserver? foundObserver = ObserversAttached.Find((item) => item == observer);
            if (foundObserver == null) throw new Exception("Observer no agregado");
            ObserversAttached.Remove(observer);
        }

        public void Notificar(string username, string action)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(username, action);
            }
        }

        public List<Usuario> ListarUsuarios()
        {
            RepositorioUsuarios repositorioUsuarios = RepositorioUsuarios.GetInstance;
            return repositorioUsuarios.ObtenerListadoTotalUsuarios();
        }

        public void RegistrarUsuario(string nUsername, string nPassword, string nEmail, string nNumTelefono)
        {
            bool usuarioExiste = RepositorioUsuarios.GetInstance.VerificarExistenciaDeUsername(nUsername);
            if (usuarioExiste) throw new Exception("El usuario ya existe");

            string passwordHash = CryptoService.EncriptarPassword(nPassword);

            Usuario nuevoUsuario = new Usuario(Guid.NewGuid(), nUsername, passwordHash, nEmail, nNumTelefono, false);
            RepositorioUsuarios.GetInstance.AgregarUsuario(nuevoUsuario);
            Notificar(SessionManager.getInstance.ObtenerUsuarioActivo()!.Username, "Registro de usuario");
        }

        public void EliminarUsuario(string nUsername)
        {
            RepositorioUsuarios.GetInstance.EliminarUsuario(nUsername);
            Notificar(SessionManager.getInstance.ObtenerUsuarioActivo()!.Username, "Eliminación de usuario");
        }

        public void ModificarUsuario(string nUsername, string nEmail, string nNumTelefono)
        {
            Usuario usuarioObtenido = RepositorioUsuarios.GetInstance.ObtenerUsuario(nUsername);
            Usuario usuarioModificado = new Usuario(usuarioObtenido.Id, nUsername, usuarioObtenido.PasswordHash, nEmail, nNumTelefono, usuarioObtenido.EstaBloqueado);
            RepositorioUsuarios.GetInstance.ModificarUsuario(usuarioModificado);
            Notificar(SessionManager.getInstance.ObtenerUsuarioActivo()!.Username, "Modificación de usuario");
        }
    }
}
