using BE;
using BLL;
using servicios;

namespace UI.Modules
{
    public partial class GestionUsuariosUI : FormBaseObserver
    {
        private GestorUsuarios gestorUsuarios;

        public GestionUsuariosUI()
        {
            InitializeComponent();
            gestorUsuarios = new GestorUsuarios();
        }
        private void CargarGridUsuarios()
        {
            dataGridViewListadoUsuarios.DataSource = null;
            List<Usuario> usuarios = gestorUsuarios.ListarUsuarios();

            var usuariosFiltrados = usuarios
                .Where(u => !u.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                .ToList();

            dataGridViewListadoUsuarios.DataSource = usuariosFiltrados;

            if (dataGridViewListadoUsuarios.Columns["PasswordHash"] != null)
            {
                dataGridViewListadoUsuarios.Columns["PasswordHash"].Visible = false;
            }

            string idiomaActual = GestorIdioma.GetInstance.IdiomaActual;

            TraducirElementosParticulares(idiomaActual);
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            var traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

            if (dataGridViewListadoUsuarios.Columns.Count > 0)
            {
                if (traducciones.ContainsKey("GridUsuario_Username") && dataGridViewListadoUsuarios.Columns["Username"] != null)
                    dataGridViewListadoUsuarios.Columns["Username"].HeaderText = traducciones["GridUsuario_Username"];

                if (traducciones.ContainsKey("GridUsuario_Telefono") && dataGridViewListadoUsuarios.Columns["NumTelefono"] != null)
                    dataGridViewListadoUsuarios.Columns["NumTelefono"].HeaderText = traducciones["GridUsuario_Telefono"];

                if (traducciones.ContainsKey("GridUsuario_Email") && dataGridViewListadoUsuarios.Columns["Email"] != null)
                    dataGridViewListadoUsuarios.Columns["Email"].HeaderText = traducciones["GridUsuario_Email"];

                if (traducciones.ContainsKey("GridUsuario_Bloqueado") && dataGridViewListadoUsuarios.Columns["EstaBloqueado"] != null)
                    dataGridViewListadoUsuarios.Columns["EstaBloqueado"].HeaderText = traducciones["GridUsuario_Bloqueado"];

                if (traducciones.ContainsKey("GridUsuario_Id") && dataGridViewListadoUsuarios.Columns["Id"] != null)
                    dataGridViewListadoUsuarios.Columns["Id"].HeaderText = traducciones["GridUsuario_Id"];
            }
        }

        private void GestionUsuariosUI_Load(object sender, EventArgs e)
        {
            CargarGridUsuarios();
        }

        private void dataGridViewListadoUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewListadoUsuarios.SelectedRows.Count < 1) return;
            DataGridViewRow? selectedRow = dataGridViewListadoUsuarios.SelectedRows[0];
            if (selectedRow != null && selectedRow.DataBoundItem is Usuario selectedUsuario)
            {
                textBoxModificacionEmail.Text = selectedUsuario.Email;
                textBoxModificacionNumTelefono.Text = selectedUsuario.NumTelefono;
            }
            else
            {
                textBoxModificacionEmail.Text = "";
                textBoxModificacionNumTelefono.Text = "";
            }

        }

        private void gestionUsuariosUIButtonConfirmarRegistrarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string nUsername = textBoxRegistroUsername.Text;
                string nPassword = textBoxRegistroContrasena.Text;
                string nPasswordConfirmacion = textBoxRegistroRepetirConstrasena.Text;
                string nEmail = textBoxRegistroEmail.Text;
                string nNumTelefono = textBoxRegistroNumTelefono.Text;

                ValidationResult usernameValidationResult = FormFieldValidationService.ValidateUsername(nUsername);
                ValidationResult emailVerificationResult = FormFieldValidationService.ValidateEmail(nEmail);
                ValidationResult phoneVerificationResult = FormFieldValidationService.ValidatePhone(nNumTelefono);

                if (!usernameValidationResult.IsValid)
                {
                    throw new Exception(usernameValidationResult.ErrorMessage);
                }

                if (!emailVerificationResult.IsValid)
                {
                    throw new Exception(emailVerificationResult.ErrorMessage);
                }

                if (!phoneVerificationResult.IsValid)
                {
                    throw new Exception(phoneVerificationResult.ErrorMessage);
                }

                if (string.IsNullOrEmpty(nPassword) || string.IsNullOrEmpty(nPasswordConfirmacion))
                {
                    throw new Exception("La contraseña no puede estar vacía.");
                }

                if (nPassword != nPasswordConfirmacion)
                {
                    throw new Exception("Las contraseñas no coinciden.");
                }

                gestorUsuarios.RegistrarUsuario(nUsername, nPassword, nEmail, nNumTelefono, "ES");

                CargarGridUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gestionUsuariosUIModificacionButtonConfirmarModificarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewListadoUsuarios.SelectedRows.Count < 1) throw new Exception("No se ha seleccionado ningún usuario para eliminar.");
                DataGridViewRow selectedRow = dataGridViewListadoUsuarios.SelectedRows[0];

                string nEmail = textBoxModificacionEmail.Text;
                string nNumTelefono = textBoxModificacionNumTelefono.Text;

                ValidationResult emailValidation = FormFieldValidationService.ValidateEmail(nEmail);
                ValidationResult phoneValidation = FormFieldValidationService.ValidatePhone(nNumTelefono);

                if (!emailValidation.IsValid)
                {
                    MessageBox.Show($"Error en el email: {emailValidation.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!phoneValidation.IsValid)
                {
                    MessageBox.Show($"Error en el número de teléfono: {phoneValidation.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Usuario selectedUsuario = (Usuario)selectedRow.DataBoundItem;
                gestorUsuarios.ModificarUsuario(selectedUsuario.Username, nEmail, nNumTelefono);
                CargarGridUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gestionUsuariosUIButtonConfirmarEliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewListadoUsuarios.SelectedRows.Count < 1) throw new Exception("No se ha seleccionado ningún usuario para eliminar.");
                DataGridViewRow selectedRow = dataGridViewListadoUsuarios.SelectedRows[0];
                Usuario selectedUsuario = (Usuario)selectedRow.DataBoundItem;
                gestorUsuarios.EliminarUsuario(selectedUsuario.Username);
                CargarGridUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
