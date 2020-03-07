using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class DateTimeOffsetValueProvider : EncryptedValueProvider<DateTimeOffset>
    {
        public DateTimeOffsetValueProvider(IEncryptedConverter<DateTimeOffset> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
