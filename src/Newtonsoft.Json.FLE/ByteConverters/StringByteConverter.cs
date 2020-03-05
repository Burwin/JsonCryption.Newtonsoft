using System.Text;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class StringByteConverter : IByteConverter<string>
    {
        public string FromBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public byte[] ToBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
