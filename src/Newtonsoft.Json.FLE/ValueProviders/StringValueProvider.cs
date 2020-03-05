using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class StringValueProvider : EncryptedValueProvider<string>
    {
        public StringValueProvider(IEncryptedConverter<string> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider, string.Empty)
        {
        }
    }
}
