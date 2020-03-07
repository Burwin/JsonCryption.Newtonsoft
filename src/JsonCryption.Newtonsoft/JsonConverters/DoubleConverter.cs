using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class DoubleConverter : EncryptedConverter<double>
    {
        private static readonly string _purpose = typeof(DoubleConverter).FullName;
        public DoubleConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<double> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
