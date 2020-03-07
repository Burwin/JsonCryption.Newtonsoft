using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class ByteArrayValueProvider : EncryptedValueProvider<byte[]>
    {
        public ByteArrayValueProvider(IEncryptedConverter<byte[]> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider, Array.Empty<byte>())
        {
        }
    }
}
