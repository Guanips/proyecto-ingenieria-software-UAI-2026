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
            if (EsModoDiseno()) return;

            // Si llegamos aquí, el programa se está ejecutando de verdad
            GestorIdioma.GetInstance.Attach(this); 
        }

            protected bool EsModoDiseno()
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;

                // Verificación agresiva para el diseñador fuera de proceso de .NET 8
                string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                return processName.Contains("devenv") || processName.Contains("DesignToolsServer");
            }

            //// Validamos si estamos en modo diseño (dentro de Visual Studio)
            //if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            //{
            //    return; // Cortamos la ejecución aquí para que no busque la BD
            //}

            //// Si llegamos aquí, el programa se está ejecutando de verdad
            //GestorIdioma.GetInstance.Attach(this);
        

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

        //protected override void OnFormClosed(FormClosedEventArgs e)
        //{
        //    // Validamos también al cerrar para evitar errores en modo diseño
        //    if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
        //    {
        //        GestorIdioma.GetInstance.Detach(this);
        //    }
        //    base.OnFormClosed(e);
        //}
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