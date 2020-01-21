using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    internal class EncryptedJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => throw new NotImplementedException();
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
