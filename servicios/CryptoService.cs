using System.Security.Cryptography;
using System.Text;

namespace servicios
{
    public class CryptoService
    {
        public static string EncriptarPassword (string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in passwordByte)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static bool Comparer(string password, string passwordDB) 
        {
            return password == passwordDB;
        }
    }
}
