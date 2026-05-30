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
        DAO dao;
        public RepositorioUsuarios()
        {
            dao = new DAO();
        }

        public List<Usuario> ObtenerListaUsuario()
        {
            List<Usuario> list_user = new List<Usuario>();

            DataTable table = dao.RetornaDataTableUsuarios();
            foreach (DataRow dr in table.Rows)
            {
                list_user.Add(new Usuario(dr.ItemArray));
            }
            return list_user;
        }
        
        public bool VerificarUsuarioPorNombre(string username)
        {
            return ObtenerListaUsuario().Exists(x => x.Username == username);

        }

        public Usuario ObtenerUsuarioPorNombre(string username)
        {
            Usuario _user = ObtenerListaUsuario().Find(x => x.Username == username);
            return _user;
        }


    }
}
