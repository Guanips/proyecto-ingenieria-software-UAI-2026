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
        public DAO()
        {
            cn = new SqlConnection("Data Source=(local);Initial Catalog=Users;Integrated Security=True;Trust Server Certificate=True");//aun no decidi la base de datos o
            ds = new DataSet("Users");
            dtUsers = new DataTable("Usuarios");
            dtBitacora = new DataTable("Bitacora");
            daUsers = new SqlDataAdapter("Select * From Usuarios", cn);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", cn);
            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);

            ds.Tables.Add(dtUsers);
            ds.Tables.Add(dtBitacora);
            daUsers.Fill(dtUsers);
            daBitacora.Fill(dtBitacora);


            //Para que no me este tirando error porque no hay DB
            dtUsers.PrimaryKey = new DataColumn[] { dtUsers.Columns[0] };
            CrearAdminInicial();
        }
        private void CrearAdminInicial()
        {
            string guidAdmin = Guid.NewGuid().ToString();//admin hardcodeado por primera y unica vez

            SqlCommand cmd = new SqlCommand(
                @"IF NOT EXISTS
                  (
                     SELECT 1
                     FROM Usuarios
                     WHERE Username = @Username
                  )
                  BEGIN
                  INSERT INTO Usuarios
                  (
                     Id,
                     Username,
                     PasswordHash
                  )
                     VALUES
                  (
                     @Id,
                     @Username,
                     @PasswordHash
                  )
                END", cn);

            cmd.Parameters.AddWithValue("@Id", guidAdmin);
            cmd.Parameters.AddWithValue("@Username", "admin");
            cmd.Parameters.AddWithValue("@PasswordHash", "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918");//amin hasheado x2

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public DataTable RetornaDataTableUsuarios() => dtUsers;
        public DataTable RetornaDataTableBitacora() => dtBitacora;

        public void ActualizarBitacora (DataTable dtBitacora) => daBitacora.Update(dtBitacora);
        
        //public void ActualizarUsuarios (DataTable dtUsuarios) => daUsers.Update(dtUsuarios);
    }
}