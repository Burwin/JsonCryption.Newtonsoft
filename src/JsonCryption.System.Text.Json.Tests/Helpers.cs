using System.Security.Cryptography;

namespace JsonCryption.System.Text.Json.Tests
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
