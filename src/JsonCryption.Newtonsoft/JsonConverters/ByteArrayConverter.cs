using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ByteArrayConverter : EncryptedConverter<byte[]>
    {
        private static readonly string _purpose = typeof(ByteArrayConverter).FullName;

        public ByteArrayConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new ByteArrayByteConverter())
        {
        }
    }
}
