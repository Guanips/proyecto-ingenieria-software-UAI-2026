using BE;
using DAL;
using servicios;

namespace BLL
{
    public class GestorLogin : ISujeto
    {
        public GestorLogin()
        {
            Attach(GestorBitacora.GetInstance);
        }

        private List<IObserver> ObserversAttached = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            ObserversAttached.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            ObserversAttached.Remove(observer);
        }

        public void Notificar(string username, string accion)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(username, accion);
            }
        }

        public void LogIn(string usuario, string pass)
        {
            RepositorioUsuarios RepoUsuarios = RepositorioUsuarios.GetInstance;
            if (!RepoUsuarios.VerificarExistenciaDeUsername(usuario)) throw new Exception("Usuario incorrecto");
            Usuario _user = RepoUsuarios.ObtenerUsuario(usuario);
            string hash = CryptoService.EncriptarPassword(pass);
            if (!CryptoService.Comparer(hash, _user.PasswordHash))
            {
                Notificar(_user.Username, "Intento de inicio de sesion fallido");
                throw new Exception("Contraseña incorrecta");
            }
            SessionManager.getInstance.LogIn(_user);
            Notificar(_user.Username, "Inicio De Sesion");
        }

        public void LogOut()
        {
            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo == null) throw new Exception("Usuario activo no encontrado en logout");

            Notificar(usuarioActivo.Username, "Cierre de Sesion");
            SessionManager.getInstance.LogOut();
        }

        public bool VerificarExistenciaUsuario(string username)
        {
            RepositorioUsuarios repoUsuarios = RepositorioUsuarios.GetInstance;
            return repoUsuarios.VerificarExistenciaDeUsername(username);
        }
    }
}
