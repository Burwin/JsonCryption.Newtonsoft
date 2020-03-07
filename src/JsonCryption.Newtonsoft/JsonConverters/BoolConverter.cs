using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class BoolConverter : EncryptedConverter<bool>
    {
        private static readonly string _purpose = typeof(BoolConverter).FullName;
        
        public BoolConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<bool> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
