namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class ByteByteConverter : IByteConverter<byte>
    {
        public byte FromBytes(byte[] bytes) => bytes[0];
        public byte[] ToBytes(byte value) => new[] { value };
    }
}
