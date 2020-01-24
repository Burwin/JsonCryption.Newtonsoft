using JsonCryption.Newtonsoft.Encryption;
using System;
using System.Security.Cryptography;

namespace JsonCryption.Newtonsoft.Tests
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

        internal static IEncryptionBlobTranslater GetTestEncryptionInfoTranslator() => new EncryptionBlobTranslater();
        
        internal static IAlgorithmProvider GetTestAlgorithmProvider()
        {
            var provider = new AlgorithmProvider();
            provider.Register(1, () => Aes.Create());
            provider.RegisterDecrypterOnly(2, () => SymmetricAlgorithm.Create());
            return provider;
        }

        internal static IKeyProvider GetTestKeyProvider()
        {
            var provider = new KeyProvider();
            provider.Register(1, GenerateRandomKey());
            provider.RegisterAsDecryptingKeyOnly(2, GenerateRandomKey());
            return provider;
        }
        internal static Encrypter GetTestEncrypter()
            => new Encrypter(GetTestKeyProvider(), GetTestAlgorithmProvider(), GetTestEncryptionInfoTranslator());
    }
}
