using BE;
using DAL;
using servicios;

namespace BLL
{
    public class GestorLogin : ISujeto
    {
        public GestorLogin() { }

        private List<IObserver> ObserversAttached = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            ObserversAttached.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            ObserversAttached.Remove(observer);
        }

        public void Notificar(Usuario usuarioInvolucrado, string accion)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(usuarioInvolucrado, accion);
            }
        }

        public void LogIn(string usuario, string pass)
        {
            RepositorioUsuarios RepoUsuarios = RepositorioUsuarios.GetInstance;
            if (!RepoUsuarios.VerificarExistenciaDeUsername(usuario)) throw new Exception("Usuario incorrecto");
            Usuario _user = RepoUsuarios.ObtenerUsuario(usuario);
            if (_user.EstaBloqueado) throw new Exception("El usuario se encuentra bloqueado. Contacte a un administrador para recuperar el acceso.");
            string hash = CryptoService.EncriptarPassword(pass);

            if (!CryptoService.Comparer(hash, _user.PasswordHash))
            {
                int nuevosIntentos = _user.IntentosFallidos + 1;
                RepoUsuarios.ActualizarIntentosFallidos(usuario, nuevosIntentos);

                if (nuevosIntentos >= 3)
                {
                    RepoUsuarios.CambiarEstadoBloqueo(usuario, true);
                    throw new Exception("Ha superado los 3 intentos fallidos. Su cuenta ha sido bloqueada por seguridad.");
                }

                throw new Exception($"Contraseña incorrecta. Le quedan {3 - nuevosIntentos} intentos antes de bloquearse.");
            }

            if (_user.IntentosFallidos > 0)
            {
                RepoUsuarios.ActualizarIntentosFallidos(usuario, 0);
            }

            SessionManager.getInstance.LogIn(_user);
            Notificar(_user, "Inicio De Sesion");
        }

        public void LogOut()
        {
            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo == null) throw new Exception("Usuario activo no encontrado en logout");

            Notificar(usuarioActivo, "Cierre de Sesion");
            SessionManager.getInstance.LogOut();
        }

        public bool VerificarExistenciaUsuario(string username)
        {
            RepositorioUsuarios repoUsuarios = RepositorioUsuarios.GetInstance;
            return repoUsuarios.VerificarExistenciaDeUsername(username);
        }
    }
}
