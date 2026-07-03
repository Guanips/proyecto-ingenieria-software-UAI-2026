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
            return new Usuario(parsedId, (string)dr[1], (string)dr[2], (string)dr[3], (string)dr[4], (bool)dr[5], (string)dr[6], (int)dr[7]);
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
                list_user.Add(new Usuario(parsedId, (string)dr[1], (string)dr[2], (string)dr[3], (string)dr[4], (bool)dr[5], (string)dr[6], (int)dr[7]));
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
