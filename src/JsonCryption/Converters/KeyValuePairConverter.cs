using JsonCryption.Encrypters;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class KeyValuePairConverter<TKey, TValue> : EncryptedConverter<KeyValuePair<TKey, TValue>>
    {
        public KeyValuePairConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override KeyValuePair<TKey, TValue> FromBytes(byte[] bytes)
        {
            var unencryptedJson = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<KeyValuePair<TKey, TValue>>(unencryptedJson, _options);
        }
        
        public override byte[] ToBytes(KeyValuePair<TKey, TValue> value)
        {
            var unencryptedJson = JsonSerializer.Serialize(value, _options);
            return Encoding.UTF8.GetBytes(unencryptedJson);
        }
    }
}
