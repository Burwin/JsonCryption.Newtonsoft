using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Newtonsoft.Json.FLE.ValueProviders
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
