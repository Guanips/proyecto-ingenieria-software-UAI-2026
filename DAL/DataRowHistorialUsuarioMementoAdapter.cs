using BE;
using System.Data;

namespace DAL
{
    public class DataRowHistorialUsuarioMementoAdapter : IEstadoUsuario
    {
        private readonly DataRow _row;

        public DataRowHistorialUsuarioMementoAdapter(DataRow row)
        {
            _row = row;
        }

        public DateTime Fecha => Convert.ToDateTime(_row["Fecha"]);
        public string Descripcion => "Recuperado desde BD";

        public string Email => _row["Email"] != DBNull.Value ? _row["Email"].ToString()! : string.Empty;
        public string NumTelefono => _row["NumTelefono"] != DBNull.Value ? _row["NumTelefono"].ToString()! : string.Empty;
    }
}
