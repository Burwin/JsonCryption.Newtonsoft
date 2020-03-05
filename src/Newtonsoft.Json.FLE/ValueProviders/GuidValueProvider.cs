using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class GuidValueProvider : EncryptedValueProvider<Guid>
    {
        public GuidValueProvider(IEncryptedConverter<Guid> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
