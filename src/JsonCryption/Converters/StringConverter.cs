using JsonCryption.Encrypters;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class StringConverter : EncryptedConverter<string>
    {
        public StringConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override string FromBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public override byte[] ToBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
