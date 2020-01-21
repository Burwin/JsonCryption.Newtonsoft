using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    public abstract class EncryptedConverter<T> : JsonConverter<T>
    {
        protected readonly IEncrypter _encrypter;
        protected readonly JsonSerializerOptions _options;

        protected EncryptedConverter(IEncrypter encrypter, JsonSerializerOptions options)
        {
            _encrypter = encrypter;
            _options = options;
        }
    }
}
