using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ULongConverter : EncryptedConverter<ulong>
    {
        private static readonly string _purpose = typeof(ULongConverter).FullName;
        public ULongConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new ULongByteConverter())
        {
        }
    }
}
