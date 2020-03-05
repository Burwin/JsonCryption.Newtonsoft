using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class IntValueProvider : EncryptedValueProvider<int>
    {
        public IntValueProvider(IEncryptedConverter<int> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
