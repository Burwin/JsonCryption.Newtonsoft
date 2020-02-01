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
        private readonly IByteConverter<T> _byteConverter;

        protected EncryptedConverter(IDataProtector dataProtector, JsonSerializerOptions options, IByteConverter<T> byteConverter)
        {
            _dataProtector = dataProtector;
            _options = options;
            _byteConverter = byteConverter;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var bytes = ReadToBytes(ref reader, typeToConvert, options);
            return _byteConverter.FromBytes(bytes);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var unprotectedBytes = _byteConverter.ToBytes(value);
            var protectedBytes = _dataProtector.Protect(unprotectedBytes);
            var cipherText = Convert.ToBase64String(protectedBytes);
            writer.WriteStringValue(cipherText);
        }

        protected byte[] DecryptString(ref Utf8JsonReader reader)
        {
            // If it's encrypted, it should be a string
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var cipherText = reader.GetString();
            var protectedBytes = Convert.FromBase64String(cipherText);
            var unprotectedBytes = _dataProtector.Unprotect(protectedBytes);
            return unprotectedBytes;
        }

        protected virtual byte[] ReadToBytes(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DecryptString(ref reader);
    }
}
