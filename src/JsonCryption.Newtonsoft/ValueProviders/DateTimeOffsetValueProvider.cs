using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class DateTimeOffsetValueProvider : EncryptedValueProvider<DateTimeOffset>
    {
        public DateTimeOffsetValueProvider(IEncryptedConverter<DateTimeOffset> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
