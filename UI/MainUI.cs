using BE;
using BLL;
using servicios;
using UI.Login;
using UI.Modules;

namespace UI
{
    public partial class MainUI : Form, IObserver
    {
        private Form? formCargadoActualmente;

        public MainUI()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            GestorIdioma.GetInstance.Attach(this);
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Enabled = false;
            }
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
            try
            {
                mainUIStripMenuItemInicio.Enabled = true;
                mainUIStripMenuItemCerrarSesion.Enabled = true;
                mainUIStripMenuItemIniciarSesion.Enabled = false;

                Usuario? usuarioActual = SessionManager.getInstance.ObtenerUsuarioActivo();

                if (usuarioActual != null)
                {
                    mainUIStripMenuItemGestionDeUsuarios.Enabled = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-USR"));
                    mainUIStripMenuItemGestionDePerfiles.Enabled = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-PERFIL"));
                    mainUIStripMenuItemBitacora.Enabled = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-CONSULTA-BIT"));
                }
                else
                {
                    throw new Exception("Login error");
                }

                formCargadoActualmente?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);

            mainUIStripMenuItemCerrarSesion.Enabled = false;

            var listaIdiomas = GestorIdioma.GetInstance.ObtenerIdiomasDisponibles();

            comboIdiomasGlobal.SelectedIndexChanged -= ComboIdiomasGlobal_SelectedIndexChanged;

            comboIdiomasGlobal.DataSource = listaIdiomas;
            comboIdiomasGlobal.DisplayMember = "Nombre";
            comboIdiomasGlobal.ValueMember = "Codigo";

            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo != null && !string.IsNullOrEmpty(usuarioActivo.Idioma))
            {
                comboIdiomasGlobal.SelectedValue = usuarioActivo.Idioma;
            }
            else
            {
                comboIdiomasGlobal.SelectedValue = "ES";
            }
            comboIdiomasGlobal.SelectedIndexChanged += ComboIdiomasGlobal_SelectedIndexChanged;

        }

        private void mainUIStripMenuItemIniciarSesion_Click(object sender, EventArgs e)
        {
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);
        }

        private void mainUIStripMenuItemCerrarSesion_Click(object sender, EventArgs e)
        {
            //SessionManager sessionManager = SessionManager.getInstance;
            //sessionManager.LogOut();

            //string mensaje = GestorIdioma.GetInstance.TraducirMensaje("msg_CierreSesionExito", "Sesión cerrada correctamente.");
            //string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloCierreSesion", "Cerrar sesión");

            //MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

            ////MessageBox.Show("Sesión cerrada correctamente.", "Cerrar sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //LoginUI loginUI = new LoginUI();
            //cargarFormulario(loginUI);
            //foreach (ToolStripMenuItem item in menuStrip1.Items)
            //{
            //    item.Enabled = false;
            //}
            //mainUIStripMenuItemCerrarSesion.Enabled = false;
            //mainUIStripMenuItemIniciarSesion.Enabled = true;
            GestorLogin gestorLogin = new GestorLogin();

            // 2. Ejecutamos el cierre a través del gestor para disparar la notificación
            gestorLogin.LogOut();

            string mensaje = GestorIdioma.GetInstance.TraducirMensaje("msg_CierreSesionExito", "Sesión cerrada correctamente.");
            string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloCierreSesion", "Cerrar sesión");

            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);

            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.Enabled = false;
            }

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

            BE.Idioma idiomaSeleccionado = (BE.Idioma)comboIdiomasGlobal.SelectedItem;

            GestorIdioma.GetInstance.CambiarIdioma(idiomaSeleccionado.Codigo);
        }

        public void Update(string username, string action)
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
