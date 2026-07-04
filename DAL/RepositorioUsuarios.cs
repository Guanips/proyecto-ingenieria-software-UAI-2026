using BE;
using System.Data;

namespace DAL
{
    public class RepositorioUsuarios
    {
        private static RepositorioUsuarios? Instance;

        private RepositorioUsuarios() { }

        public static RepositorioUsuarios GetInstance
        {
            get
            {
                if (Instance == null) Instance = new RepositorioUsuarios();
                return Instance;
            }
        }

        private void CargarPermisosUsuario(Usuario usuario)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPerfilUsuario = ds.Tables["PerfilUsuario"];

            if (dtPerfilUsuario == null)
            {
                return;
            }

            RepositorioPerfiles repoPerfiles = new RepositorioPerfiles();
            List<Permiso> permisosDisponibles = repoPerfiles.ObtenerPermisos();

            DataRow[] filasUsuario = dtPerfilUsuario.Select($"ID_Usuario = '{usuario.Id}'");

            foreach (DataRow fila in filasUsuario)
            {
                uint idPermiso = Convert.ToUInt32(fila["ID_Perfil"]);
                Permiso? permisoEncontrado = permisosDisponibles.FirstOrDefault(p => p.ID == idPermiso);

                if (permisoEncontrado != null)
                {
                    usuario.Permisos.Add(permisoEncontrado);
                }
            }
        }

        public Usuario ObtenerUsuario(string username)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable table = ds.Tables["Usuario"];
            if (table == null)
            {
                throw new Exception("La tabla 'Usuario' no existe en el DataSet.");
            }
            DataRow[] foundRows = table.Select($"Username = '{username}'");
            if (foundRows.Length == 0)
            {
                throw new Exception($"No se encontró ningún usuario con el nombre de usuario '{username}'.");
            }
            DataRow dr = foundRows[0];
            Guid parsedId = Guid.Parse(dr[0].ToString());
            Usuario usuario = new Usuario(parsedId, (string)dr["Username"], (string)dr["PasswordHash"], (string)dr["Email"], (string)dr["NumTelefono"], (bool)dr["EstaBloqueado"], (string)dr["Idioma"], (int)dr["IntentosFallidos"]);
            CargarPermisosUsuario(usuario);
            return usuario;
        }

        public List<Usuario> ObtenerListadoTotalUsuarios()
        {
            List<Usuario> list_user = new List<Usuario>();
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable table = ds.Tables["Usuario"];

            if (table == null)
            {
                throw new Exception("La tabla 'Usuario' no existe en el DataSet.");
            }

            foreach (DataRow dr in table.Rows)
            {
                Guid parsedId = Guid.Parse(dr[0].ToString());
                Usuario usuario = new Usuario(parsedId, (string)dr["Username"], (string)dr["PasswordHash"], (string)dr["Email"], (string)dr["NumTelefono"], (bool)dr["EstaBloqueado"], (string)dr["Idioma"], (int)dr["IntentosFallidos"]);
                CargarPermisosUsuario(usuario);
                list_user.Add(usuario);
            }

            return list_user;
        }

        public bool VerificarExistenciaDeUsername(string username)
        {
            return ObtenerListadoTotalUsuarios().Exists(x => x.Username == username);

        }

        public List<Usuario> ObtenerUsuariosBloqueados()
        {
            return new List<Usuario>();
        }

        public void AgregarUsuario(Usuario nUsuario)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();

            DataTable dtUsuarios = ds.Tables["Usuario"];
            if (dtUsuarios == null) throw new Exception("No se encontró la tabla de usuarios en el DataSet");

            DataRow newRow = dtUsuarios.NewRow();
            newRow[0] = nUsuario.Id;
            newRow[1] = nUsuario.Username;
            newRow[2] = nUsuario.PasswordHash;
            newRow[3] = nUsuario.Email;
            newRow[4] = nUsuario.NumTelefono;
            newRow[5] = nUsuario.EstaBloqueado;
            newRow[6] = nUsuario.Idioma;
            newRow[7] = nUsuario.IntentosFallidos;

            dtUsuarios.Rows.Add(newRow);
            DAO.GetInstance.SubirCambiosBD();
        }

        public void ModificarUsuario(Usuario usuarioModificado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();

            DataTable dtUsuarios = ds.Tables["Usuario"];
            if (dtUsuarios == null) throw new Exception("No se encontró la tabla de usuarios en el DataSet");
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{usuarioModificado.Username}'");
            if (foundRows.Length == 0)
            {
                throw new Exception($"No se encontró ningún usuario con el username: {usuarioModificado.Username}");
            }

            DataRow filaUsuario = foundRows[0];
            filaUsuario["Email"] = usuarioModificado.Email;
            filaUsuario["NumTelefono"] = usuarioModificado.NumTelefono;

            DAO.GetInstance.SubirCambiosBD();
        }

        public void AgregarHistorialUsuario(Usuario usuarioModificado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtHistorialUsuario = ds.Tables["HistorialUsuario"];
            if (dtHistorialUsuario == null) throw new Exception("No se encontró la tabla de historial de usuarios en el DataSet");
            DataRow newRow = dtHistorialUsuario.NewRow();
            newRow["ID_Usuario"] = usuarioModificado.Id;
            newRow["Email"] = usuarioModificado.Email;
            newRow["NumTelefono"] = usuarioModificado.NumTelefono;
            newRow["Fecha"] = DateTime.Now;
            dtHistorialUsuario.Rows.Add(newRow);
            DAO.GetInstance.SubirCambiosBD();
        }

        public void EliminarUsuario(string username)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            if (dtUsuarios == null) throw new Exception("No se encontró la tabla de usuarios en el DataSet");
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");
            if (foundRows.Length == 0)
            {
                throw new Exception($"No se encontró ningún usuario con el username: {username}");
            }
            DataRow filaUsuario = foundRows[0];
            filaUsuario.Delete();
            DAO.GetInstance.SubirCambiosBD();
        }
        public void CambiarEstadoBloqueo(string username, bool estaBloqueado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");

            if (foundRows.Length > 0)
            {
                DataRow filaUsuario = foundRows[0];
                filaUsuario["EstaBloqueado"] = estaBloqueado;

                // Si el admin lo desbloquea, los fallos vuelven a 0
                if (!estaBloqueado)
                {
                    filaUsuario["IntentosFallidos"] = 0;
                }

                DAO.GetInstance.SubirCambiosBD();
            }
        }

        public void ActualizarIntentosFallidos(string username, int nuevosIntentos)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");

            if (foundRows.Length > 0)
            {
                foundRows[0]["IntentosFallidos"] = nuevosIntentos;
                DAO.GetInstance.SubirCambiosBD();
            }
        }
    }
}
