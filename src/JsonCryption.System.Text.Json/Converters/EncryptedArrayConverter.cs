using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal sealed class EncryptedArrayConverter<T> : JsonConverter<T[]>
    {
        private readonly IDataProtector _dataProtector;

        public EncryptedArrayConverter(IDataProtector dataProtector, JsonSerializerOptions options, IByteConverter<T> byteConverter)
        {
            _dataProtector = dataProtector;
        }

        public override T[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var cipherText = reader.GetString();
            var plainText = _dataProtector.Unprotect(cipherText);
            var deserialized = JsonSerializer.Deserialize<T[]>(plainText);
            return deserialized;
        }

        public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
        {
            var plainText = JsonSerializer.Serialize(value, options);
            var cipherText = _dataProtector.Protect(plainText);
            writer.WriteStringValue(cipherText);
        }
    }
}
