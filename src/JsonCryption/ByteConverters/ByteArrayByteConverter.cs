namespace JsonCryption.ByteConverters
{
    public sealed class ByteArrayByteConverter : IByteConverter<byte[]>
    {
        public byte[] FromBytes(byte[] bytes) => bytes;
        public byte[] ToBytes(byte[] value) => value;
    }
}
