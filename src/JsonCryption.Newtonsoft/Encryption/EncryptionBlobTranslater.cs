using System;

namespace JsonCryption.Newtonsoft.Encryption
{
    internal class EncryptionBlobTranslater : IEncryptionBlobTranslater
    {
        public EncryptedBlob FromCipher(string cipher) => throw new NotImplementedException();
        public string ToCipher(EncryptedBlob encryptionInfo) => throw new NotImplementedException();
    }
}
