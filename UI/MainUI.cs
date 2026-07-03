using BE;
using BLL;
using servicios;
using UI.Login;
using UI.Modules;

namespace UI
{
    public partial class MainUI : Form,IObserver
    {
        private Form? formCargadoActualmente;

        public MainUI()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            GestorIdioma.GetInstance.Attach(this);
        }

        private void cargarFormulario(Form formulario)
        {
            if (formCargadoActualmente != null)
            {
                formCargadoActualmente.Dispose();
                formCargadoActualmente = null;
            }
            formCargadoActualmente = formulario;

            if (formulario is LoginUI loginUI)
            {
                loginUI.SesionIniciada += LoginUI_SesionIniciada;
            }

            formCargadoActualmente.MdiParent = this;
            formCargadoActualmente.Show();
        }

        private void LoginUI_SesionIniciada(object? sender, EventArgs e)
        {
            mainUIStripMenuItemCerrarSesion.Enabled = true;
            mainUIStripMenuItemIniciarSesion.Enabled = false;
            formCargadoActualmente?.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);

            mainUIStripMenuItemCerrarSesion.Enabled = false;
        }

        private void mainUIStripMenuItemIniciarSesion_Click(object sender, EventArgs e)
        {
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);
        }

        private void mainUIStripMenuItemCerrarSesion_Click(object sender, EventArgs e)
        {
            SessionManager sessionManager = SessionManager.getInstance;
            sessionManager.LogOut();
            MessageBox.Show("Sesión cerrada correctamente.", "Cerrar sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);
            mainUIStripMenuItemCerrarSesion.Enabled = false;
            mainUIStripMenuItemIniciarSesion.Enabled = true;
        }

        private void mainUIStripMenuItemABMUsuarios_Click(object sender, EventArgs e)
        {
            GestionUsuariosUI gestorUsuariosUI = new GestionUsuariosUI();
            cargarFormulario(gestorUsuariosUI);
        }

        private void mainUIStripMenuItemDesbloqueoUsuarios_Click(object sender, EventArgs e)
        {
            BloqueoUsuariosUI bloqueoUsuariosUI = new BloqueoUsuariosUI();
            cargarFormulario(bloqueoUsuariosUI);
        }

        private void mainUIStripMenuItemABMPerfiles_Click(object sender, EventArgs e)
        {
            GestionPerfilesUI gestionPerfilesUI = new GestionPerfilesUI();
            cargarFormulario(gestionPerfilesUI);
        }

        private void mainUIStripMenuItemConsultarBitacora_Click(object sender, EventArgs e)
        {
            BitacoraUI bitacoraUI = new BitacoraUI();
            cargarFormulario(bitacoraUI);
        }
        
        private void ComboIdiomasGlobal_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboIdiomasGlobal.SelectedItem == null) return;
            string idiomaSeleccionado = comboIdiomasGlobal.SelectedItem.ToString()!;

            GestorIdioma.GetInstance.CambiarIdioma(idiomaSeleccionado);
        }

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
