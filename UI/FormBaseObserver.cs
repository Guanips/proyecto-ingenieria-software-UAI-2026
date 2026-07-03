using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BE;
using BLL;
using servicios;

namespace UI
{
    public partial class FormBaseObserver : Form, IObserver
    {
        public FormBaseObserver()
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!EsModoDiseno())
            {
                GestorIdioma.GetInstance.Attach(this);
            }
        }

        protected bool EsModoDiseno()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;

            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return processName.Contains("devenv") || processName.Contains("DesignToolsServer");
        }

        public virtual void Update(Usuario usuarioInvolucrado, string action)
        {
            if (action.StartsWith("Idioma:"))
            {
                string codigoIdioma = action.Split(':')[1];

                Dictionary<string, string> traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

                TranslateServices.TraducirObjeto(this, traducciones);

                TraducirElementosParticulares(codigoIdioma);
            }
        }

        protected virtual void TraducirElementosParticulares(string codigoIdioma)
        {
            
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (!EsModoDiseno())
            {
                GestorIdioma.GetInstance.Detach(this);
            }
            base.OnFormClosed(e);
        }
    }
}