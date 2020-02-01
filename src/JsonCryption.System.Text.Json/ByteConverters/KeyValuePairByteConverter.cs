using JsonCryption.ByteConverters;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.System.Text.Json.ByteConverters
{
    internal sealed class KeyValuePairByteConverter<TKey, TValue> : IByteConverter<KeyValuePair<TKey, TValue>>
    {
        private readonly JsonSerializerOptions _options;

        public KeyValuePairByteConverter(JsonSerializerOptions options)
        {
            _options = options;
        }
        
        public KeyValuePair<TKey, TValue> FromBytes(byte[] bytes)
        {
            var unencryptedJson = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<KeyValuePair<TKey, TValue>>(unencryptedJson, _options);
        }

        public byte[] ToBytes(KeyValuePair<TKey, TValue> value)
        {
            var unencryptedJson = JsonSerializer.Serialize(value, _options);
            return Encoding.UTF8.GetBytes(unencryptedJson);
        }
    }
}
