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

        public void GuardarNuevoIdiomaConTraducciones(Idioma nuevoIdioma, Dictionary<string, string> traducciones)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtIdioma = ds.Tables["Idioma"];
            DataTable? dtTraduccion = ds.Tables["Traduccion"];

            if (dtIdioma == null || dtTraduccion == null)
                throw new Exception("Error: No se encontraron las tablas de Idioma o Traducción en el DataSet.");

            
            DataRow rowIdioma = dtIdioma.NewRow();
            rowIdioma["Codigo"] = nuevoIdioma.Codigo;
            rowIdioma["Nombre"] = nuevoIdioma.Nombre;
            dtIdioma.Rows.Add(rowIdioma);

            
            foreach (KeyValuePair<string, string> kvp in traducciones)
            {
                DataRow rowTraduccion = dtTraduccion.NewRow();
                rowTraduccion["CodigoIdioma"] = nuevoIdioma.Codigo;
                rowTraduccion["KeyEtiqueta"] = kvp.Key;
                rowTraduccion["Texto"] = string.IsNullOrWhiteSpace(kvp.Value) ? kvp.Key : kvp.Value;
                dtTraduccion.Rows.Add(rowTraduccion);
            }

            
            dao.SubirCambiosBD();
        }
    }
}
