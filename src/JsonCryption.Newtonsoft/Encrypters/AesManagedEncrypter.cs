using System.Security.Cryptography;

namespace JsonCryption.Newtonsoft.Encrypters
{
    internal sealed class AesManagedEncrypter : Encrypter
    {
        public AesManagedEncrypter(byte[] key) : base(key)
        {
        }

        protected override SymmetricAlgorithm GetAlgorithm() => new AesManaged();
    }
}
