using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DAO
    {
        private static DAO? Instance;
        private static readonly object _lock = new object();

        private DataSet mainDataSet;
        private SqlDataAdapter daUsers, daBitacora, daPermiso, daPermisoRelacion, daIdioma, daTraduccion, daPerfilUsuario;
        private SqlCommandBuilder cbUsers, cbBitacora, cbPermiso, cbPermisoRelacion, cbIdioma, cbTraduccion, cbPerfilUsuario;
        private DAO()
        {
            string connectionString = ObtenerStringConexionEnv();

            // 1. Instanciar los DataAdapters con sus consultas base
            daUsers = new SqlDataAdapter("Select * From Usuario", connectionString);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", connectionString);
            daPermiso = new SqlDataAdapter("Select * From Permiso", connectionString);
            daPermisoRelacion = new SqlDataAdapter("Select * From PermisoRelacion", connectionString);
            daIdioma = new SqlDataAdapter("Select * From Idioma", connectionString);
            daTraduccion = new SqlDataAdapter("Select * From Traduccion", connectionString);
            daPerfilUsuario = new SqlDataAdapter("Select * From PerfilUsuario", connectionString);

            daUsers.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daBitacora.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermiso.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermisoRelacion.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daIdioma.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daTraduccion.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPerfilUsuario.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);
            cbIdioma = new SqlCommandBuilder(daIdioma);
            cbTraduccion = new SqlCommandBuilder(daTraduccion);
            cbPerfilUsuario = new SqlCommandBuilder(daPerfilUsuario);

            mainDataSet = new DataSet("Users");

            // 2. Abrir la conexión y CARGAR los datos PRIMERO
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                CargarTablaConEsquema(daUsers, "Usuario", conn);
                CargarTablaConEsquema(daBitacora, "Bitacora", conn);
                CargarTablaConEsquema(daPermiso, "Permiso", conn);
                CargarTablaConEsquema(daPermisoRelacion, "PermisoRelacion", conn);
                CargarTablaConEsquema(daIdioma, "Idioma", conn);
                CargarTablaConEsquema(daTraduccion, "Traduccion", conn);
                CargarTablaConEsquema(daPerfilUsuario, "PerfilUsuario", conn);
            }

            // 3. INSTANCIAR los CommandBuilders DESPUÉS de que las tablas tengan su esquema y PK cargados
            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);
            cbPerfilUsuario = new SqlCommandBuilder(daPerfilUsuario);

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

            // Llenamos el esquema y los datos
            adapter.FillSchema(mainDataSet, SchemaType.Source, tableName);
            adapter.Fill(mainDataSet, tableName);

            // DOBLE CHECK: Si por alguna razón el proveedor de SQL Server no mapeó la PK, la forzamos basándonos en la BD
            if (mainDataSet.Tables[tableName]!.PrimaryKey.Length == 0)
            {
                // Esto le avisa al CommandBuilder de forma explícita cuál es la columna clave
                // En tus tablas es 'ID', salvo en las compuestas como PermisoRelacion que usa ambas
                if (tableName == "PermisoRelacion")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] {
                mainDataSet.Tables[tableName]!.Columns["ID_Padre"]!,
                mainDataSet.Tables[tableName]!.Columns["ID_Hijo"]!
            };
                }
                else if (tableName == "PerfilUsuario")
                {
                    // PerfilUsuario no tiene PK explícita en tu script (solo FKs), podrías asignarle una compuesta si la requiere:
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] {
                mainDataSet.Tables[tableName]!.Columns["ID_Usuario"]!,
                mainDataSet.Tables[tableName]!.Columns["ID_Perfil"]!
            };
                }
                else
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["ID"]! };
                }
            }
        }

        private void ArmarRelaciones()
        {
            DataTable? dtPermiso = mainDataSet.Tables["Permiso"];
            DataTable? dtPermisoRelacion = mainDataSet.Tables["PermisoRelacion"];
            DataTable? dtPerfilUsuario = mainDataSet.Tables["PerfilUsuario"];
            DataTable? dtUsuario = mainDataSet.Tables["Usuario"];

            if (dtPermiso == null || dtPermisoRelacion == null || dtPerfilUsuario == null || dtUsuario == null) throw new Exception("Error en el armado de relaciones DAO");

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

            DataRelation drPerfilUsuarioUsuario = new DataRelation(
                "FK_PerfilUsuarioUsuario",
                dtUsuario.Columns["ID"]!,
                dtPerfilUsuario.Columns["ID_Usuario"]!
            );

            DataRelation drPerfilUsuarioPerfil = new DataRelation(
                "FK_PerfilUsuarioPerfil",
                dtPermiso.Columns["ID"]!,
                dtPerfilUsuario.Columns["ID_Perfil"]!
            );

            mainDataSet.Relations.Add(drPermisoPadre);
            mainDataSet.Relations.Add(drPermisoHijo);

            DataTable? dtIdioma = mainDataSet.Tables["Idioma"];
            DataTable? dtTraduccion = mainDataSet.Tables["Traduccion"];

            if (dtIdioma == null || dtTraduccion == null) throw new Exception("Error en el armado de relaciones DAO para Idiomas");

            DataRelation drIdiomaTraduccion = new DataRelation(
                "FK_Traduccion_Idioma",
                dtIdioma.Columns["Codigo"]!,        // PK en Idioma
                dtTraduccion.Columns["CodigoIdioma"]! // FK en Traduccion
            );

            mainDataSet.Relations.Add(drIdiomaTraduccion);
            mainDataSet.Relations.Add(drPerfilUsuarioUsuario);
            mainDataSet.Relations.Add(drPerfilUsuarioPerfil);
        }

        private void ConfigurarAutoincrementoGeneral()
        {
            //if (!mainDataSet.Tables.Contains("Bitacora") || !mainDataSet.Tables.Contains("Permiso")) return;

            //DataTable dtBitacora = mainDataSet.Tables["Bitacora"]!;
            //DataTable dtPermiso = mainDataSet.Tables["Permiso"]!;

            //if (!dtBitacora.Columns.Contains("ID") || !dtPermiso.Columns.Contains("ID")) return;

            //DataColumn columnaIdRegistroBitacora = dtBitacora.Columns["ID"]!;
            //int maxId = 0;

            //if (dtBitacora.Rows.Count > 0)
            //{
            //    maxId = dtBitacora.AsEnumerable()
            //        .Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"]));
            //}

            //columnaIdRegistroBitacora.AutoIncrement = true;
            //columnaIdRegistroBitacora.AutoIncrementSeed = maxId + 1;
            //columnaIdRegistroBitacora.AutoIncrementStep = 1;

            //DataColumn columnaIdRegistroPermiso = dtPermiso.Columns["ID"]!;
            //int maxIdPermiso = 0;

            //if (dtPermiso.Rows.Count > 0)
            //{
            //    maxIdPermiso = dtPermiso.AsEnumerable()
            //        .Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"]));
            //}

            //columnaIdRegistroPermiso.AutoIncrement = true;
            //columnaIdRegistroPermiso.AutoIncrementSeed = maxIdPermiso + 1;
            //columnaIdRegistroPermiso.AutoIncrementStep = 1;
            if (mainDataSet.Tables.Contains("Bitacora") && mainDataSet.Tables["Bitacora"]!.Columns.Contains("ID"))
            {
                DataTable dtBitacora = mainDataSet.Tables["Bitacora"]!;
                DataColumn columnaIdRegistroBitacora = dtBitacora.Columns["ID"]!;
                int maxId = dtBitacora.Rows.Count > 0 ? dtBitacora.AsEnumerable().Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"])) : 0;
                columnaIdRegistroBitacora.AutoIncrement = true;
                columnaIdRegistroBitacora.AutoIncrementSeed = maxId + 1;
                columnaIdRegistroBitacora.AutoIncrementStep = 1;
            }

            if (mainDataSet.Tables.Contains("Permiso") && mainDataSet.Tables["Permiso"]!.Columns.Contains("ID"))
            {
                DataTable dtPermiso = mainDataSet.Tables["Permiso"]!;
                DataColumn columnaIdRegistroPermiso = dtPermiso.Columns["ID"]!;
                int maxIdPermiso = dtPermiso.Rows.Count > 0 ? dtPermiso.AsEnumerable().Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"])) : 0;
                columnaIdRegistroPermiso.AutoIncrement = true;
                columnaIdRegistroPermiso.AutoIncrementSeed = maxIdPermiso + 1;
                columnaIdRegistroPermiso.AutoIncrementStep = 1;
            }

            // 4. Autoincremento para Traduccion (IdTraduccion)
            if (mainDataSet.Tables.Contains("Traduccion") && mainDataSet.Tables["Traduccion"]!.Columns.Contains("IdTraduccion"))
            {
                DataTable dtTraduccion = mainDataSet.Tables["Traduccion"]!;
                DataColumn colIdTrad = dtTraduccion.Columns["IdTraduccion"]!;
                int maxIdTrad = dtTraduccion.Rows.Count > 0 ? dtTraduccion.AsEnumerable().Max(r => r["IdTraduccion"] == DBNull.Value ? 0 : Convert.ToInt32(r["IdTraduccion"])) : 0;
                colIdTrad.AutoIncrement = true;
                colIdTrad.AutoIncrementSeed = maxIdTrad + 1;
                colIdTrad.AutoIncrementStep = 1;
            }
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
                daIdioma.SelectCommand.Connection = conn;
                daTraduccion.SelectCommand.Connection = conn;
                daPerfilUsuario.SelectCommand.Connection = conn;

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

                daIdioma.InsertCommand = cbIdioma.GetInsertCommand();
                daIdioma.UpdateCommand = cbIdioma.GetUpdateCommand();
                daIdioma.DeleteCommand = cbIdioma.GetDeleteCommand();
                daIdioma.InsertCommand.Connection = conn;
                daIdioma.UpdateCommand.Connection = conn;
                daIdioma.DeleteCommand.Connection = conn;

                daTraduccion.InsertCommand = cbTraduccion.GetInsertCommand();
                daTraduccion.UpdateCommand = cbTraduccion.GetUpdateCommand();
                daTraduccion.DeleteCommand = cbTraduccion.GetDeleteCommand();
                daTraduccion.InsertCommand.Connection = conn;
                daTraduccion.UpdateCommand.Connection = conn;
                daTraduccion.DeleteCommand.Connection = conn;
                daPerfilUsuario.InsertCommand = cbPerfilUsuario.GetInsertCommand();
                daPerfilUsuario.UpdateCommand = cbPerfilUsuario.GetUpdateCommand();
                daPerfilUsuario.DeleteCommand = cbPerfilUsuario.GetDeleteCommand();
                daPerfilUsuario.InsertCommand.Connection = conn;
                daPerfilUsuario.UpdateCommand.Connection = conn;
                daPerfilUsuario.DeleteCommand.Connection = conn;

                daUsers.Update(mainDataSet, "Usuario");
                daBitacora.Update(mainDataSet, "Bitacora");
                daPermiso.Update(mainDataSet, "Permiso");
                daPermisoRelacion.Update(mainDataSet, "PermisoRelacion");
                daIdioma.Update(mainDataSet, "Idioma");
                daTraduccion.Update(mainDataSet, "Traduccion");
                daPerfilUsuario.Update(mainDataSet, "PerfilUsuario");

                mainDataSet.AcceptChanges();
            }
        }

    }
}