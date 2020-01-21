using System;
using System.Security.Cryptography;

namespace JsonCryption.Encrypters
{
    internal sealed class AesManagedEncrypter : Encrypter
    {
        public AesManagedEncrypter(byte[] key) : base(key)
        {
        }

        public override string Encrypt(bool value) => EncryptBytes(BitConverter.GetBytes(value));

        protected override SymmetricAlgorithm GetAlgorithm() => new AesManaged();
    }
}
