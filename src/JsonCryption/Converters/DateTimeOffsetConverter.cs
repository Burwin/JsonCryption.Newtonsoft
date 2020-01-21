using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        public DateTimeOffsetConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override DateTimeOffset FromBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        protected override byte[] ToBytes(DateTimeOffset value) => throw new NotImplementedException();
    }
}
