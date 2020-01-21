using System.Text.Json;

namespace JsonCryption
{
    public sealed class JsonCryptionOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }
        public IEncrypter Encrypter { get; }

        public JsonCryptionOptions SetDefaultOptions(JsonSerializerOptions options) => throw new System.NotImplementedException();

        public JsonCryptionOptions SetEncrypter(IEncrypter encrypter) => throw new System.NotImplementedException();
    }
}