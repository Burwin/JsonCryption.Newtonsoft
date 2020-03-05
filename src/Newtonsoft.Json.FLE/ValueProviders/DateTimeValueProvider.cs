using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class DateTimeValueProvider : EncryptedValueProvider<DateTime>
    {
        public DateTimeValueProvider(IEncryptedConverter<DateTime> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
