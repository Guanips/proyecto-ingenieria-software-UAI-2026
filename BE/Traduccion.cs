using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Traduccion
    {
        public string KeyEtiqueta { get; set; } // Ej: "lbl_Username", "menu_Bitacora"
        public string TextoTraducido { get; set; }

        public Traduccion(string keyEtiqueta, string textoTraducido)
        {
            KeyEtiqueta = keyEtiqueta;
            TextoTraducido = textoTraducido;
        }
    }
}
