using BE;
using System.Data;

namespace DAL
{
    public class RepositorioBitacora
    {
        DAO dao;
        public RepositorioBitacora() 
        {
            dao = new DAO();
        }

        public List<Registro> ListarRegistros () => MappearRegistros();
        private List<Registro> MappearRegistros()
        {
            List<Registro> _listRegistro = new List<Registro>();
            foreach (DataRow dr in dao.RetornaDataTableBitacora().Rows)
            {
                DataRow usuarioRelacionado = dr.GetParentRow("FK_Bitacora_Usuario");
                _listRegistro.Add(new Registro((string)usuarioRelacionado[1], (DateTime)dr[2], (string)dr[3]));
            }
            return _listRegistro;
        }
        public void AlmacenarRegistro (Registro r) 
        {
            DataTable dt = dao.RetornaDataTableBitacora();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] {r.Username,r.Fecha,r.Accion };
            dt.Rows.Add(dr);
            dao.ActualizarBitacora(dt);
        }
    }
}
