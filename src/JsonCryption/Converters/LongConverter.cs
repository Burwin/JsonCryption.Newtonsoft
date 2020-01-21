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

        protected override long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);
        protected override byte[] ToBytes(long value) => BitConverter.GetBytes(value);
    }
}
