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

        private GestorUsuarios gestor;
        public UI_iniciar_sesion(GestorUsuarios gestorCompartido)
        {
            InitializeComponent();
            gestor = gestorCompartido;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {

                string _user = textBox1.Text;
                string _password = textBox2.Text;

                gestor.LogIn(_user, _password);

                if (this.MdiParent is main_UI formPadre)
                {
                    
                    formPadre.ValidarFormulario();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }
    }
}
