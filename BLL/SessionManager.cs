using BE;

namespace BLL
{
    public class SessionManager
    {
        static BE_Usuario user;
        public static BE_Usuario User
        {
            get
            {
                return user;
            }
        }
        public static bool Logged()
        {
            return user != null;
        }
        public static void Login(BE_Usuario usuario)
        {
            user = usuario;
        }
        public static void Logout()
        {
            user = null;
        }
    }
}
