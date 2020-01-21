using JsonCryption.Encrypters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal abstract class EncryptedConverterFactory : JsonConverterFactory
    {
        protected readonly Encrypter _encrypter;
        protected readonly JsonSerializerOptions _options;

        protected EncryptedConverterFactory(Encrypter encrypter, JsonSerializerOptions options)
        {
            _encrypter = encrypter;
            _options = options;
        }
    }
}
