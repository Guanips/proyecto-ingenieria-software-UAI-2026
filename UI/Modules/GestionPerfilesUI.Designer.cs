namespace UI.Modules
{
    partial class GestionPerfilesUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            perfilesUIGroupBoxTreeView = new GroupBox();
            perfilesUIButtonCrearPerfil = new Button();
            textBoxNombrePerfil = new TextBox();
            perfilesUILabelNombrePerfil = new Label();
            treeViewCompositePermisos = new TreeView();
            perfilesUIGroupBoxListBoxPerfiles = new GroupBox();
            perfilUIButtonAsignarPerfil = new Button();
            listBoxPerfiles = new ListBox();
            perfilesUIGroupBoxUsuarios = new GroupBox();
            perfilUIButtonDesasignarPerfilUsuario = new Button();
            perfilUIButtonAsignarPerfilUsuario = new Button();
            treeViewPermisosUsuario = new TreeView();
            dataGridViewUsuarios = new DataGridView();
            perfilesUIGroupBoxListBoxPermisos = new GroupBox();
            perfilUIButtonAsignarPermiso = new Button();
            listBoxPermisos = new ListBox();
            perfilesUIGroupBoxTreeView.SuspendLayout();
            perfilesUIGroupBoxListBoxPerfiles.SuspendLayout();
            perfilesUIGroupBoxUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            perfilesUIGroupBoxListBoxPermisos.SuspendLayout();
            SuspendLayout();
            // 
            // perfilesUIGroupBoxTreeView
            // 
            perfilesUIGroupBoxTreeView.Controls.Add(perfilesUIButtonCrearPerfil);
            perfilesUIGroupBoxTreeView.Controls.Add(textBoxNombrePerfil);
            perfilesUIGroupBoxTreeView.Controls.Add(perfilesUILabelNombrePerfil);
            perfilesUIGroupBoxTreeView.Controls.Add(treeViewCompositePermisos);
            perfilesUIGroupBoxTreeView.Location = new Point(13, 29);
            perfilesUIGroupBoxTreeView.Name = "perfilesUIGroupBoxTreeView";
            perfilesUIGroupBoxTreeView.Size = new Size(297, 436);
            perfilesUIGroupBoxTreeView.TabIndex = 0;
            perfilesUIGroupBoxTreeView.TabStop = false;
            perfilesUIGroupBoxTreeView.Text = "Arbol de perfiles y permisos";
            // 
            // perfilesUIButtonCrearPerfil
            // 
            perfilesUIButtonCrearPerfil.Location = new Point(6, 403);
            perfilesUIButtonCrearPerfil.Name = "perfilesUIButtonCrearPerfil";
            perfilesUIButtonCrearPerfil.Size = new Size(277, 23);
            perfilesUIButtonCrearPerfil.TabIndex = 3;
            perfilesUIButtonCrearPerfil.Text = "Crear perfil";
            perfilesUIButtonCrearPerfil.UseVisualStyleBackColor = true;
            perfilesUIButtonCrearPerfil.Click += perfilesUIButtonCrearPerfil_Click;
            // 
            // textBoxNombrePerfil
            // 
            textBoxNombrePerfil.Location = new Point(6, 374);
            textBoxNombrePerfil.Name = "textBoxNombrePerfil";
            textBoxNombrePerfil.Size = new Size(277, 23);
            textBoxNombrePerfil.TabIndex = 2;
            // 
            // perfilesUILabelNombrePerfil
            // 
            perfilesUILabelNombrePerfil.AutoSize = true;
            perfilesUILabelNombrePerfil.Location = new Point(6, 356);
            perfilesUILabelNombrePerfil.Name = "perfilesUILabelNombrePerfil";
            perfilesUILabelNombrePerfil.Size = new Size(136, 15);
            perfilesUILabelNombrePerfil.TabIndex = 1;
            perfilesUILabelNombrePerfil.Text = "Nombre del nuevo perfil";
            // 
            // treeViewCompositePermisos
            // 
            treeViewCompositePermisos.Location = new Point(6, 22);
            treeViewCompositePermisos.Name = "treeViewCompositePermisos";
            treeViewCompositePermisos.Size = new Size(277, 322);
            treeViewCompositePermisos.TabIndex = 0;
            // 
            // perfilesUIGroupBoxListBoxPerfiles
            // 
            perfilesUIGroupBoxListBoxPerfiles.Controls.Add(perfilUIButtonAsignarPerfil);
            perfilesUIGroupBoxListBoxPerfiles.Controls.Add(listBoxPerfiles);
            perfilesUIGroupBoxListBoxPerfiles.Location = new Point(316, 29);
            perfilesUIGroupBoxListBoxPerfiles.Name = "perfilesUIGroupBoxListBoxPerfiles";
            perfilesUIGroupBoxListBoxPerfiles.Size = new Size(209, 228);
            perfilesUIGroupBoxListBoxPerfiles.TabIndex = 1;
            perfilesUIGroupBoxListBoxPerfiles.TabStop = false;
            perfilesUIGroupBoxListBoxPerfiles.Text = "Perfiles disponibles";
            // 
            // perfilUIButtonAsignarPerfil
            // 
            perfilUIButtonAsignarPerfil.Location = new Point(6, 182);
            perfilUIButtonAsignarPerfil.Name = "perfilUIButtonAsignarPerfil";
            perfilUIButtonAsignarPerfil.Size = new Size(194, 38);
            perfilUIButtonAsignarPerfil.TabIndex = 1;
            perfilUIButtonAsignarPerfil.Text = "Asignar perfil a perfil";
            perfilUIButtonAsignarPerfil.UseVisualStyleBackColor = true;
            perfilUIButtonAsignarPerfil.Click += perfilUIButtonAsignarPerfil_Click;
            // 
            // listBoxPerfiles
            // 
            listBoxPerfiles.FormattingEnabled = true;
            listBoxPerfiles.ItemHeight = 15;
            listBoxPerfiles.Location = new Point(6, 22);
            listBoxPerfiles.Name = "listBoxPerfiles";
            listBoxPerfiles.Size = new Size(194, 154);
            listBoxPerfiles.TabIndex = 0;
            // 
            // perfilesUIGroupBoxUsuarios
            // 
            perfilesUIGroupBoxUsuarios.Controls.Add(perfilUIButtonDesasignarPerfilUsuario);
            perfilesUIGroupBoxUsuarios.Controls.Add(perfilUIButtonAsignarPerfilUsuario);
            perfilesUIGroupBoxUsuarios.Controls.Add(treeViewPermisosUsuario);
            perfilesUIGroupBoxUsuarios.Controls.Add(dataGridViewUsuarios);
            perfilesUIGroupBoxUsuarios.Location = new Point(531, 29);
            perfilesUIGroupBoxUsuarios.Name = "perfilesUIGroupBoxUsuarios";
            perfilesUIGroupBoxUsuarios.Size = new Size(535, 436);
            perfilesUIGroupBoxUsuarios.TabIndex = 2;
            perfilesUIGroupBoxUsuarios.TabStop = false;
            perfilesUIGroupBoxUsuarios.Text = "Usuarios disponibles";
            // 
            // perfilUIButtonDesasignarPerfilUsuario
            // 
            perfilUIButtonDesasignarPerfilUsuario.Location = new Point(287, 386);
            perfilUIButtonDesasignarPerfilUsuario.Name = "perfilUIButtonDesasignarPerfilUsuario";
            perfilUIButtonDesasignarPerfilUsuario.Size = new Size(242, 44);
            perfilUIButtonDesasignarPerfilUsuario.TabIndex = 3;
            perfilUIButtonDesasignarPerfilUsuario.Text = "Desasignar perfil a usuario";
            perfilUIButtonDesasignarPerfilUsuario.UseVisualStyleBackColor = true;
            perfilUIButtonDesasignarPerfilUsuario.Click += perfilUIButtonDesasignarPerfilUsuario_Click;
            // 
            // perfilUIButtonAsignarPerfilUsuario
            // 
            perfilUIButtonAsignarPerfilUsuario.Location = new Point(13, 386);
            perfilUIButtonAsignarPerfilUsuario.Name = "perfilUIButtonAsignarPerfilUsuario";
            perfilUIButtonAsignarPerfilUsuario.Size = new Size(268, 44);
            perfilUIButtonAsignarPerfilUsuario.TabIndex = 2;
            perfilUIButtonAsignarPerfilUsuario.Text = "Asignar perfil a usuario";
            perfilUIButtonAsignarPerfilUsuario.UseVisualStyleBackColor = true;
            perfilUIButtonAsignarPerfilUsuario.Click += perfilUIButtonAsignarPerfilUsuario_Click;
            // 
            // treeViewPermisosUsuario
            // 
            treeViewPermisosUsuario.Location = new Point(287, 24);
            treeViewPermisosUsuario.Name = "treeViewPermisosUsuario";
            treeViewPermisosUsuario.Size = new Size(242, 347);
            treeViewPermisosUsuario.TabIndex = 1;
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Location = new Point(13, 24);
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(268, 347);
            dataGridViewUsuarios.TabIndex = 0;
            // 
            // perfilesUIGroupBoxListBoxPermisos
            // 
            perfilesUIGroupBoxListBoxPermisos.Controls.Add(perfilUIButtonAsignarPermiso);
            perfilesUIGroupBoxListBoxPermisos.Controls.Add(listBoxPermisos);
            perfilesUIGroupBoxListBoxPermisos.Location = new Point(316, 263);
            perfilesUIGroupBoxListBoxPermisos.Name = "perfilesUIGroupBoxListBoxPermisos";
            perfilesUIGroupBoxListBoxPermisos.Size = new Size(209, 202);
            perfilesUIGroupBoxListBoxPermisos.TabIndex = 2;
            perfilesUIGroupBoxListBoxPermisos.TabStop = false;
            perfilesUIGroupBoxListBoxPermisos.Text = "Permisos disponibles";
            // 
            // perfilUIButtonAsignarPermiso
            // 
            perfilUIButtonAsignarPermiso.Location = new Point(6, 155);
            perfilUIButtonAsignarPermiso.Name = "perfilUIButtonAsignarPermiso";
            perfilUIButtonAsignarPermiso.Size = new Size(194, 38);
            perfilUIButtonAsignarPermiso.TabIndex = 2;
            perfilUIButtonAsignarPermiso.Text = "Asignar permiso a perfil";
            perfilUIButtonAsignarPermiso.UseVisualStyleBackColor = true;
            perfilUIButtonAsignarPermiso.Click += perfilUIButtonAsignarPermiso_Click;
            // 
            // listBoxPermisos
            // 
            listBoxPermisos.FormattingEnabled = true;
            listBoxPermisos.ItemHeight = 15;
            listBoxPermisos.Location = new Point(6, 22);
            listBoxPermisos.Name = "listBoxPermisos";
            listBoxPermisos.Size = new Size(194, 124);
            listBoxPermisos.TabIndex = 1;
            // 
            // GestionPerfilesUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1079, 494);
            ControlBox = false;
            Controls.Add(perfilesUIGroupBoxListBoxPermisos);
            Controls.Add(perfilesUIGroupBoxUsuarios);
            Controls.Add(perfilesUIGroupBoxListBoxPerfiles);
            Controls.Add(perfilesUIGroupBoxTreeView);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GestionPerfilesUI";
            Text = "GestionPerfilesUI";
            WindowState = FormWindowState.Maximized;
            perfilesUIGroupBoxTreeView.ResumeLayout(false);
            perfilesUIGroupBoxTreeView.PerformLayout();
            perfilesUIGroupBoxListBoxPerfiles.ResumeLayout(false);
            perfilesUIGroupBoxUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            perfilesUIGroupBoxListBoxPermisos.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox perfilesUIGroupBoxTreeView;
        private TreeView treeViewCompositePermisos;
        private GroupBox perfilesUIGroupBoxListBoxPerfiles;
        private GroupBox perfilesUIGroupBoxUsuarios;
        private GroupBox perfilesUIGroupBoxListBoxPermisos;
        private Label perfilesUILabelNombrePerfil;
        private Button perfilesUIButtonCrearPerfil;
        private TextBox textBoxNombrePerfil;
        private Button perfilUIButtonAsignarPerfil;
        private ListBox listBoxPerfiles;
        private Button perfilUIButtonDesasignarPerfilUsuario;
        private Button perfilUIButtonAsignarPerfilUsuario;
        private TreeView treeViewPermisosUsuario;
        private DataGridView dataGridViewUsuarios;
        private Button perfilUIButtonAsignarPermiso;
        private ListBox listBoxPermisos;
    }
}