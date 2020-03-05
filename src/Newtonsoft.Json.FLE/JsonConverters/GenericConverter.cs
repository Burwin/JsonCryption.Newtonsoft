using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class GenericConverter : JsonConverter, IEncryptedConverter
    {
        private readonly IDataProtector _dataProtector;

        public GenericConverter(IDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
        }

        public override bool CanConvert(Type objectType) => true;

        public object FromCipherText(string cipherText, JsonSerializer serializer = null)
        {
            var plaintextJson = _dataProtector.Unprotect(cipherText);

            using var textReader = new StringReader(plaintextJson);
            using var reader = new JsonTextReader(textReader);
            return serializer.Deserialize(reader);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => FromCipherText((string)reader.Value, serializer);

        public string ToCipherText(object value, JsonSerializer serializer)
        {
            var builder = new StringBuilder();
            using var textWriter = new StringWriter(builder);
            serializer.Serialize(textWriter, value);

            var plaintext = builder.ToString();
            return _dataProtector.Protect(plaintext);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, value);

            var plaintext = builder.ToString();
            var cipherText = _dataProtector.Protect(plaintext);
            writer.WriteValue(cipherText);
        }
    }
}
