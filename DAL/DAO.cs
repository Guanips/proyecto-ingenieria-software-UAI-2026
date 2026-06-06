using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using BE;

namespace DAL
{
    public class DAO
    {
        SqlConnection cn;
        DataSet ds;
        DataTable dtUsers, dtBitacora;
        SqlCommandBuilder cbUsers, cbBitacora;
        SqlDataAdapter daUsers, daBitacora;
        DataRelation drUsuarioBitacora;

        public DAO()
        {
            cn = new SqlConnection("Data Source=.;Initial Catalog=db_demostracion_ing_soft_cg;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");//aun no decidi la base de datos o
            ds = new DataSet("Users");
            dtUsers = new DataTable("Usuario");
            dtBitacora = new DataTable("Bitacora");
            daUsers = new SqlDataAdapter("Select * From Usuario", cn);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", cn);
            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);

            ds.Tables.Add(dtUsers);
            ds.Tables.Add(dtBitacora);
            daUsers.Fill(dtUsers);
            daBitacora.Fill(dtBitacora);

            dtUsers.PrimaryKey = new DataColumn[] { dtUsers.Columns[0] };
            dtBitacora.PrimaryKey = new DataColumn[] {dtBitacora.Columns[0] };
            drUsuarioBitacora = new DataRelation("FK_Bitacora_Usuario", dtUsers.Columns[0], dtBitacora.Columns[1]);
            ds.Relations.Add(drUsuarioBitacora);
        }

        public DataTable RetornaDataTableUsuarios() => dtUsers;
        public DataTable RetornaDataTableBitacora() => dtBitacora;

        public void ActualizarBitacora (DataTable dtBitacora) => daBitacora.Update(dtBitacora);
        
        //public void ActualizarUsuarios (DataTable dtUsuarios) => daUsers.Update(dtUsuarios);
    }
}