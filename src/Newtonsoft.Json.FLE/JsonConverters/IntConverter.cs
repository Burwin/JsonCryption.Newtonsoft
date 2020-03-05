using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class IntConverter : EncryptedConverter<int>
    {
        private static readonly string _purpose = typeof(IntConverter).FullName;
        public IntConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<int> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
