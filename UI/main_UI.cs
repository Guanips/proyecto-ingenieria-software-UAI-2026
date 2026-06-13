using BLL;
using UI.Login;

namespace UI
{
    public partial class main_UI : Form
    {
        private GestorUsuarios _gestorUsuarios = new GestorUsuarios();
        private GestorBitacora _bitacora = new GestorBitacora();
        public main_UI()
        {
            InitializeComponent();
            _gestorUsuarios.Attach(_bitacora);
            this.IsMdiContainer = true;
            button1.Visible = false;
            dataGridView1.Visible = false;
            ValidarFormulario();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_iniciar_sesion Iniciar_Sesion = new UI_iniciar_sesion(_gestorUsuarios);
            Iniciar_Sesion.MdiParent = this;
            Iniciar_Sesion.Show();
        }

        public void ValidarFormulario()
        {
            this.iniciarSesionToolStripMenuItem.Enabled = !SessionManager.getInstance.Logged();
            this.cerrarSesionToolStripMenuItem.Enabled = SessionManager.getInstance.Logged();
            if (SessionManager.getInstance.Logged())
            {
                dataGridView1.DataSource = null;
                button1.Visible = true;
                dataGridView1.Visible = true;
            }
            else
            {
                button1.Visible = false;
                dataGridView1.Visible = false;

            }
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form hijo in this.MdiChildren)
            {
                hijo.Close();
            }
            _gestorUsuarios.LogOut();
            ValidarFormulario();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mostrar(dataGridView1,_bitacora.ConsultarBitacora());
        }
        private void Mostrar(DataGridView dgv, object o)
        {
            dgv.DataSource = null; dgv.DataSource = o;
        }
    }
}
