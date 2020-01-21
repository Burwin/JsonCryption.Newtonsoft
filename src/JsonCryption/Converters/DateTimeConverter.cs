using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeConverter : EncryptedConverter<DateTime>
    {
        public DateTimeConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = DecryptString(ref reader);
            var dateAsLong = BitConverter.ToInt64(bytes, 0);
            return DateTime.FromBinary(dateAsLong);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(_encrypter.Encrypt(value));
    }
}
