using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class EncryptedDictionaryValueProvider<TKey, TValue> : EncryptedValueProvider<Dictionary<TKey, TValue>>
    {
        public EncryptedDictionaryValueProvider(
            IEncryptedConverter<Dictionary<TKey, TValue>> encryptedConverter,
            IValueProvider innerProvider,
            Dictionary<TKey, TValue> emptyValue,
            JsonSerializer serializer)
            : base(encryptedConverter, innerProvider, emptyValue, serializer)
        {
        }
    }
}
