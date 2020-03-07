using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class SByteValueProvider : EncryptedValueProvider<sbyte>
    {
        public SByteValueProvider(IEncryptedConverter<sbyte> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
