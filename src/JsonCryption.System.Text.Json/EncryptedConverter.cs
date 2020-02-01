using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    internal abstract class EncryptedConverter<T> : JsonConverter<T>
    {
        protected readonly IDataProtector _dataProtector;
        protected readonly JsonSerializerOptions _options;

        protected EncryptedConverter(IDataProtector dataProtector, JsonSerializerOptions options)
        {
            _dataProtector = dataProtector;
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
            var base64 = Convert.ToBase64String(bytes);
            var cipherText = _dataProtector.Protect(base64);
            writer.WriteStringValue(cipherText);
        }

        protected byte[] DecryptString(ref Utf8JsonReader reader)
        {
            // If it's encrypted, it should be a string
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var cipherText = reader.GetString();
            var base64 = _dataProtector.Unprotect(cipherText);
            return Convert.FromBase64String(base64);
        }

        public abstract T FromBytes(byte[] bytes);

        public abstract byte[] ToBytes(T value);

        protected virtual byte[] ReadToBytes(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DecryptString(ref reader);
    }
}
