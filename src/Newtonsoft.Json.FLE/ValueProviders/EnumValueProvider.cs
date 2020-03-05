using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
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
