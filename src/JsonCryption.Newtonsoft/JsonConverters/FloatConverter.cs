using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class FloatConverter : EncryptedConverter<float>
    {
        private static readonly string _purpose = typeof(FloatConverter).FullName;
        
        public FloatConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new FloatByteConverter())
        {
        }
    }
}
