using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ShortConverter : EncryptedConverter<short>
    {
        private static readonly string _purpose = typeof(ShortConverter).FullName;
        public ShortConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<short> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
