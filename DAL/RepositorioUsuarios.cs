using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepositorioUsuarios
    {
        private static RepositorioUsuarios? Instance;

        private RepositorioUsuarios() {}

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
            return new Usuario(parsedId, (string)dr[1], (string)dr[2]);
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
                list_user.Add(new Usuario(parsedId, (string)dr[1], (string)dr[2]));
            }

            return list_user;
        }
        
        public bool VerificarExistenciaDeUsername(string username)
        {
            return ObtenerListadoTotalUsuarios().Exists(x => x.Username == username);

        }

        public Usuario ObtenerUsuarioPorNombre(string username)
        {
            Usuario _user = ObtenerListadoTotalUsuarios().Find(x => x.Username == username);
            return _user;
        }

        public List<Usuario> ObtenerUsuariosBloqueados() 
        {
            return new List<Usuario>();
        }


    }
}
