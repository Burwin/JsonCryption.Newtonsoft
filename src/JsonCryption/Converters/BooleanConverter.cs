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
            // If it's encrypted, it should be a string
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var encrypted = reader.GetString();
            var bytes = _encrypter.DecryptToByteArray(encrypted);
            return BitConverter.ToBoolean(bytes, startIndex: 0);
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            => writer.WriteStringValue(_encrypter.Encrypt(value));
    }
}
