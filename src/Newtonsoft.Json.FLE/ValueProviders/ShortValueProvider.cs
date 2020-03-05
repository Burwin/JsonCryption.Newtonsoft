using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class ShortValueProvider : EncryptedValueProvider<short>
    {
        public ShortValueProvider(IEncryptedConverter<short> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
