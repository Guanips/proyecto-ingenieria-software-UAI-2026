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
                    sb.Append(b.ToString("X2")); // Retorna admin = 8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918
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
