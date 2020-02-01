using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class SByteConverter : EncryptedConverter<sbyte>
    {
        private static readonly string _purpose = typeof(SByteConverter).FullName;
        public SByteConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new SByteByteConverter())
        {
        }
    }
}
