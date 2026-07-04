using BE;
using DAL;
using servicios;
using System.Collections.Generic;

namespace BLL
{
    public static class GestorIntegridad
    {
        public static List<Usuario> VerificarIntegridadDVH()
        {
            List<Usuario> usuariosCorruptos = new List<Usuario>();
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();

            foreach (var u in usuarios)
            {
                if (u.DVH != IntegridadService.CalcularDVH(u))
                {
                    usuariosCorruptos.Add(u);
                }
            }
            return usuariosCorruptos;
        }

        public static bool VerificarIntegridadDVV()
        {
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();

            string dvvCalculado = IntegridadService.CalcularDVV(usuarios);
            string dvvAlmacenado = RepositorioIntegridad.GetInstance.ObtenerDVV("Usuario");

            if (string.IsNullOrEmpty(dvvAlmacenado)) return true;

            return dvvCalculado == dvvAlmacenado;
        }
    }
}