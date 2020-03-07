using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class EncryptedDictionaryConverter<TKey, TValue> : JsonConverter, IEncryptedConverter<Dictionary<TKey, TValue>>
    {
        private readonly IDataProtector _dataProtector;

        public EncryptedDictionaryConverter(IDataProtectionProvider dataProtectionProvider)
        {
            var purpose = $"{typeof(TKey).FullName}-{typeof(TValue).FullName}";
            _dataProtector = dataProtectionProvider.CreateProtector(purpose);
        }

        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => FromCipherText((string)reader.Value, serializer);

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException($"CanWrite is false so this is never called");

        object IEncryptedConverter.FromCipherText(string cipherText, JsonSerializer serializer) => FromCipherText(cipherText, serializer);

        public Dictionary<TKey, TValue> FromCipherText(string cipherText, JsonSerializer serializer = null)
        {
            var plaintextJson = _dataProtector.Unprotect(cipherText);

            using var textReader = new StringReader(plaintextJson);
            using var reader = new JsonTextReader(textReader);
            return serializer.Deserialize<Dictionary<TKey, TValue>>(reader);
        }

        public string ToCipherText(object value, JsonSerializer serializer = null) => ToCipherText((Dictionary<TKey, TValue>)value, serializer);
        
        public string ToCipherText(Dictionary<TKey, TValue> value, JsonSerializer serializer = null)
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
