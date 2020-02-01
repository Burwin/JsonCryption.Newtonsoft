using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class StringConverter : EncryptedConverter<string>
    {
        private static readonly string _purpose = typeof(StringConverter).FullName;
        public StringConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<string> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
