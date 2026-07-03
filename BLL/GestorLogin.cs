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
            if (!RepoUsuarios.VerificarExistenciaDeUsername(usuario))
            {
                string msg = GestorIdioma.GetInstance.TraducirMensaje("err_UsuarioIncorrecto", "Usuario incorrecto");
                throw new Exception(msg);
            }
            Usuario _user = RepoUsuarios.ObtenerUsuario(usuario);
            if (_user.EstaBloqueado)
            {
                string msg = GestorIdioma.GetInstance.TraducirMensaje("err_UsuarioBloqueado", "El usuario se encuentra bloqueado. Contacte a un administrador para recuperar el acceso.");
                throw new Exception(msg);
            }
            string hash = CryptoService.EncriptarPassword(pass);

            
            if (!CryptoService.Comparer(hash, _user.PasswordHash))
            {
                int nuevosIntentos = _user.IntentosFallidos + 1;
                RepoUsuarios.ActualizarIntentosFallidos(usuario, nuevosIntentos);

                if (nuevosIntentos >= 3)
                {
                    RepoUsuarios.CambiarEstadoBloqueo(usuario, true);
                    string msgBloqueo = GestorIdioma.GetInstance.TraducirMensaje("err_MaxIntentos", "Ha superado los 3 intentos fallidos. Su cuenta ha sido bloqueada por seguridad.");
                    throw new Exception(msgBloqueo);
                }

                string msgIntentos = GestorIdioma.GetInstance.TraducirMensaje("err_QuedanIntentos", "Contraseña incorrecta. Le quedan {0} intentos antes de bloquearse.");
                throw new Exception(string.Format(msgIntentos, 3 - nuevosIntentos));
            }

            if (_user.IntentosFallidos > 0)
            {
                RepoUsuarios.ActualizarIntentosFallidos(usuario, 0);
            }

            string logInicio = GestorIdioma.GetInstance.TraducirMensaje("log_InicioSesion", "Inicio De Sesion");
            
            SessionManager.getInstance.LogIn(_user);
            Notificar(_user, "LOG_LOGIN");
        }

        public void LogOut()
        {
            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo == null)
            {
                string msgNoUser = GestorIdioma.GetInstance.TraducirMensaje("err_NoUserLogout", "Usuario activo no encontrado en logout");
                throw new Exception(msgNoUser);
            }

            string logCierre = GestorIdioma.GetInstance.TraducirMensaje("log_CierreSesion", "Cierre de Sesion");
            
            Notificar(usuarioActivo, "LOG_LOGOUT");
            SessionManager.getInstance.LogOut();
        }

        public bool VerificarExistenciaUsuario(string username)
        {
            RepositorioUsuarios repoUsuarios = RepositorioUsuarios.GetInstance;
            return repoUsuarios.VerificarExistenciaDeUsername(username);
        }
    }
}
