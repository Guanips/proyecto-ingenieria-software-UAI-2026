using BE;
using BLL;
using servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Login
{
    public partial class UI_iniciar_sesion : Form
    {

        GestorUsuarios gestor;
        public UI_iniciar_sesion()
        {
            InitializeComponent();
            gestor = new GestorUsuarios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                main_UI frm = (main_UI)this.MdiParent;
                string _user = textBox1.Text;
                string _password = textBox2.Text;

                gestor.LogIn(_user, _password);
                //if(usuario == null) SessionManager.getInstance.LogIn(usuario);


                //Consultar Bitacora


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
