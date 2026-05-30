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

        public void Notificar(string username, string action)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(username, action)
;           }
        }

        public void LogIn(string usuario, string pass)
        {

            if (!RepoUsuarios.VerificarUsuarioPorNombre(usuario)) throw new Exception("Usuario incorrecto");
            Usuario _user = RepoUsuarios.ObtenerUsuarioPorNombre(usuario);
            string hash = CryptoService.EncriptarPassword(pass);
            if (!CryptoService.Comparer(hash, _user.Passwordhash)) throw new Exception("Contraseña incorrecta");
            SessionManager.getInstance.LogIn(_user);


        }

        public void LogOut() 
        {
            SessionManager.getInstance.LogOut();
        }
    }
}
