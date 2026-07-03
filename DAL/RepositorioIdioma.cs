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

        /*
            -- 1. Tabla de Idiomas
            CREATE TABLE Idioma (
                Codigo VARCHAR(5) NOT NULL,   -- Ej: 'ES', 'EN'
                Nombre VARCHAR(50) NOT NULL,  -- Ej: 'Español', 'English'
                CONSTRAINT PK_Idioma PRIMARY KEY (Codigo)
            );

            -- 2. Tabla de Traducciones
            CREATE TABLE Traduccion (
                IdTraduccion INT IDENTITY(1,1) NOT NULL,
                CodigoIdioma VARCHAR(5) NOT NULL,
                KeyEtiqueta VARCHAR(100) NOT NULL, -- El nombre del control (Ej: loginUILabelUsername)
                Texto NVARCHAR(MAX) NOT NULL,      -- El texto a mostrar (Ej: 'Nombre de usuario')
                CONSTRAINT PK_Traduccion PRIMARY KEY (IdTraduccion),
                CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (CodigoIdioma) REFERENCES Idioma(Codigo)
            );

            -- 3. Índice Único: Asegura que no puedas tener dos "loginUILabelUsername" para el idioma "ES"
            CREATE UNIQUE INDEX UIX_Idioma_Etiqueta ON Traduccion(CodigoIdioma, KeyEtiqueta);

            ALTER TABLE Usuario ADD IntentosFallidos INT NOT NULL DEFAULT 0;
         
         */
    }
}
