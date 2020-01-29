using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class EncryptedArrayConverter<T> : JsonConverter, IEncryptedConverter<T[]>
    {
        private readonly IDataProtector _dataProtector;

        public EncryptedArrayConverter(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(typeof(T).FullName);
        }
        
        public override bool CanConvert(Type objectType) => true;
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => FromCipherText((string)reader.Value, serializer);

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException($"CanWrite is false so this is never called");

        object IEncryptedConverter.FromCipherText(string cipherText, JsonSerializer serializer) => FromCipherText(cipherText, serializer);
        
        public T[] FromCipherText(string cipherText, JsonSerializer serializer)
        {
            var plaintextJson = _dataProtector.Unprotect(cipherText);

            using var textReader = new StringReader(plaintextJson);
            using var reader = new JsonTextReader(textReader);
            return serializer.Deserialize<T[]>(reader);
        }

        public string ToCipherText(object value, JsonSerializer serializer) => ToCipherText((T[])value, serializer);
        
        public string ToCipherText(T[] value, JsonSerializer serializer)
        {
            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, value);

            var unprotectedJson = builder.ToString();
            var protectedJson = _dataProtector.Protect(unprotectedJson);
            return protectedJson;
        }
    }
}
