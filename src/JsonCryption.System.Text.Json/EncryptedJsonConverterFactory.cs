using JsonCryption.System.Text.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    internal class EncryptedJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => Coordinator.Singleton.HasConverter(typeToConvert);
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => Coordinator.Singleton.GetConverter(typeToConvert, options);
    }
}
