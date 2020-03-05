using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class UShortConverter : EncryptedConverter<ushort>
    {
        private static readonly string _purpose = typeof(UShortConverter).FullName;
        public UShortConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<ushort> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
