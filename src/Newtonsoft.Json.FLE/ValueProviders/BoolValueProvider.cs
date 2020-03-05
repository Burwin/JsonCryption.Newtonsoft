using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class BoolValueProvider : EncryptedValueProvider<bool>
    {
        public BoolValueProvider(IEncryptedConverter<bool> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
