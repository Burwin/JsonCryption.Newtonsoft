using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class BoolConverter : EncryptedConverter<bool>
    {
        private static readonly string _purpose = typeof(BoolConverter).FullName;
        
        public BoolConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new BoolByteConverter())
        {
        }
    }
}
