using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class CharConverter : EncryptedConverter<char>
    {
        private static readonly string _purpose = typeof(CharConverter).FullName;

        public CharConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<char> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
