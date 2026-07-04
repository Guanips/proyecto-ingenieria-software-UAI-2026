using BE;
using System.Data;

namespace DAL
{
    public class HistorialUsuarioMapper
    {
        public static IMementoUsuario MapToMemento(DataRow row)
        {
            return new DataRowHistorialUsuarioMementoAdapter(row);
        }
    }
}
