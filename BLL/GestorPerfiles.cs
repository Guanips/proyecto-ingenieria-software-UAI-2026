using BE;
using DAL;

namespace BLL
{
    public class GestorPerfiles
    {
        private RepositorioPerfiles repositorioPerfiles;

        public GestorPerfiles()
        {
            repositorioPerfiles = new RepositorioPerfiles();
        }

        public void CrearPerfil(string nombrePerfil)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();
            bool existePermiso = permisosGlobales.Any(p => p.Nombre.Equals(nombrePerfil));
            if (existePermiso)
            {
                throw new Exception($"Ya existe un permiso o perfil con el nombre '{nombrePerfil}'.");
            }
            repositorioPerfiles.CrearPerfil(nombrePerfil);
        }

        public void AgregarPermisoAPerfil(uint idPerfil, uint idPermiso)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();

            Perfil? perfil = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfil);
            if (perfil == null)
            {
                throw new Exception($"No se encontró el perfil con ID {idPerfil}.");
            }

            Permiso? permiso = permisosGlobales.FirstOrDefault(p => p.ID == idPermiso);
            if (permiso == null)
            {
                throw new Exception($"No se encontró el permiso con ID {idPermiso}.");
            }

            bool permisoYaExiste = perfil.PermisosAsociados.Any(p => p.ID == idPermiso);
            if (permisoYaExiste)
            {
                throw new Exception($"El permiso con ID {idPermiso} ya está asociado al perfil '{perfil.Nombre}'.");
            }

            repositorioPerfiles.AsociarPermisoConPermiso(idPerfil, idPermiso);
        }

        public void AgregarPerfilAPerfil(uint idPerfilPadre, uint idPerfilHijo)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();

            Perfil? perfilPadre = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfilPadre);
            if (perfilPadre == null)
            {
                throw new Exception($"No se encontró el perfil padre con ID {idPerfilPadre}.");
            }

            Perfil? perfilHijo = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfilHijo);
            if (perfilHijo == null)
            {
                throw new Exception($"No se encontró el perfil hijo con ID {idPerfilHijo}.");
            }

            // Validar que no sea auto-referencia
            if (idPerfilPadre == idPerfilHijo)
            {
                throw new Exception($"Un perfil no puede contenerse a sí mismo.");
            }

            // Validar que no exista ya
            bool perfilYaExiste = perfilPadre.PermisosAsociados.Any(p => p.ID == idPerfilHijo);
            if (perfilYaExiste)
            {
                throw new Exception($"El perfil con ID {idPerfilHijo} ya está asociado al perfil '{perfilPadre.Nombre}'.");
            }

            // Validar referencias circulares (anidación)
            if (ContieneReferenciaCircular(perfilHijo, idPerfilPadre, permisosGlobales))
            {
                throw new Exception($"Agregar el perfil '{perfilHijo.Nombre}' al perfil '{perfilPadre.Nombre}' crearía una referencia circular.");
            }

            repositorioPerfiles.AsociarPermisoConPermiso(idPerfilPadre, idPerfilHijo);
        }

        public void AgregarPerfilAUsuario(string username, uint idPerfil)
        {
            Usuario usuarioSeleccionado = RepositorioUsuarios.GetInstance.ObtenerUsuario(username);
            if (usuarioSeleccionado.Permisos.Any(p => p.ID == idPerfil)) throw new Exception("El usuario seleccionado ya posee ese perfil");
            repositorioPerfiles.AsociarPerfilAUsuario(usuarioSeleccionado, idPerfil);
        }

        private bool ContieneReferenciaCircular(Perfil perfilHijo, uint idPerfilPadre, List<Permiso> permisosGlobales)
        {
            HashSet<uint> visitados = new HashSet<uint>();
            return VerificarCircularRecursivo(perfilHijo, idPerfilPadre, visitados, permisosGlobales);
        }

        private bool VerificarCircularRecursivo(Perfil perfilActual, uint idObjetivo, HashSet<uint> visitados, List<Permiso> permisosGlobales)
        {
            // Si ya visitamos este perfil, hay un ciclo
            if (visitados.Contains(perfilActual.ID))
            {
                return true;
            }

            visitados.Add(perfilActual.ID);

            // Verificar si alguno de los permisos asociados contiene al perfil padre
            foreach (Permiso permisoAsociado in perfilActual.PermisosAsociados)
            {
                if (permisoAsociado.ID == idObjetivo)
                {
                    return true;
                }

                // Si es un perfil, recursivamente verificar sus permisos
                if (permisoAsociado is Perfil perfilAnidado)
                {
                    if (VerificarCircularRecursivo(perfilAnidado, idObjetivo, visitados, permisosGlobales))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DesasignarPerfilDeUsuario(string username, uint idPerfil)
        {
            Usuario usuarioSeleccionado = RepositorioUsuarios.GetInstance.ObtenerUsuario(username);
            if (!usuarioSeleccionado.Permisos.Any(p => p.ID == idPerfil)) throw new Exception("El usuario seleccionado no posee ese perfil");
            repositorioPerfiles.DesasociarPerfilDeUsuario(usuarioSeleccionado.Id, idPerfil);
        }

        public List<Permiso> ListarPermisos()
        {
            return repositorioPerfiles.ObtenerPermisos();
        }

        public List<Usuario> ListarUsuarios()
        {
            return RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();
        }
    }
}
