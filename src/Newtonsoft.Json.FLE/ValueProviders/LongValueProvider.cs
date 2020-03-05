using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class LongValueProvider : EncryptedValueProvider<long>
    {
        public LongValueProvider(IEncryptedConverter<long> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
