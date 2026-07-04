using BE;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class RepositorioIntegridad
    {
        private static RepositorioIntegridad? Instance;
        private static readonly object _lock = new object();

        private RepositorioIntegridad() { }

        public static RepositorioIntegridad GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        if (Instance == null)
                        {
                            Instance = new RepositorioIntegridad();
                        }
                    }
                }
                return Instance;
            }
        }

        public string ObtenerDVV(string nombreTabla)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtDVV = ds.Tables["DVV"];

            if (dtDVV == null) return string.Empty;

            DataRow[] rows = dtDVV.Select($"NombreTabla = '{nombreTabla}'");
            if (rows.Length > 0)
            {
                return rows[0]["ValorHash"].ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        public void ActualizarDVV(string nombreTabla, string nuevoValorHash)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtDVV = ds.Tables["DVV"];

            if (dtDVV == null) throw new Exception("La tabla DVV no existe en el DataSet. Asegúrate de haberla mapeado en DAO.cs.");

            DataRow[] rows = dtDVV.Select($"NombreTabla = '{nombreTabla}'");
            if (rows.Length > 0)
            {
                rows[0]["ValorHash"] = nuevoValorHash;
            }
            else
            {
                DataRow newRow = dtDVV.NewRow();
                newRow["NombreTabla"] = nombreTabla;
                newRow["ValorHash"] = nuevoValorHash;
                dtDVV.Rows.Add(newRow);
            }

            DAO.GetInstance.SubirCambiosBD();
        }

        public void ActualizarDVVGlobal()
        {
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();
            string nuevoDVV = servicios.IntegridadService.CalcularDVV(usuarios);
            ActualizarDVV("Usuario", nuevoDVV);
        }

        public void GuardarBackup(Usuario u)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtBackup = ds.Tables["UsuarioBackup"];

            if (dtBackup == null) throw new Exception("La tabla UsuarioBackup no existe en el DataSet.");

            DataRow[] rows = dtBackup.Select($"ID = '{u.Id}'");
            DataRow row;

            if (rows.Length > 0)
            {
                row = rows[0];
            }
            else
            {
                row = dtBackup.NewRow();
                row["ID"] = u.Id;
                dtBackup.Rows.Add(row);
            }

            row["Username"] = u.Username;
            row["PasswordHash"] = u.PasswordHash;
            row["Email"] = u.Email;
            row["NumTelefono"] = u.NumTelefono;
            row["EstaBloqueado"] = u.EstaBloqueado;
            row["Idioma"] = u.Idioma;
            row["IntentosFallidos"] = u.IntentosFallidos;
            row["DVH"] = string.IsNullOrEmpty(u.DVH) ? (object)DBNull.Value : u.DVH;

            DAO.GetInstance.SubirCambiosBD();
        }

        public void EliminarDeBackup(Guid idUsuario)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtBackup = ds.Tables["UsuarioBackup"];

            if (dtBackup == null) throw new Exception("La tabla UsuarioBackup no existe en el DataSet.");

            DataRow[] rows = dtBackup.Select($"ID = '{idUsuario}'");
            if (rows.Length > 0)
            {
                rows[0].Delete();
                DAO.GetInstance.SubirCambiosBD();
            }
        }
    }
}