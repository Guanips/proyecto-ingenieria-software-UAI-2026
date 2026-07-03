namespace UI
{
    partial class MainUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            mainUIStripMenuItemInicio = new ToolStripMenuItem();
            mainUIStripMenuItemIniciarSesion = new ToolStripMenuItem();
            mainUIStripMenuItemCerrarSesion = new ToolStripMenuItem();
            mainUIStripMenuItemGestionDeUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemABMUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemDesbloqueoUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemGestionDePerfiles = new ToolStripMenuItem();
            mainUIStripMenuItemABMPerfiles = new ToolStripMenuItem();
            mainUIStripMenuItemBitacora = new ToolStripMenuItem();
            mainUIStripMenuItemConsultarBitacora = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { mainUIStripMenuItemInicio, mainUIStripMenuItemGestionDeUsuarios, mainUIStripMenuItemGestionDePerfiles, mainUIStripMenuItemBitacora });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1108, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // mainUIStripMenuItemInicio
            // 
            mainUIStripMenuItemInicio.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemIniciarSesion, mainUIStripMenuItemCerrarSesion });
            mainUIStripMenuItemInicio.Name = "mainUIStripMenuItemInicio";
            mainUIStripMenuItemInicio.Size = new Size(48, 20);
            mainUIStripMenuItemInicio.Text = "Inicio";
            // 
            // mainUIStripMenuItemIniciarSesion
            // 
            mainUIStripMenuItemIniciarSesion.Name = "mainUIStripMenuItemIniciarSesion";
            mainUIStripMenuItemIniciarSesion.Size = new Size(180, 22);
            mainUIStripMenuItemIniciarSesion.Text = "Iniciar sesion";
            mainUIStripMenuItemIniciarSesion.Click += mainUIStripMenuItemIniciarSesion_Click;
            // 
            // mainUIStripMenuItemCerrarSesion
            // 
            mainUIStripMenuItemCerrarSesion.Name = "mainUIStripMenuItemCerrarSesion";
            mainUIStripMenuItemCerrarSesion.Size = new Size(180, 22);
            mainUIStripMenuItemCerrarSesion.Text = "Cerrar sesion";
            mainUIStripMenuItemCerrarSesion.Click += mainUIStripMenuItemCerrarSesion_Click;
            // 
            // mainUIStripMenuItemGestionDeUsuarios
            // 
            mainUIStripMenuItemGestionDeUsuarios.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMUsuarios, mainUIStripMenuItemDesbloqueoUsuarios });
            mainUIStripMenuItemGestionDeUsuarios.Name = "mainUIStripMenuItemGestionDeUsuarios";
            mainUIStripMenuItemGestionDeUsuarios.Size = new Size(122, 20);
            mainUIStripMenuItemGestionDeUsuarios.Text = "Gestión de usuarios";
            // 
            // mainUIStripMenuItemABMUsuarios
            // 
            mainUIStripMenuItemABMUsuarios.Name = "mainUIStripMenuItemABMUsuarios";
            mainUIStripMenuItemABMUsuarios.Size = new Size(200, 22);
            mainUIStripMenuItemABMUsuarios.Text = "ABM Usuarios";
            mainUIStripMenuItemABMUsuarios.Click += mainUIStripMenuItemABMUsuarios_Click;
            // 
            // mainUIStripMenuItemDesbloqueoUsuarios
            // 
            mainUIStripMenuItemDesbloqueoUsuarios.Name = "mainUIStripMenuItemDesbloqueoUsuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Size = new Size(200, 22);
            mainUIStripMenuItemDesbloqueoUsuarios.Text = "Desbloqueo de usuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Click += mainUIStripMenuItemDesbloqueoUsuarios_Click;
            // 
            // mainUIStripMenuItemGestionDePerfiles
            // 
            mainUIStripMenuItemGestionDePerfiles.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMPerfiles });
            mainUIStripMenuItemGestionDePerfiles.Name = "mainUIStripMenuItemGestionDePerfiles";
            mainUIStripMenuItemGestionDePerfiles.Size = new Size(116, 20);
            mainUIStripMenuItemGestionDePerfiles.Text = "Gestión de perfiles";
            // 
            // mainUIStripMenuItemABMPerfiles
            // 
            mainUIStripMenuItemABMPerfiles.Name = "mainUIStripMenuItemABMPerfiles";
            mainUIStripMenuItemABMPerfiles.Size = new Size(221, 22);
            mainUIStripMenuItemABMPerfiles.Text = "Alta y asignación de perfiles";
            mainUIStripMenuItemABMPerfiles.Click += mainUIStripMenuItemABMPerfiles_Click;
            // 
            // mainUIStripMenuItemBitacora
            // 
            mainUIStripMenuItemBitacora.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemConsultarBitacora });
            mainUIStripMenuItemBitacora.Name = "mainUIStripMenuItemBitacora";
            mainUIStripMenuItemBitacora.Size = new Size(62, 20);
            mainUIStripMenuItemBitacora.Text = "Bitacora";
            // 
            // mainUIStripMenuItemConsultarBitacora
            // 
            mainUIStripMenuItemConsultarBitacora.Name = "mainUIStripMenuItemConsultarBitacora";
            mainUIStripMenuItemConsultarBitacora.Size = new Size(171, 22);
            mainUIStripMenuItemConsultarBitacora.Text = "Consultar bitacora";
            mainUIStripMenuItemConsultarBitacora.Click += mainUIStripMenuItemConsultarBitacora_Click;
            // 
            // MainUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 564);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainUI";
            Text = "Sistema de gestion";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem mainUIStripMenuItemInicio;
        private ToolStripMenuItem mainUIStripMenuItemIniciarSesion;
        private ToolStripMenuItem mainUIStripMenuItemCerrarSesion;
        private ToolStripMenuItem mainUIStripMenuItemGestionDeUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemABMUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemDesbloqueoUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemGestionDePerfiles;
        private ToolStripMenuItem mainUIStripMenuItemABMPerfiles;
        private ToolStripMenuItem mainUIStripMenuItemBitacora;
        private ToolStripMenuItem mainUIStripMenuItemConsultarBitacora;
    }
}
