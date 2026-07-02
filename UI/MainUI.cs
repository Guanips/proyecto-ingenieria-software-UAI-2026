using BLL;
using UI.Login;
using UI.Modules;

namespace UI
{
    public partial class MainUI : Form
    {
        private Form? formCargadoActualmente;

        public MainUI()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
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
    }
}
