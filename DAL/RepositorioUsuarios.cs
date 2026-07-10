using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class RepositorioUsuarios
    {
        private static RepositorioUsuarios? Instance;

        private RepositorioUsuarios() { }

        public static RepositorioUsuarios GetInstance
        {
            get
            {
                if (Instance == null) Instance = new RepositorioUsuarios();
                return Instance;
            }
        }

        private void CargarPermisosUsuario(Usuario usuario)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPerfilUsuario = ds.Tables["PerfilUsuario"];

            if (dtPerfilUsuario == null) return;

            RepositorioPerfiles repoPerfiles = new RepositorioPerfiles();
            List<Permiso> permisosDisponibles = repoPerfiles.ObtenerPermisos();

            DataRow[] filasUsuario = dtPerfilUsuario.Select($"ID_Usuario = '{usuario.Id}'");

            foreach (DataRow fila in filasUsuario)
            {
                uint idPermiso = Convert.ToUInt32(fila["ID_Perfil"]);
                Permiso? permisoEncontrado = permisosDisponibles.FirstOrDefault(p => p.ID == idPermiso);

                if (permisoEncontrado != null)
                {
                    usuario.Permisos.Add(permisoEncontrado);
                }
            }
        }

        public Usuario ObtenerUsuario(string username)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable table = ds.Tables["Usuario"];

            if (table == null) throw new Exception("La tabla 'Usuario' no existe en el DataSet.");

            DataRow[] foundRows = table.Select($"Username = '{username}'");
            if (foundRows.Length == 0) throw new Exception($"No se encontró ningún usuario con el nombre de usuario '{username}'.");

            DataRow dr = foundRows[0];
            Guid parsedId = Guid.Parse(dr[0].ToString()!);

            // 1. Instanciamos con datos puros de negocio
            Usuario usuario = new Usuario(parsedId, (string)dr["Username"], (string)dr["PasswordHash"], (string)dr["Email"], (string)dr["NumTelefono"], (bool)dr["EstaBloqueado"], (string)dr["Idioma"], (int)dr["IntentosFallidos"]);

            // 2. Le inyectamos el DVH fuera del constructor
            usuario.DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString()! : "";

            CargarPermisosUsuario(usuario);
            return usuario;
        }

        public List<Usuario> ObtenerListadoTotalUsuarios()
        {
            List<Usuario> list_user = new List<Usuario>();
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable table = ds.Tables["Usuario"];

            if (table == null) throw new Exception("La tabla 'Usuario' no existe en el DataSet.");

            foreach (DataRow dr in table.Rows)
            {
                Guid parsedId = Guid.Parse(dr[0].ToString()!);

                Usuario usuario = new Usuario(parsedId, (string)dr["Username"], (string)dr["PasswordHash"], (string)dr["Email"], (string)dr["NumTelefono"], (bool)dr["EstaBloqueado"], (string)dr["Idioma"], (int)dr["IntentosFallidos"]);
                usuario.DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString()! : "";

                CargarPermisosUsuario(usuario);
                list_user.Add(usuario);
            }

            return list_user;
        }

        public bool VerificarExistenciaDeUsername(string username)
        {
            return ObtenerListadoTotalUsuarios().Exists(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void AgregarUsuario(Usuario nUsuario)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];

            // --- INTERCEPCIÓN ---
            nUsuario.DVH = servicios.IntegridadService.CalcularDVH(nUsuario);

            DataRow newRow = dtUsuarios.NewRow();
            newRow["ID"] = nUsuario.Id;
            newRow["Username"] = nUsuario.Username;
            newRow["PasswordHash"] = nUsuario.PasswordHash;
            newRow["Email"] = nUsuario.Email;
            newRow["NumTelefono"] = nUsuario.NumTelefono;
            newRow["EstaBloqueado"] = nUsuario.EstaBloqueado;
            newRow["Idioma"] = nUsuario.Idioma;
            newRow["IntentosFallidos"] = nUsuario.IntentosFallidos;
            newRow["DVH"] = nUsuario.DVH;

            dtUsuarios.Rows.Add(newRow);
            DAO.GetInstance.SubirCambiosBD();

            // Sincronización automática de DVV y Backup
            RepositorioIntegridad.GetInstance.ActualizarDVVGlobal();
            
        }

        public void ModificarUsuario(Usuario usuarioModificado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{usuarioModificado.Username}'");

            // --- INTERCEPCIÓN ---
            usuarioModificado.DVH = servicios.IntegridadService.CalcularDVH(usuarioModificado);

            DataRow filaUsuario = foundRows[0];
            filaUsuario["Email"] = usuarioModificado.Email;
            filaUsuario["NumTelefono"] = usuarioModificado.NumTelefono;
            filaUsuario["DVH"] = usuarioModificado.DVH;

            DAO.GetInstance.SubirCambiosBD();

            RepositorioIntegridad.GetInstance.ActualizarDVVGlobal();
            
        }

        public void AgregarHistorialUsuario(Usuario usuarioModificado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtHistorialUsuario = ds.Tables["HistorialUsuario"];

            DataRow newRow = dtHistorialUsuario.NewRow();
            newRow["ID_Usuario"] = usuarioModificado.Id;
            newRow["Email"] = usuarioModificado.Email;
            newRow["NumTelefono"] = usuarioModificado.NumTelefono;
            newRow["Fecha"] = DateTime.Now;

            dtHistorialUsuario.Rows.Add(newRow);
            DAO.GetInstance.SubirCambiosBD();
        }

        public void EliminarUsuario(string username)
        {
            Usuario u = ObtenerUsuario(username);

            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];

            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");
            DataRow filaUsuario = foundRows[0];
            filaUsuario.Delete();
            DAO.GetInstance.SubirCambiosBD();

            // --- Sincronización de Bajas ---
            RepositorioIntegridad.GetInstance.ActualizarDVVGlobal();
        }

        public void CambiarEstadoBloqueo(string username, bool estaBloqueado)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");

            if (foundRows.Length > 0)
            {
                DataRow filaUsuario = foundRows[0];
                filaUsuario["EstaBloqueado"] = estaBloqueado;

                if (!estaBloqueado)
                {
                    filaUsuario["IntentosFallidos"] = 0;
                }
                DAO.GetInstance.SubirCambiosBD();
            }
        }

        public void ActualizarIntentosFallidos(string username, int nuevosIntentos)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable dtUsuarios = ds.Tables["Usuario"];
            DataRow[] foundRows = dtUsuarios.Select($"Username = '{username}'");

            if (foundRows.Length > 0)
            {
                foundRows[0]["IntentosFallidos"] = nuevosIntentos;
                DAO.GetInstance.SubirCambiosBD();
            }
        }
    }
}
