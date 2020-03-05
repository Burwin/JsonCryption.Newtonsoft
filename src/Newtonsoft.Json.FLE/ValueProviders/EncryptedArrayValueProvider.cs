using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class EncryptedArrayValueProvider<T> : EncryptedValueProvider<T[]>
    {
        public EncryptedArrayValueProvider(IEncryptedConverter<T[]> encryptedConverter, IValueProvider innerProvider, T[] emptyValue, JsonSerializer serializer)
            : base(encryptedConverter, innerProvider, emptyValue, serializer)
        {
        }
    }
}
