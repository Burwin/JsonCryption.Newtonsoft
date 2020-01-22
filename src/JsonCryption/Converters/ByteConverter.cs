using JsonCryption.Encrypters;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ByteConverter : EncryptedConverter<byte>
    {
        public ByteConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override byte FromBytes(byte[] bytes) => bytes[0];
        public override byte[] ToBytes(byte value) => new[] { value };
    }
}
