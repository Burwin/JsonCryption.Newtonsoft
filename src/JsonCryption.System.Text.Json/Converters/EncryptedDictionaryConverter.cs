using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal class EncryptedDictionaryConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    {
        private readonly IDataProtector _dataProtector;
        private readonly JsonSerializerOptions _options;
        private readonly EncryptedArrayConverter<KeyValuePair<TKey, TValue>> _arrayConverter;

        public EncryptedDictionaryConverter(IDataProtector dataProtector, JsonSerializerOptions options,
            EncryptedArrayConverter<KeyValuePair<TKey, TValue>> arrayConverter)
        {
            _dataProtector = dataProtector;
            _options = options;
            _arrayConverter = arrayConverter;
        }

        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var kvpArray = _arrayConverter.Read(ref reader, typeToConvert, options ?? _options);
            return kvpArray.ToDictionary(x => x.Key, x => x.Value);
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
            => _arrayConverter.Write(writer, value.ToArray(), options ?? _options);
    }
}
