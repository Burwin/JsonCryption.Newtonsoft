using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class DateTimeValueProvider : EncryptedValueProvider<DateTime>
    {
        public DateTimeValueProvider(IEncryptedConverter<DateTime> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
