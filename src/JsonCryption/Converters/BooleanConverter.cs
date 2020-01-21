using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class BooleanConverter : EncryptedConverter<bool>
    {
        public BooleanConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = DecryptString(ref reader);
            return BitConverter.ToBoolean(bytes, startIndex: 0);
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            => writer.WriteStringValue(_encrypter.Encrypt(value));
    }
}
