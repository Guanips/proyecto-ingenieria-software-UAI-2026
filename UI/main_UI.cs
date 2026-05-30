using BLL;
using UI.Login;

namespace UI
{
    public partial class main_UI : Form
    {
        public main_UI()
        {
            InitializeComponent();
            ValidarFormulario();
            this.IsMdiContainer = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_iniciar_sesion Iniciar_Sesion = new UI_iniciar_sesion();
            Iniciar_Sesion.MdiParent = this;
            Iniciar_Sesion.Show();
        }

        private void ValidarFormulario()
        {
            this.iniciarSesionToolStripMenuItem.Enabled = !SessionManager.getInstance.Logged();
            this.cerrarSesionToolStripMenuItem.Enabled = SessionManager.getInstance.Logged();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ValidarFormulario();
            SessionManager.getInstance.LogOut();
        }
    }
}
