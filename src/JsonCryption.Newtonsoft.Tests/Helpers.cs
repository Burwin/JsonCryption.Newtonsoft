using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace Newtonsoft.Json.FLE.Tests
{
    class Helpers
    {
        public static byte[] GenerateRandomKey()
        {
            using var aes = Aes.Create();
            return aes.Key;
        }

        internal static IDataProtectionProvider GetTestDataProtectionProvider(string applicationName) => DataProtectionProvider.Create(applicationName);
    }
}
