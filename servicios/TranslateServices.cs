using BE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace servicios
{
    public static class TranslateServices
    {
        public static void TraducirObjeto(object contenedor, Dictionary<string, string> traducciones)
        {
            if (contenedor == null) return;

            Type tipoContenedor = contenedor.GetType();

            PropertyInfo? propName = tipoContenedor.GetProperty("Name");
            string nombreControl = propName?.GetValue(contenedor)?.ToString() ?? string.Empty;

            if (!string.IsNullOrEmpty(nombreControl) && traducciones.ContainsKey(nombreControl))
            {
                PropertyInfo? propText = tipoContenedor.GetProperty("Text");
                if (propText != null && propText.CanWrite)
                {
                    propText.SetValue(contenedor, traducciones[nombreControl]);
                }
            }

            // Evaluar separadamente Controls, Items y DropDownItems
            string[] nombresColecciones = { "Controls", "Items", "DropDownItems" };

            foreach (string nombreCol in nombresColecciones)
            {
                PropertyInfo? propColeccion = tipoContenedor.GetProperty(nombreCol);
                if (propColeccion != null)
                {
                    var listaItems = propColeccion.GetValue(contenedor) as System.Collections.IEnumerable;
                    if (listaItems != null)
                    {
                        foreach (var subControl in listaItems)
                        {
                            TraducirObjeto(subControl, traducciones);
                        }
                    }
                }
            }
        }
    }
    
}
