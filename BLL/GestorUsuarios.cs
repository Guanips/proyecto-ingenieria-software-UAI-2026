using BE;
using DAL;
using servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GestorUsuarios : ISujeto
    {
        private List<IObserver> ObserversAttached = new List<IObserver>();
        private RepositorioUsuarios RepoUsuarios = new RepositorioUsuarios();

        public GestorUsuarios () { }

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

        public void Notificar(Usuario usuarioInvolucrado, string action)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(usuarioInvolucrado, action)
;           }
        }

        public void LogIn(string usuario, string pass)
        {
            if (!RepoUsuarios.VerificarUsuarioPorNombre(usuario)) throw new Exception("Usuario incorrecto");
            Usuario _user = RepoUsuarios.ObtenerUsuarioPorNombre(usuario);
            string hash = CryptoService.EncriptarPassword(pass);
            if (!CryptoService.Comparer(hash, _user.PasswordHash)) throw new Exception("Contraseña incorrecta");
            SessionManager.getInstance.LogIn(_user);
            Notificar(_user, "Inicio De Sesion");
        }

        public void LogOut() 
        {
            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo == null) throw new Exception("Usuario activo no encontrado en logout");

            Notificar(usuarioActivo,"Cierre de Sesion");
            SessionManager.getInstance.LogOut();
        }
    }
}
