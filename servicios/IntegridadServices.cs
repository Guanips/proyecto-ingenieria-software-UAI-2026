using BE;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace servicios
{
    public static class IntegridadService
    {
        public static string CalcularDVH(Usuario u)
        {
            // Solo se toman Username, Id, Password, Email y Telefono
            string cadena = $"{u.Id}{u.Username}{u.PasswordHash}{u.Email}{u.NumTelefono}";
            return CryptoService.EncriptarPassword(cadena);
        }

        public static string CalcularDVV(List<Usuario> usuarios)
        {
            StringBuilder sb = new StringBuilder();

            // Ordenamiento por ID para asegurar el mismo orden de concatenación
            foreach (var u in usuarios.OrderBy(x => x.Id))
            {
                sb.Append(u.DVH);
            }

            // El hash final de todos los DVH concatenados (incluso si hay solo 1)
            return CryptoService.EncriptarPassword(sb.ToString());
        }
    }
}