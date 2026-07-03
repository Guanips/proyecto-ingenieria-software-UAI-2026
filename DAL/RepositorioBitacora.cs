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
            foreach (DataRow dr in tablaBitacora.Rows)
            {
                _listRegistro.Add(new Registro((string)dr["Username"], (DateTime)dr["Fecha"], (string)dr["Accion"]));
            }
            return _listRegistro;
        }

        public void AlmacenarRegistro(Registro nRegistro)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable tablaBitacora = ds.Tables["Bitacora"];

            if (tablaBitacora == null) throw new Exception("Tabla de bitacora no encontrada en el DataSet");

            DataRow nFilaRegistro = tablaBitacora.NewRow();
            nFilaRegistro["Username"] = nRegistro.Username;
            nFilaRegistro["Fecha"] = nRegistro.Fecha;
            nFilaRegistro["Accion"] = nRegistro.Accion;
            tablaBitacora.Rows.Add(nFilaRegistro);
            dao.SubirCambiosBD();
        }
    }
}
