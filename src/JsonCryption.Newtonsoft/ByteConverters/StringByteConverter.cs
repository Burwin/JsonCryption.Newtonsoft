using System.Text;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class StringByteConverter : IByteConverter<string>
    {
        public string FromBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public byte[] ToBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
