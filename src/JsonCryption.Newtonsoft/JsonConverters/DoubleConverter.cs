using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class DoubleConverter : EncryptedConverter<double>
    {
        private static readonly string _purpose = typeof(DoubleConverter).FullName;
        public DoubleConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new DoubleByteConverter())
        {
        }
    }
}
