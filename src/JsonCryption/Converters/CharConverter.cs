using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class CharConverter : EncryptedConverter<char>
    {
        public CharConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override char Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = DecryptString(ref reader);
            return BitConverter.ToChar(bytes, 0);
        }

        public override void Write(Utf8JsonWriter writer, char value, JsonSerializerOptions options)
            => writer.WriteStringValue(_encrypter.Encrypt(value));
    }
}
