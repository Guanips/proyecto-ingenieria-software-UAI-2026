using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using BE;

namespace DAL
{
    public class DAO
    {
        DataSet ds;
        DataTable dtUsers, dtBitacora;
        SqlCommandBuilder cbUsers, cbBitacora;
        SqlDataAdapter daUsers, daBitacora;
        DataRelation drUsuarioBitacora;

        public DAO()
        {
            // obtención de las variables de entorno necesarias para inicializar la conexión a la base de datos
            string connectionString = obtenerStringConexionEnv();

            // conexión a DB
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                daUsers = new SqlDataAdapter("Select * From Usuario", conn);
                daBitacora = new SqlDataAdapter("Select * From Bitacora", conn);
                cbUsers = new SqlCommandBuilder(daUsers);
                cbBitacora = new SqlCommandBuilder(daBitacora);

                ds = new DataSet("Users");
                dtUsers = new DataTable("Usuario");
                dtBitacora = new DataTable("Bitacora");
                ds.Tables.Add(dtUsers);
                ds.Tables.Add(dtBitacora);
                daUsers.Fill(dtUsers);
                daBitacora.Fill(dtBitacora);

                // config de PK
                DataColumn columnaIdRegistro = dtBitacora.Columns[0];
                dtBitacora.PrimaryKey = [columnaIdRegistro];
                columnaIdRegistro.AutoIncrement = true;
                columnaIdRegistro.AutoIncrementSeed = 0;
                columnaIdRegistro.AutoIncrementStep = 1;

                dtUsers.PrimaryKey = [dtUsers.Columns[0]];

                // config de relación usuario -> registro
                drUsuarioBitacora = new DataRelation("FK_Bitacora_Usuario", dtUsers.Columns[0], dtBitacora.Columns[1]);
                ds.Relations.Add(drUsuarioBitacora);
            }
        }

        private string obtenerStringConexionEnv ()
        {
            // primero se encuentra el directorio donde está en archivo .env de forma dinámica
            string? connectionString;
            string directorioActualDAO = AppContext.BaseDirectory;
            DirectoryInfo? directorioRaiz = new DirectoryInfo(directorioActualDAO);

            while (directorioRaiz != null && directorioRaiz.GetFiles("*.sln").Length == 0)
            {
                directorioRaiz = directorioRaiz.Parent;
            }

            if (directorioRaiz == null) throw new Exception("Raiz del proyecto no encontrada para cargar archivo de configuración");
            string rutaArchivoEnv = Path.Combine(directorioRaiz.FullName, ".env");

            // se carga el archivo .env y se obtiene la variable correspondiente
            DotNetEnv.Env.Load(rutaArchivoEnv);
            connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");
            if (connectionString == null) throw new Exception("Configuración para conexión no encontrada");
            return connectionString;
        }

        public DataTable RetornaDataTableUsuarios()
        {
            return dtUsers;
        }

        public DataTable RetornaDataTableBitacora() 
        {
            return dtBitacora;
        }

        public void ActualizarBitacora (DataTable dtBitacora) 
        {
            string connectionString = obtenerStringConexionEnv();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                daBitacora = new SqlDataAdapter("Select * From Bitacora", conn);
                cbBitacora = new SqlCommandBuilder(daBitacora);
                daBitacora.Update(dtBitacora);       
            }
        }
        
        //public void ActualizarUsuarios (DataTable dtUsuarios) => daUsers.Update(dtUsuarios);
    }
}