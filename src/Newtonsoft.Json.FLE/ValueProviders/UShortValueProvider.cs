using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class UShortValueProvider : EncryptedValueProvider<ushort>
    {
        public UShortValueProvider(IEncryptedConverter<ushort> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
