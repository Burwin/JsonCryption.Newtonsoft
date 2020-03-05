using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class FloatValueProvider : EncryptedValueProvider<float>
    {
        public FloatValueProvider(IEncryptedConverter<float> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
