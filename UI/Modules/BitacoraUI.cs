using BE;
using BLL;

namespace UI.Modules
{
    public partial class BitacoraUI : Form
    {
        public BitacoraUI()
        {
            InitializeComponent();

            List<Registro> registros = GestorBitacora.GetInstance.ConsultarBitacoraCompleta();
            List<string> usernames = registros.Select(r => r.Username).Distinct().ToList();
            List<string> acciones = registros.Select(r => r.Accion).Distinct().ToList();

            dataGridViewRegistrosBitacora.DataSource = registros;

            comboBoxAccion.DataSource = acciones;
            comboBoxAccion.SelectedIndex = -1;

            comboBoxUsername.DataSource = usernames;
            comboBoxUsername.SelectedIndex = -1;
        }

        private void bitacoraUIButtonLimpiarFiltros_Click(object sender, EventArgs e)
        {
            dataGridViewRegistrosBitacora.DataSource = GestorBitacora.GetInstance.ConsultarBitacoraCompleta();
            comboBoxAccion.SelectedIndex = -1;
            comboBoxUsername.SelectedIndex = -1;
        }

        private void comboBoxAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxAccion.SelectedIndex;
            if (selectedIndex >= 0)
            {
                string selectedAccion = comboBoxAccion.SelectedItem.ToString();
                List<Registro> registrosFiltrados = GestorBitacora.GetInstance.ConsultarBitacoraFiltradaPorAccion(selectedAccion);
                dataGridViewRegistrosBitacora.DataSource = registrosFiltrados;
            }
        }

        private void comboBoxUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBoxUsername.SelectedIndex;
            if (selectedIndex >= 0)
            {
                string selectedUsername = comboBoxUsername.SelectedItem.ToString();
                List<Registro> registrosFiltrados = GestorBitacora.GetInstance.ConsultarBitacoraFiltradaPorUsername(selectedUsername);
                dataGridViewRegistrosBitacora.DataSource = registrosFiltrados;
            }
        }

        private void bitacoraUILabelComboBoxAccion_Click(object sender, EventArgs e)
        {

        }
    }
}
