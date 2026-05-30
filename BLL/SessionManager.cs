using BE;

namespace BLL
{
    public class SessionManager
    {
        // implementacion del singleton
        private static SessionManager? Instance;
        private SessionManager () { }
        public static SessionManager getInstance
        {
            get
            {
                if (Instance == null) Instance = new SessionManager();
                return Instance;
            }
        }

        // logica especifica para nuestro caso
        private Usuario? UsuarioActivo;
        public Usuario? ObtenerUsuarioActivo()
        {
            return UsuarioActivo;
        }

        public void LogIn(Usuario usuario)
        {
            UsuarioActivo = usuario;
        }

        public void LogOut()
        {
            UsuarioActivo = null;
        }
        public bool Logged()
        {
            return UsuarioActivo != null;
        }
    }
}
