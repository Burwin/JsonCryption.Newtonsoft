using JsonCryption.Encrypters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    internal abstract class EncryptedConverter<T> : JsonConverter<T>
    {
        protected readonly Encrypter _encrypter;
        protected readonly JsonSerializerOptions _options;

        protected EncryptedConverter(Encrypter encrypter, JsonSerializerOptions options)
        {
            _encrypter = encrypter;
            _options = options;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = ReadToBytes(ref reader, typeToConvert, options);
            return FromBytes(bytes);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var bytes = ToBytes(value);
            writer.WriteStringValue(_encrypter.Encrypt(bytes));
        }

        protected byte[] DecryptString(ref Utf8JsonReader reader)
        {
            // If it's encrypted, it should be a string
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var encrypted = reader.GetString();
            return _encrypter.DecryptToByteArray(encrypted);
        }

        protected abstract T FromBytes(byte[] bytes);

        protected abstract byte[] ToBytes(T value);

        protected virtual byte[] ReadToBytes(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DecryptString(ref reader);
    }
}
