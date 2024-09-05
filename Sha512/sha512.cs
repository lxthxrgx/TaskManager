using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Sha512
{
    public static class HashHelper
    {
        public static string ComputeSha512Hash(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] hash;

            using (SHA512 sha512 = SHA512.Create())
            {
                hash = sha512.ComputeHash(data);
            }

            return Convert.ToBase64String(hash);
        }
    }
}
