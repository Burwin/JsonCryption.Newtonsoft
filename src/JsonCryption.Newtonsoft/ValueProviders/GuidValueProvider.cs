using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class GuidValueProvider : EncryptedValueProvider<Guid>
    {
        public GuidValueProvider(IEncryptedConverter<Guid> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
