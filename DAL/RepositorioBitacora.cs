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
                DataRow? usuarioRelacionado = dr.GetParentRow("FK_Bitacora_Usuario");
                if (usuarioRelacionado == null) throw new Exception("Usuario relacionado a registro de bitacora no encontrado");
                _listRegistro.Add(new Registro((string)usuarioRelacionado[1], (DateTime)dr[2], (string)dr[3]));
            }
            return _listRegistro;
        }
        public void AlmacenarRegistro (Registro nRegistro, string idUsuarioInvolucrado) 
        {
            DataTable tablaBitacora = dao.RetornaDataTableBitacora();
            DataRow nFilaRegistro = tablaBitacora.NewRow();
            nFilaRegistro[1] = idUsuarioInvolucrado;
            nFilaRegistro[2] = nRegistro.Fecha;
            nFilaRegistro[3] = nRegistro.Accion;
            tablaBitacora.Rows.Add(nFilaRegistro);
            dao.ActualizarBitacora(tablaBitacora);
        }
    }
}
