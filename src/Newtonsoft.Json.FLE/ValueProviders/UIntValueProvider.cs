using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class UIntValueProvider : EncryptedValueProvider<uint>
    {
        public UIntValueProvider(IEncryptedConverter<uint> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
