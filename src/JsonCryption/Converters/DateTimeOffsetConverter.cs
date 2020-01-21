using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        public DateTimeOffsetConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
