using System.Data;

namespace DAL
{
    public class RepositorioHistorialUsuario
    {
        // Obtiene todos los registros históricos de un usuario en particular
        public DataRow[] ObtenerHistorialPorUsuario(Guid idUsuario)
        {
            DataTable dtHistorial = DAO.GetInstance.ObtenerDataSet().Tables["HistorialUsuario"]!;

            // Usamos Select para filtrar en memoria por el ID del usuario.
            // Ordenamos por fecha descendente para tener el historial más reciente primero.
            return dtHistorial.Select($"ID_Usuario = '{idUsuario}'", "Fecha DESC");
        }

        // Guarda un nuevo registro en el DataSet
        public void GuardarHistorial(Guid idUsuario, string email, string numTelefono, DateTime fecha)
        {
            DataTable dtHistorial = DAO.GetInstance.ObtenerDataSet().Tables["HistorialUsuario"]!;

            DataRow nuevaFila = dtHistorial.NewRow();
            nuevaFila["ID_Usuario"] = idUsuario;
            nuevaFila["Email"] = string.IsNullOrEmpty(email) ? DBNull.Value : email;
            nuevaFila["NumTelefono"] = string.IsNullOrEmpty(numTelefono) ? DBNull.Value : numTelefono;
            nuevaFila["Fecha"] = fecha;

            dtHistorial.Rows.Add(nuevaFila);
        }
    }
}