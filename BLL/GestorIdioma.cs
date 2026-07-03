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
    public class GestorIdioma : ISujeto
    {

        private static GestorIdioma? _instance;
        private static readonly object _lock = new object();

        private string _idiomaActual = "ES";
        private List<IObserver> ObserversAttached = new List<IObserver>();
        private RepositorioIdioma _repoIdioma = new RepositorioIdioma();

        private GestorIdioma() { }

        public static GestorIdioma GetInstance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = new GestorIdioma();
                    return _instance;
                }
            }
        }

        public string IdiomaActual => _idiomaActual;

        public Dictionary<string, string> ObtenerTraduccionesActuales(string codigoIdioma)
        {
            return _repoIdioma.ObtenerTraducciones(codigoIdioma);
        }

        public void Attach(IObserver observer)
        {
            if (!ObserversAttached.Contains(observer)) ObserversAttached.Add(observer);

            Usuario? activo = SessionManager.getInstance.ObtenerUsuarioActivo();
            // CORRECCIÓN: Se agregó el ', 0' al final del constructor
            Usuario dummy = activo ?? new Usuario(Guid.Empty, "invitado", "", "", "", false, _idiomaActual, 0);
            observer.Update(dummy, $"Idioma:{_idiomaActual}");
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

        public void CambiarIdioma(string nuevoIdioma)
        {
            _idiomaActual = nuevoIdioma;

            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo != null)
            {
                usuarioActivo.Idioma = nuevoIdioma;

                System.Data.DataSet ds = DAO.GetInstance.ObtenerDataSet();
                System.Data.DataTable dtUsuarios = ds.Tables["Usuario"];
                System.Data.DataRow[] foundRows = dtUsuarios.Select($"Username = '{usuarioActivo.Username}'");
                if (foundRows.Length > 0)
                {
                    foundRows[0]["Idioma"] = nuevoIdioma;
                    DAO.GetInstance.SubirCambiosBD();
                }

                Notificar(usuarioActivo, $"Idioma:{nuevoIdioma}");
            }
            else
            {
                // CORRECCIÓN: Se agregó el ', 0' al final del constructor
                Usuario temporal = new Usuario(Guid.Empty, "invitado", "", "", "", false, nuevoIdioma, 0);
                Notificar(temporal, $"Idioma:{nuevoIdioma}");
            }
        }
    }
}