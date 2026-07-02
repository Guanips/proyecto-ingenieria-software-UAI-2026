using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DAO
    {
        private static DAO? Instance;
        private static readonly object _lock = new object();

        private DataSet mainDataSet;
        private SqlCommandBuilder cbUsers, cbBitacora;
        private SqlDataAdapter daUsers, daBitacora;
        private DataRelation drUsuarioBitacora;

        private DAO()
        {
            string connectionString = ObtenerStringConexionEnv();

            daUsers = new SqlDataAdapter("Select * From Usuario", connectionString);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", connectionString);

            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);

            mainDataSet = new DataSet("Users");
            DataTable dtUsers = new DataTable("Usuario");
            DataTable dtBitacora = new DataTable("Bitacora");
            mainDataSet.Tables.Add(dtUsers);
            mainDataSet.Tables.Add(dtBitacora);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                daUsers.Fill(dtUsers);
                daBitacora.Fill(dtBitacora);
            }

            DataColumn columnaIdRegistro = dtBitacora.Columns[0];
            dtBitacora.PrimaryKey = new DataColumn[] { columnaIdRegistro };
            int maxId = 0;

            if (dtBitacora.Rows.Count > 0)
            {
                maxId = dtBitacora.AsEnumerable().Max(r => r.Field<int>("ID"));
            }

            columnaIdRegistro.AutoIncrement = true;
            columnaIdRegistro.AutoIncrementSeed = maxId + 1;
            columnaIdRegistro.AutoIncrementStep = 1;

            dtUsers.PrimaryKey = new DataColumn[] { dtUsers.Columns[0] };

            drUsuarioBitacora = new DataRelation("FK_Bitacora_Usuario", dtUsers.Columns[0], dtBitacora.Columns[1]);
            mainDataSet.Relations.Add(drUsuarioBitacora);
        }

        public static DAO GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        if (Instance == null)
                        {
                            Instance = new DAO();
                        }
                    }
                }
                return Instance;
            }
        }

        private string ObtenerStringConexionEnv()
        {
            string? connectionString;
            string directorioActualDAO = AppContext.BaseDirectory;
            DirectoryInfo? directorioRaiz = new DirectoryInfo(directorioActualDAO);

            while (directorioRaiz != null && directorioRaiz.GetFiles("*.sln").Length == 0)
            {
                directorioRaiz = directorioRaiz.Parent;
            }

            if (directorioRaiz == null) throw new Exception("Raiz del proyecto no encontrada para cargar archivo de configuración");
            string rutaArchivoEnv = Path.Combine(directorioRaiz.FullName, ".env");

            DotNetEnv.Env.Load(rutaArchivoEnv);
            connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");
            if (connectionString == null) throw new Exception("Configuración para conexión no encontrada");
            return connectionString;
        }

        public DataSet ObtenerDataSet()
        {
            return mainDataSet;
        }

        public void SubirCambiosBD()
        {
            string connectionString = ObtenerStringConexionEnv();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                daUsers.SelectCommand.Connection = conn;
                daBitacora.SelectCommand.Connection = conn;

                daUsers.InsertCommand = cbUsers.GetInsertCommand();
                daUsers.UpdateCommand = cbUsers.GetUpdateCommand();
                daUsers.DeleteCommand = cbUsers.GetDeleteCommand();
                daUsers.InsertCommand.Connection = conn;
                daUsers.UpdateCommand.Connection = conn;
                daUsers.DeleteCommand.Connection = conn;

                daBitacora.InsertCommand = cbBitacora.GetInsertCommand();
                daBitacora.UpdateCommand = cbBitacora.GetUpdateCommand();
                daBitacora.DeleteCommand = cbBitacora.GetDeleteCommand();
                daBitacora.InsertCommand.Connection = conn;
                daBitacora.UpdateCommand.Connection = conn;
                daBitacora.DeleteCommand.Connection = conn;

                daUsers.Update(mainDataSet, "Usuario");
                daBitacora.Update(mainDataSet, "Bitacora");

                mainDataSet.AcceptChanges();
            }
        }
    }
}