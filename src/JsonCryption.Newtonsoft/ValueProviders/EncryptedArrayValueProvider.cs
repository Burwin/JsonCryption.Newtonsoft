using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class EncryptedArrayValueProvider<T> : EncryptedValueProvider<T[]>
    {
        public EncryptedArrayValueProvider(IEncryptedConverter<T[]> encryptedConverter, IValueProvider innerProvider, T[] emptyValue, JsonSerializer serializer)
            : base(encryptedConverter, innerProvider, emptyValue, serializer)
        {
        }
    }
}
