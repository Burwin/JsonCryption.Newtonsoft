using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class StringValueProvider : EncryptedValueProvider<string>
    {
        public StringValueProvider(IEncryptedConverter<string> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider, string.Empty)
        {
        }
    }
}
