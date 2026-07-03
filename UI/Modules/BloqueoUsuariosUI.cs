using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI.Modules
{
    public partial class BloqueoUsuariosUI : FormBaseObserver
    {
        private GestorBloqueoUsuarios gestorBloqueos;

        public BloqueoUsuariosUI()
        {
            InitializeComponent();
            gestorBloqueos = new GestorBloqueoUsuarios();
            this.Load += BloqueoUsuariosUI_Load;
        }

        private void BloqueoUsuariosUI_Load(object? sender, EventArgs e)
        {
            CargarGridBloqueados();
        }

        private void CargarGridBloqueados()
        {
            dataGridViewBloqueados.DataSource = null;
            dataGridViewBloqueados.DataSource = gestorBloqueos.ObtenerUsuariosBloqueados();

            if (dataGridViewBloqueados.Columns["PasswordHash"] != null)
                dataGridViewBloqueados.Columns["PasswordHash"].Visible = false;

            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBloqueados.SelectedRows.Count < 1)
                {
                    string msgValidacion = GestorIdioma.GetInstance.TraducirMensaje("err_NoUserDesbloquear", "No se ha seleccionado ningún usuario para desbloquear.");
                    throw new Exception(msgValidacion);
                }

                Usuario seleccionado = (Usuario)dataGridViewBloqueados.SelectedRows[0].DataBoundItem;

                gestorBloqueos.DesbloquearUsuario(seleccionado.Username);

                string msgExito = GestorIdioma.GetInstance.TraducirMensaje("msg_DesbloqueoExito", "Usuario desbloqueado correctamente.");
                string tituloExito = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito");
                MessageBox.Show(msgExito, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGridBloqueados();
            }
            catch (Exception ex)
            {
                string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error");
                MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            var traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

            // Reutilizamos las keys que ya tenés en la BD para las columnas del DataGridView
            if (dataGridViewBloqueados.Columns.Count > 0)
            {
                if (traducciones.ContainsKey("GridUsuario_Username") && dataGridViewBloqueados.Columns["Username"] != null)
                    dataGridViewBloqueados.Columns["Username"].HeaderText = traducciones["GridUsuario_Username"];

                if (traducciones.ContainsKey("GridUsuario_Email") && dataGridViewBloqueados.Columns["Email"] != null)
                    dataGridViewBloqueados.Columns["Email"].HeaderText = traducciones["GridUsuario_Email"];
            }
        }
    }
}
