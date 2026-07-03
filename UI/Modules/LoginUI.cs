using BE;
using BLL;
using servicios;
using System.Collections.Generic;

namespace UI.Login
{
    public partial class LoginUI : FormBaseObserver, IObserver
    {

        private GestorLogin gestorLogin;
        public event EventHandler? SesionIniciada;
        //private RepositorioIdioma repoIdioma = new RepositorioIdioma();

        public LoginUI()
        {
            InitializeComponent();
            gestorLogin = new GestorLogin();
            GestorIdioma.GetInstance.Attach(this);
        }

        private void loginUIButtonIniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                string nUsername = textBoxUsername.Text;
                string nPassword = textBoxContrasena.Text;

                ValidationResult usernameValidationResult = FormFieldValidationService.ValidateUsername(nUsername);
                bool isPasswordEmpty = string.IsNullOrWhiteSpace(nPassword);

                if (!usernameValidationResult.IsValid)
                {
                    throw new Exception(usernameValidationResult.ErrorMessage);
                }

                if (isPasswordEmpty)
                {
                    throw new Exception("La contraseña no puede estar vacía.");
                }

                gestorLogin.LogIn(nUsername, nPassword);

                MessageBox.Show("Inicio de sesión exitoso.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SesionIniciada?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //public void Update(Usuario usuarioInvolucrado, string action)
        //{
        //    if (action.StartsWith("Idioma:"))
        //    {
        //        string codigoIdioma = action.Split(':')[1];
        //        var traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

        //        // Al servicio no le importa qué es 'this', lo inspecciona dinámicamente en caliente
        //        TranslateServices.TraducirObjeto(this, traducciones);
        //    }
        //}
        public void Update(Usuario usuarioInvolucrado, string action)
        {
            if (action.StartsWith("Idioma:"))
            {
                string codigoIdioma = action.Split(':')[1];

                Dictionary<string, string> traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

                TranslateServices.TraducirObjeto(this, traducciones);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            GestorIdioma.GetInstance.Detach(this);
            base.OnFormClosed(e);
        }
    }
}
