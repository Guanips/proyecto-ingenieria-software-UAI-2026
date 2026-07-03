using BE;
using System.Data;

namespace DAL
{
    public class RepositorioBitacora
    {
        public RepositorioBitacora() { }

        public List<Registro> ListarRegistros() => MappearRegistros();
        private List<Registro> MappearRegistros()
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable tablaBitacora = ds.Tables["Bitacora"];

            if (tablaBitacora == null) throw new Exception("Tabla de bitacora no encontrada en el DataSet");

            List<Registro> _listRegistro = new List<Registro>();
            //foreach (DataRow dr in tablaBitacora.Rows)
            //{
            //    DataRow? usuarioRelacionado = dr.GetParentRow("FK_Bitacora_Usuario");
            //    if (usuarioRelacionado == null) throw new Exception("Usuario relacionado a registro de bitacora no encontrado");
            //    _listRegistro.Add(new Registro((string)usuarioRelacionado[1], (DateTime)dr[2], (string)dr[3]));
            //}
            foreach (DataRow dr in tablaBitacora.Rows)
            {
                DataRow? usuarioRelacionado = dr.GetParentRow("FK_Bitacora_Usuario");

                // En vez de explotar, usamos los datos directos de la fila de Bitácora o un fallback
                string username = usuarioRelacionado != null ? (string)usuarioRelacionado[1] : (string)dr["Username"];

                _listRegistro.Add(new Registro(username, (DateTime)dr[2], (string)dr[3]));
            }
            return _listRegistro;
        }
        public void AlmacenarRegistro(Registro nRegistro, string idUsuarioInvolucrado)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable tablaBitacora = ds.Tables["Bitacora"];

            if (tablaBitacora == null) throw new Exception("Tabla de bitacora no encontrada en el DataSet");

            DataRow nFilaRegistro = tablaBitacora.NewRow();
            nFilaRegistro[1] = idUsuarioInvolucrado;
            nFilaRegistro[2] = nRegistro.Fecha;
            nFilaRegistro[3] = nRegistro.Accion;
            tablaBitacora.Rows.Add(nFilaRegistro);
            dao.SubirCambiosBD();
        }
    }
}
