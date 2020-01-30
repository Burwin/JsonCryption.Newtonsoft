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
            writer.WriteStringValue(_dataProtector.Protect(bytes));
        }

        protected byte[] DecryptString(ref Utf8JsonReader reader)
        {
            // If it's encrypted, it should be a string
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var encrypted = reader.GetString();
            var unencrypted = _dataProtector.Unprotect(encrypted);
            return Convert.FromBase64String(unencrypted);
        }

        public abstract T FromBytes(byte[] bytes);

        public abstract byte[] ToBytes(T value);

        protected virtual byte[] ReadToBytes(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DecryptString(ref reader);
    }
}
