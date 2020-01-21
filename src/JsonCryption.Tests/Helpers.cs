using System.Security.Cryptography;

namespace JsonCryption.Tests
{
    class Helpers
    {
        public static byte[] GenerateRandomKey()
        {
            using (var aes = Aes.Create())
            {
                return aes.Key;
            }
        }
    }
}
