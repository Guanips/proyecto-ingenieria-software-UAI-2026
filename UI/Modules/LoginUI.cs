using BE;
using BLL;
using servicios;
using System.Collections.Generic;

namespace UI.Login
{
    public partial class LoginUI : FormBaseObserver
    {

        private GestorLogin gestorLogin;
        public event EventHandler? SesionIniciada;
        //private RepositorioIdioma repoIdioma = new RepositorioIdioma();

        public LoginUI()
        {
            InitializeComponent();
            gestorLogin = new GestorLogin();
            
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
                    string msgUser = GestorIdioma.GetInstance.TraducirMensaje(usernameValidationResult.ErrorMessage, "Error de validación");
                    throw new Exception(msgUser);
                }

                if (isPasswordEmpty)
                {
                    string msgPass = GestorIdioma.GetInstance.TraducirMensaje("err_PassVacia", "La contraseña no puede estar vacía.");
                    throw new Exception(msgPass);
                }

                gestorLogin.LogIn(nUsername, nPassword);

                string mensaje = GestorIdioma.GetInstance.TraducirMensaje("msg_InicioSesionExito", "Inicio de sesión exitoso.");
                string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito");

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SesionIniciada?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error");
                MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
    }
}
