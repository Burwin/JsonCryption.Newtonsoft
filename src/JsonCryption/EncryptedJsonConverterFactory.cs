using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    internal class EncryptedJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => JsonCryption.Singleton.HasConverter(typeToConvert);
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => JsonCryption.Singleton.GetConverter(typeToConvert, options);
    }
}
