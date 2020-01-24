namespace JsonCryption.Newtonsoft.Encryption
{
    public interface IEncryptionBlobTranslater
    {
        EncryptedBlob FromCipher(string cipher);
        string ToCipher(EncryptedBlob encryptionInfo);
    }
}