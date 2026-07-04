using BE;
using BLL;
using servicios;

namespace UI.Modules
{
    public partial class GestionPerfilesUI : FormBaseObserver
    {
        private GestorPerfiles gestorPerfiles;

        public GestionPerfilesUI()
        {
            InitializeComponent();
            gestorPerfiles = new GestorPerfiles();
        }

        private void GestionPerfilesUI_Load(object sender, EventArgs e)
        {
            try
            {
                RecargarTodosLosDisplays();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los perfiles y permisos: {ex.Message}",
                                "Error de Carga",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void RecargarTodosLosDisplays()
        {
            List<Permiso> permisosGlobales = gestorPerfiles.ListarPermisos();
            CargarPermisosEnTreeViewComposite(treeViewCompositePermisos, permisosGlobales);
            LlenarListBoxPerfiles(permisosGlobales);
            LlenarListBoxPermisos(permisosGlobales);
            LlenarDataGridViewUsuarios();
        }

        public void CargarPermisosEnTreeViewComposite(TreeView treeView, List<Permiso> permisosRaiz)
        {
            treeView.Nodes.Clear();

            foreach (Permiso permiso in permisosRaiz)
            {
                if (permiso is PermisoSimple) continue;
                TreeNode nodoRaiz = new TreeNode(permiso.Nombre);
                nodoRaiz.Tag = permiso;
                LlenarNodosRecursivo(nodoRaiz, permiso);

                treeView.Nodes.Add(nodoRaiz);
            }
            treeView.ExpandAll();
        }

        private void LlenarNodosRecursivo(TreeNode nodoVisualPadre, Permiso componente)
        {
            if (componente is Perfil perfil)
            {
                foreach (Permiso hijo in perfil.PermisosAsociados)
                {
                    TreeNode nodoVisualHijo = new TreeNode(hijo.Nombre);
                    nodoVisualHijo.Tag = hijo;

                    nodoVisualPadre.Nodes.Add(nodoVisualHijo);
                    LlenarNodosRecursivo(nodoVisualHijo, hijo);
                }
            }
        }

        private void LlenarDataGridViewUsuarios()
        {
            List<Usuario> usuarios = gestorPerfiles.ListarUsuarios();
            Usuario admin = usuarios.FirstOrDefault(u => u.Username == "admin");
            if (admin != null)
            {
                usuarios.Remove(admin);
            }

            dataGridViewUsuarios.DataSource = null;
            dataGridViewUsuarios.DataSource = usuarios;
            OcultarColumnasDataGridView();
        }

        private void OcultarColumnasDataGridView()
        {
            string[] columnasOcultas = { "PasswordHash", "Permisos", "NumTelefono", "Email", "EstaBloqueado", "IntentosFallidos" };
            foreach (string nombreColumna in columnasOcultas)
            {
                if (dataGridViewUsuarios.Columns[nombreColumna] != null)
                {
                    dataGridViewUsuarios.Columns[nombreColumna].Visible = false;
                }
            }
        }

        private void LlenarListBoxPerfiles(List<Permiso> permisos)
        {
            listBoxPerfiles.Items.Clear();
            listBoxPerfiles.DisplayMember = "Nombre";
            listBoxPerfiles.ValueMember = "ID";

            foreach (Permiso permiso in permisos)
            {
                if (permiso is Perfil perfil)
                {
                    listBoxPerfiles.Items.Add(perfil);
                }
            }
        }

        private void LlenarListBoxPermisos(List<Permiso> permisos)
        {
            listBoxPermisos.Items.Clear();
            listBoxPermisos.DisplayMember = "Nombre";
            listBoxPermisos.ValueMember = "ID";

            foreach (Permiso permiso in permisos)
            {
                if (permiso is PermisoSimple permisoSimple)
                {
                    listBoxPermisos.Items.Add(permisoSimple);
                }
            }
        }

        private void perfilesUIButtonCrearPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                string nNombrePerfil = textBoxNombrePerfil.Text;
                ValidationResult nombreValidationResult = FormFieldValidationService.ValidateProfileName(nNombrePerfil);
                if (!nombreValidationResult.IsValid) throw new Exception(nombreValidationResult.ErrorMessage);
                gestorPerfiles.CrearPerfil(nNombrePerfil);

                textBoxNombrePerfil.Clear();
                RecargarTodosLosDisplays();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void perfilUIButtonAsignarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                Perfil? perfilPadre = treeViewCompositePermisos.SelectedNode?.Tag as Perfil;
                Perfil? perfilHijo = listBoxPerfiles.SelectedItem as Perfil;
                if (perfilPadre == null) throw new Exception("Debe seleccionar un perfil padre en el árbol.");
                if (perfilHijo == null) throw new Exception("Debe seleccionar un perfil hijo en la lista.");

                gestorPerfiles.AgregarPerfilAPerfil(perfilPadre.ID, perfilHijo.ID);

                List<Permiso> permisosGlobales = gestorPerfiles.ListarPermisos();
                CargarPermisosEnTreeViewComposite(treeViewCompositePermisos, permisosGlobales);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void perfilUIButtonAsignarPermiso_Click(object sender, EventArgs e)
        {
            try
            {
                PermisoSimple? selectedPermiso = listBoxPermisos.SelectedItem as PermisoSimple;
                Perfil? selectedPerfil = treeViewCompositePermisos.SelectedNode?.Tag as Perfil;
                if (selectedPermiso == null) throw new Exception("Debe seleccionar un permiso para asignar.");
                if (selectedPerfil == null) throw new Exception("Debe seleccionar un perfil para asignar el permiso.");

                gestorPerfiles.AgregarPermisoAPerfil(selectedPerfil.ID, selectedPermiso.ID);

                List<Permiso> permisosGlobales = gestorPerfiles.ListarPermisos();
                CargarPermisosEnTreeViewComposite(treeViewCompositePermisos, permisosGlobales);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void perfilUIButtonAsignarPerfilUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario? selectedUsuario = dataGridViewUsuarios.SelectedRows[0].DataBoundItem as Usuario;
                Perfil? selectedPerfil = listBoxPerfiles.SelectedItem as Perfil;
                if (selectedUsuario == null) throw new Exception("Debe seleccionar un usuario.");
                if (selectedPerfil == null) throw new Exception("Debe seleccionar un perfil.");

                gestorPerfiles.AgregarPerfilAUsuario(selectedUsuario.Username, selectedPerfil.ID);

                RecargarTodosLosDisplays();

                Usuario? updatedUsuario = gestorPerfiles.ListarUsuarios().FirstOrDefault(u => u.Username == selectedUsuario.Username);
                if (updatedUsuario != null)
                {
                    CargarPermisosEnTreeViewComposite(treeViewPermisosUsuario, updatedUsuario.Permisos);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void perfilUIButtonDesasignarPerfilUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Permiso? permisoSeleccionado = treeViewPermisosUsuario.SelectedNode?.Tag as Permiso;
                Usuario? usuarioSeleccionado = dataGridViewUsuarios.SelectedRows[0].DataBoundItem as Usuario;

                if (permisoSeleccionado == null) throw new Exception("Debe seleccionar un permiso o perfil para desasignar.");
                if (permisoSeleccionado is PermisoSimple) throw new Exception("Solo se pueden desasignar perfiles, no permisos simples.");
                if (usuarioSeleccionado == null) throw new Exception("Debe seleccionar un usuario.");
                gestorPerfiles.DesasignarPerfilDeUsuario(usuarioSeleccionado.Username, permisoSeleccionado.ID);
                RecargarTodosLosDisplays();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count < 1) return;

            Usuario? usuarioSeleccionado = dataGridViewUsuarios.SelectedRows[0].DataBoundItem as Usuario;
            if (usuarioSeleccionado != null)
            {
                CargarPermisosEnTreeViewComposite(treeViewPermisosUsuario, usuarioSeleccionado.Permisos);
            }
        }
    }
}
