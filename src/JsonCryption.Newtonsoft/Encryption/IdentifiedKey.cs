namespace JsonCryption.Newtonsoft.Encryption
{
    public sealed class IdentifiedKey
    {
        public int ID { get; }
        public byte[] Key { get; }

        public IdentifiedKey(int id, byte[] key)
        {
            ID = id;
            Key = key;
        }
    }
}
