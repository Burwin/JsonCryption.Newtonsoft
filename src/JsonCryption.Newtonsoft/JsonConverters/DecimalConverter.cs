using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class DecimalConverter : EncryptedConverter<decimal>
    {
        private static readonly string _purpose = typeof(DecimalConverter).FullName;
        public DecimalConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new DecimalByteConverter())
        {
        }
    }
}
