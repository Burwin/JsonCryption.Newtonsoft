using JsonCryption.Encrypters;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ByteArrayConverter : EncryptedConverter<byte[]>
    {
        public ByteArrayConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override byte[] FromBytes(byte[] bytes) => bytes;
        protected override byte[] ToBytes(byte[] value) => value;
    }
}
