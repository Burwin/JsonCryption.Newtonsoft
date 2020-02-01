using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class IntConverter : EncryptedConverter<int>
    {
        private static readonly string _purpose = typeof(IntConverter).FullName;
        public IntConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new IntByteConverter())
        {
        }
    }
}
