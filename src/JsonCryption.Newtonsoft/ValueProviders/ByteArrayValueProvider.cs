using Newtonsoft.Json.FLE.JsonConverters;
using Newtonsoft.Json.Serialization;
using System;

namespace Newtonsoft.Json.FLE.ValueProviders
{
    internal sealed class ByteArrayValueProvider : EncryptedValueProvider<byte[]>
    {
        public ByteArrayValueProvider(IEncryptedConverter<byte[]> encryptedConverter, IValueProvider innerProvider)
            : base(encryptedConverter, innerProvider, Array.Empty<byte>())
        {
        }
    }
}
