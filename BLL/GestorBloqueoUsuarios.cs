using BE;
using DAL;

namespace BLL
{
    public class GestorBloqueoUsuarios
    {
        public List<Usuario> ObtenerUsuariosBloqueados()
        {
            // Reutilizamos el método de listar todos los usuarios y filtramos con LINQ
            return RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios()
                                      .Where(u => u.EstaBloqueado)
                                      .ToList();
        }

        public void DesbloquearUsuario(string username)
        {
            RepositorioUsuarios repo = RepositorioUsuarios.GetInstance;

            // 1. Cambiamos el estado a NO bloqueado (false)
            repo.CambiarEstadoBloqueo(username, false);

            // 2. Reiniciamos el contador de intentos fallidos a 0
            repo.ActualizarIntentosFallidos(username, 0);
        }
    }
}
