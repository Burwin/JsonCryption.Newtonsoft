using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class UShortConverter : EncryptedConverter<ushort>
    {
        private static readonly string _purpose = typeof(UShortConverter).FullName;
        public UShortConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new UShortByteConverter())
        {
        }
    }
}
