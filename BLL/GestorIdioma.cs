using BE;
using DAL;

namespace BLL
{
    public class GestorIdioma : ISujeto
    {

        private static GestorIdioma? _instance;
        private static readonly object _lock = new object();

        private string _idiomaActual = "ES";
        private List<IObserver> ObserversAttached = new List<IObserver>();
        private RepositorioIdioma _repoIdioma = new RepositorioIdioma();
        public List<Idioma> ObtenerIdiomasDisponibles()
        {
            // Llamamos al repositorio que lee directamente de la tabla Idioma de la BD
            return _repoIdioma.ObtenerTodosLosIdiomas();
        }

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
            observer.Update(dummy.Username, $"Idioma:{_idiomaActual}");
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
        public void RegistrarNuevoIdioma(string codigo, string nombre, Dictionary<string, string> traducciones)
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre))
            {
                string msgError = TraducirMensaje("err_CodigoNombreObligatorios", "El código y el nombre del idioma son obligatorios.");
                throw new Exception(msgError);
            }

            codigo = codigo.Trim().ToUpper();
            nombre = nombre.Trim();

            bool yaExiste = ObtenerIdiomasDisponibles().Any(i => i.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (yaExiste)
            {
                string msgError = TraducirMensaje("err_IdiomaYaExiste", "El código de idioma ya se encuentra registrado en el sistema.");
                throw new Exception(msgError);
            }

            if (traducciones == null || traducciones.Count == 0)
            {
                string msgError = TraducirMensaje("err_TraduccionObligatoria", "Debe proveer al menos una traducción para el nuevo idioma.");
                throw new Exception(msgError);
            }

            Idioma nuevoIdioma = new Idioma(codigo, nombre);

            _repoIdioma.GuardarNuevoIdiomaConTraducciones(nuevoIdioma, traducciones);

            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo != null)
            {
                Notificar(usuarioActivo.Username, $"LOG_IDIOMA_ADD:{codigo}");
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

                Notificar(usuarioActivo.Username, $"Idioma:{nuevoIdioma}");
            }
            else
            {
                // CORRECCIÓN: Se agregó el ', 0' al final del constructor
                Usuario temporal = new Usuario(Guid.Empty, "invitado", "", "", "", false, nuevoIdioma, 0);
                Notificar(temporal.Username, $"Idioma:{nuevoIdioma}");
            }
        }
        public string TraducirMensaje(string keyEtiqueta, string mensajePorDefecto)
        {
            var traducciones = _repoIdioma.ObtenerTraducciones(_idiomaActual);

            if (traducciones.ContainsKey(keyEtiqueta))
            {
                return traducciones[keyEtiqueta];
            }
            return mensajePorDefecto;
        }
    }
}