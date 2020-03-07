using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class FloatConverter : EncryptedConverter<float>
    {
        private static readonly string _purpose = typeof(FloatConverter).FullName;
        
        public FloatConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<float> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
