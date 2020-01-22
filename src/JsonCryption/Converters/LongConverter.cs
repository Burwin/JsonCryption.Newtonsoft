using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class LongConverter : EncryptedConverter<long>
    {
        public LongConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);
        public override byte[] ToBytes(long value) => BitConverter.GetBytes(value);
    }
}
