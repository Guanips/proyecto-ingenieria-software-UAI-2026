// Archivo: BitacoraUI.cs
using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI.Modules
{
    public partial class BitacoraUI : FormBaseObserver
    {
        private GestorBitacora gestorBitacora;
        private List<Registro> listaOriginal;

        public BitacoraUI()
        {
            InitializeComponent();
            gestorBitacora = new GestorBitacora();
            this.Load += BitacoraUI_Load;
        }

        private void BitacoraUI_Load(object? sender, EventArgs e)
        {
            // Cargamos la lista real desde la base de datos
            listaOriginal = gestorBitacora.ConsultarBitacora();
            CargarFiltrosComboBox();
            AplicarFiltros();
        }

        private void CargarFiltrosComboBox()
        {
            comboFiltroAccion.Items.Clear();
            comboFiltroAccion.Items.Add("Todas");
            comboFiltroAccion.Items.Add("LOG_LOGIN");
            comboFiltroAccion.Items.Add("LOG_LOGOUT");
            comboFiltroAccion.Items.Add("LOG_USER_ADD");
            comboFiltroAccion.Items.Add("LOG_USER_MOD");
            comboFiltroAccion.Items.Add("LOG_USER_DEL");
            comboFiltroAccion.Items.Add("LOG_PERFIL_ADD");
            comboFiltroAccion.Items.Add("LOG_PERMISOS_MOD");

            comboFiltroAccion.SelectedIndex = 0;

            // Enganchamos los eventos para que filtren automáticamente al escribir/seleccionar
            comboFiltroAccion.SelectedIndexChanged += (s, e) => AplicarFiltros();
            textBoxFiltroUsuario.TextChanged += (s, e) => AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (listaOriginal == null) return;

            // 1. Empezamos con la lista completa
            var registrosFiltrados = listaOriginal.AsEnumerable();

            // 2. Filtramos por nombre de usuario (ignorando mayúsculas/minúsculas)
            string usrFiltro = textBoxFiltroUsuario.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(usrFiltro))
            {
                registrosFiltrados = registrosFiltrados.Where(r => r.Username.ToLower().Contains(usrFiltro));
            }

            // 3. Filtramos por Acción
            string accionSeleccionada = comboFiltroAccion.SelectedItem?.ToString() ?? "Todas";
            if (accionSeleccionada != "Todas")
            {
                registrosFiltrados = registrosFiltrados.Where(r => r.Accion == accionSeleccionada);
            }

            // 4. Proyectamos y TRADUCIMOS la acción para mostrarla en la grilla
            var listaMostrar = registrosFiltrados.Select(r => new
            {
                Usuario = r.Username,
                FechaHora = r.Fecha,
                // Traducimos la Key (ej: "LOG_LOGIN") al texto correspondiente (ej: "Inicio de sesión")
                AccionRealizada = GestorIdioma.GetInstance.TraducirMensaje(r.Accion, r.Accion)
            }).OrderByDescending(x => x.FechaHora).ToList();

            dataGridViewBitacora.DataSource = null;
            dataGridViewBitacora.DataSource = listaMostrar;

            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            textBoxFiltroUsuario.Text = string.Empty;
            comboFiltroAccion.SelectedIndex = 0;
            AplicarFiltros();
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            var traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

            if (dataGridViewBitacora.Columns.Count > 0)
            {
                if (traducciones.ContainsKey("GridBitacora_Usuario") && dataGridViewBitacora.Columns["Usuario"] != null)
                    dataGridViewBitacora.Columns["Usuario"].HeaderText = traducciones["GridBitacora_Usuario"];

                if (traducciones.ContainsKey("GridBitacora_Fecha") && dataGridViewBitacora.Columns["FechaHora"] != null)
                    dataGridViewBitacora.Columns["FechaHora"].HeaderText = traducciones["GridBitacora_Fecha"];

                if (traducciones.ContainsKey("GridBitacora_Accion") && dataGridViewBitacora.Columns["AccionRealizada"] != null)
                    dataGridViewBitacora.Columns["AccionRealizada"].HeaderText = traducciones["GridBitacora_Accion"];
            }
        }
    }
}