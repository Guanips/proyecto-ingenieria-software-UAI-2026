using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositorioIdioma
    {
        public RepositorioIdioma() { }

        /// <summary>
        /// Busca en el DataSet en memoria las traducciones correspondientes a un idioma
        /// y las devuelve empaquetadas en un diccionario asociativo.
        /// </summary>
        public Dictionary<string, string> ObtenerTraducciones(string codigoIdioma)
        {
            var diccionarioResult = new Dictionary<string, string>();

            try
            {
                DataSet ds = DAO.GetInstance.ObtenerDataSet();

                DataTable dtTraducciones = ds.Tables["Traduccion"];

                if (dtTraducciones != null)
                {
                    DataRow[] filasFiltradas = dtTraducciones.Select($"CodigoIdioma = '{codigoIdioma}'");

                    foreach (DataRow fila in filasFiltradas)
                    {
                        string keyEtiqueta = fila["KeyEtiqueta"].ToString() ?? string.Empty;
                        string textoTraducido = fila["Texto"].ToString() ?? string.Empty;

                        if (!string.IsNullOrEmpty(keyEtiqueta) && !diccionarioResult.ContainsKey(keyEtiqueta))
                        {
                            diccionarioResult.Add(keyEtiqueta, textoTraducido);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al recuperar las traducciones para el idioma '{codigoIdioma}': " + ex.Message);
            }

            return diccionarioResult;
        }

        public List<Idioma> ObtenerTodosLosIdiomas()
        {
            List<Idioma> lista = new List<Idioma>();

            // Accedemos al DataSet centralizado a través de tu DAO
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtIdiomas = ds.Tables["Idioma"];

            if (dtIdiomas != null)
            {
                foreach (DataRow row in dtIdiomas.Rows)
                {
                    string codigo = row["Codigo"].ToString() ?? string.Empty;
                    string nombre = row["Nombre"].ToString() ?? string.Empty;

                    lista.Add(new Idioma(codigo, nombre));
                }
            }

            return lista;
        }

    }
}
