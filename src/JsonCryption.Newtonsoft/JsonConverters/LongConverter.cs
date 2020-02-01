using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class LongConverter : EncryptedConverter<long>
    {
        private static readonly string _purpose = typeof(LongConverter).FullName;
        public LongConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new LongByteConverter())
        {
        }
    }
}
