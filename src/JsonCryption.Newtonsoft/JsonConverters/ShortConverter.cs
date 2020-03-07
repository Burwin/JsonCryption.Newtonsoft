using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
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
