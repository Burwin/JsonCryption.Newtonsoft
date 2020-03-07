using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class EnumValueProvider<T> : EncryptedValueProvider<T>
        where T : struct, Enum
    {
        public EnumValueProvider(IEncryptedConverter<T> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider)
        {
        }
    }
}
