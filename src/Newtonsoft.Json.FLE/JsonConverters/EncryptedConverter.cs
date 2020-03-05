using Microsoft.AspNetCore.DataProtection;
using System;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal interface IEncryptedConverter
    {
        string ToCipherText(object value, JsonSerializer serializer = null);
        object FromCipherText(string cipherText, JsonSerializer serializer = null);
    }

    internal interface IEncryptedConverter<T> : IEncryptedConverter
    {
        string ToCipherText(T value, JsonSerializer serializer = null);
        new T FromCipherText(string cipherText, JsonSerializer serializer = null);
    }
    
    internal abstract class EncryptedConverter<T> : JsonConverter, IEncryptedConverter<T>
    {
        private readonly IDataProtector _dataProtector;
        private readonly IByteConverter<T> _byteConverter;

        public EncryptedConverter(IDataProtector dataProtector, IByteConverter<T> byteConverter)
        {
            _dataProtector = dataProtector;
            _byteConverter = byteConverter;
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => ReadJson(reader, objectType, default(T), false, serializer);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException($"CanWrite is false so this is never called");

        public override bool CanConvert(Type objectType) => true;

        public T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
            => FromCipherText((string)reader.Value, serializer);

        public void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
            => throw new NotImplementedException($"CanWrite is false so this is never called");

        public string ToCipherText(object value, JsonSerializer serializer = null) => ToCipherText((T)value);
        public string ToCipherText(T value, JsonSerializer serializer = null)
        {
            var unprotectedBytes = _byteConverter.ToBytes(value);
            var protectedBytes = _dataProtector.Protect(unprotectedBytes);
            var cipherText = Convert.ToBase64String(protectedBytes);
            return cipherText;
        }

        object IEncryptedConverter.FromCipherText(string cipherText, JsonSerializer serializer) => FromCipherText(cipherText, serializer);
        public T FromCipherText(string cipherText, JsonSerializer serializer = null)
        {
            var protectedBytes = Convert.FromBase64String(cipherText);
            var unprotectedBytes = _dataProtector.Unprotect(protectedBytes);
            return _byteConverter.FromBytes(unprotectedBytes);
        }
    }
}
