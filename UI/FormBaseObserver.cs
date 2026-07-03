using System;
using System.Collections.Generic;
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
            GestorIdioma.GetInstance.Attach(this);
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
            // Al cerrarse la pantalla, se desvincula para evitar fugas de memoria
            GestorIdioma.GetInstance.Detach(this);
            base.OnFormClosed(e);
        }
    }
}
