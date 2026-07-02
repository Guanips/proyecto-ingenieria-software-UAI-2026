using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DAO
    {
        private static DAO? Instance;
        private static readonly object _lock = new object();

        private DataSet mainDataSet;
        private SqlCommandBuilder cbUsers, cbBitacora, cbPermiso, cbPermisoRelacion;
        private SqlDataAdapter daUsers, daBitacora, daPermiso, daPermisoRelacion;

        private DAO()
        {
            string connectionString = ObtenerStringConexionEnv();

            daUsers = new SqlDataAdapter("Select * From Usuario", connectionString);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", connectionString);
            daPermiso = new SqlDataAdapter("Select * From Permiso", connectionString);
            daPermisoRelacion = new SqlDataAdapter("Select * From PermisoRelacion", connectionString);

            daUsers.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daBitacora.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermiso.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermisoRelacion.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);

            mainDataSet = new DataSet("Users");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                CargarTablaConEsquema(daUsers, "Usuario", conn);
                CargarTablaConEsquema(daBitacora, "Bitacora", conn);
                CargarTablaConEsquema(daPermiso, "Permiso", conn);
                CargarTablaConEsquema(daPermisoRelacion, "PermisoRelacion", conn);
            }

            ConfigurarAutoincrementoGeneral();
            ArmarRelaciones();
        }

        private void CargarTablaConEsquema(SqlDataAdapter adapter, string tableName, SqlConnection connection)
        {
            if (adapter.SelectCommand == null)
            {
                adapter.SelectCommand = new SqlCommand();
            }

            adapter.SelectCommand.CommandText = $"Select * From {tableName}";
            adapter.SelectCommand.Connection = connection;

            adapter.FillSchema(mainDataSet, SchemaType.Source, tableName);
            adapter.Fill(mainDataSet, tableName);
        }

        private void ArmarRelaciones()
        {
            DataTable? dtPermiso = mainDataSet.Tables["Permiso"];
            DataTable? dtPermisoRelacion = mainDataSet.Tables["PermisoRelacion"];

            if (dtPermiso == null || dtPermisoRelacion == null) throw new Exception("Error en el armado de relaciones DAO");

            DataRelation drPermisoPadre = new DataRelation(
                "FK_PermisoRelacion_Padre",
                dtPermiso.Columns["ID"]!,
                dtPermisoRelacion.Columns["ID_Padre"]!
            );

            DataRelation drPermisoHijo = new DataRelation(
                "FK_PermisoRelacion_Hijo",
                dtPermiso.Columns["ID"]!,
                dtPermisoRelacion.Columns["ID_Hijo"]!
            );

            mainDataSet.Relations.Add(drPermisoPadre);
            mainDataSet.Relations.Add(drPermisoHijo);
        }

        private void ConfigurarAutoincrementoGeneral()
        {
            if (!mainDataSet.Tables.Contains("Bitacora") || !mainDataSet.Tables.Contains("Permiso")) return;

            DataTable dtBitacora = mainDataSet.Tables["Bitacora"]!;
            DataTable dtPermiso = mainDataSet.Tables["Permiso"]!;

            if (!dtBitacora.Columns.Contains("ID") || !dtPermiso.Columns.Contains("ID")) return;

            DataColumn columnaIdRegistroBitacora = dtBitacora.Columns["ID"]!;
            int maxId = 0;

            if (dtBitacora.Rows.Count > 0)
            {
                maxId = dtBitacora.AsEnumerable()
                    .Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"]));
            }

            columnaIdRegistroBitacora.AutoIncrement = true;
            columnaIdRegistroBitacora.AutoIncrementSeed = maxId + 1;
            columnaIdRegistroBitacora.AutoIncrementStep = 1;

            DataColumn columnaIdRegistroPermiso = dtPermiso.Columns["ID"]!;
            int maxIdPermiso = 0;

            if (dtPermiso.Rows.Count > 0)
            {
                maxIdPermiso = dtPermiso.AsEnumerable()
                    .Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"]));
            }

            columnaIdRegistroPermiso.AutoIncrement = true;
            columnaIdRegistroPermiso.AutoIncrementSeed = maxIdPermiso + 1;
            columnaIdRegistroPermiso.AutoIncrementStep = 1;
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
                daPermiso.SelectCommand.Connection = conn;
                daPermisoRelacion.SelectCommand.Connection = conn;

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

                daPermiso.InsertCommand = cbPermiso.GetInsertCommand();
                daPermiso.UpdateCommand = cbPermiso.GetUpdateCommand();
                daPermiso.DeleteCommand = cbPermiso.GetDeleteCommand();
                daPermiso.InsertCommand.Connection = conn;
                daPermiso.UpdateCommand.Connection = conn;
                daPermiso.DeleteCommand.Connection = conn;

                daPermisoRelacion.InsertCommand = cbPermisoRelacion.GetInsertCommand();
                daPermisoRelacion.UpdateCommand = cbPermisoRelacion.GetUpdateCommand();
                daPermisoRelacion.DeleteCommand = cbPermisoRelacion.GetDeleteCommand();
                daPermisoRelacion.InsertCommand.Connection = conn;
                daPermisoRelacion.UpdateCommand.Connection = conn;
                daPermisoRelacion.DeleteCommand.Connection = conn;

                daUsers.Update(mainDataSet, "Usuario");
                daBitacora.Update(mainDataSet, "Bitacora");
                daPermiso.Update(mainDataSet, "Permiso");
                daPermisoRelacion.Update(mainDataSet, "PermisoRelacion");

                mainDataSet.AcceptChanges();
            }
        }
    }
}