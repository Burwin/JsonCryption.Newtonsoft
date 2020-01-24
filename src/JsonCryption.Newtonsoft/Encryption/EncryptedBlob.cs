namespace JsonCryption.Newtonsoft.Encryption
{
    public struct EncryptedBlob
    {
        public int KeyId { get; }
        public int AlgorithmId { get; }
        public string EncryptedText { get; }

        public EncryptedBlob(int keyId, int algorithmId, string encryptedText)
        {
            KeyId = keyId;
            AlgorithmId = algorithmId;
            EncryptedText = encryptedText;
        }
    }
}
