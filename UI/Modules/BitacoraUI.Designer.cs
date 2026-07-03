namespace UI.Modules
{
    partial class BitacoraUI
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
            this.btnLimpiarFiltros = new Button();
            this.comboFiltroAccion = new ComboBox();
            this.dataGridViewBitacora = new DataGridView();
            textBoxFiltroUsuario = new TextBox();
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewBitacora).BeginInit();
            SuspendLayout();
            // 
            // btnLimpiarFiltros
            // 
            this.btnLimpiarFiltros.Location = new Point(552, 135);
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Size = new Size(87, 28);
            this.btnLimpiarFiltros.TabIndex = 0;
            this.btnLimpiarFiltros.Text = "Limpiar";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            // 
            // comboFiltroAccion
            // 
            this.comboFiltroAccion.FormattingEnabled = true;
            this.comboFiltroAccion.Location = new Point(552, 26);
            this.comboFiltroAccion.Name = "comboFiltroAccion";
            this.comboFiltroAccion.Size = new Size(97, 23);
            this.comboFiltroAccion.TabIndex = 1;
            // 
            // dataGridViewBitacora
            // 
            this.dataGridViewBitacora.AllowUserToAddRows = false;
            this.dataGridViewBitacora.AllowUserToDeleteRows = false;
            this.dataGridViewBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBitacora.Location = new Point(24, 26);
            this.dataGridViewBitacora.Name = "dataGridViewBitacora";
            this.dataGridViewBitacora.ReadOnly = true;
            this.dataGridViewBitacora.Size = new Size(493, 333);
            this.dataGridViewBitacora.TabIndex = 2;
            // 
            // textBoxFiltroUsuario
            // 
            textBoxFiltroUsuario.Location = new Point(552, 77);
            textBoxFiltroUsuario.Name = "textBoxFiltroUsuario";
            textBoxFiltroUsuario.Size = new Size(97, 23);
            textBoxFiltroUsuario.TabIndex = 3;
            // 
            // BitacoraUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxFiltroUsuario);
            Controls.Add(this.dataGridViewBitacora);
            Controls.Add(this.comboFiltroAccion);
            Controls.Add(this.btnLimpiarFiltros);
            Name = "BitacoraUI";
            Text = "BitacoraUI";
            Load += BitacoraUI_Load;
            ((System.ComponentModel.ISupportInitialize)this.dataGridViewBitacora).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnLimpiarFiltros;
        private System.Windows.Forms.ComboBox comboFiltroAccion;
        private System.Windows.Forms.DataGridView dataGridViewBitacora;
        private System.Windows.Forms.TextBox textBoxFiltroUsuario;
    }
}