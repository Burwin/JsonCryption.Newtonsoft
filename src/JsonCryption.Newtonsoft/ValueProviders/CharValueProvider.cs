using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class CharValueProvider : EncryptedValueProvider<char>
    {
        public CharValueProvider(IEncryptedConverter<char> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
