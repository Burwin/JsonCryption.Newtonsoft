using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ByteConverter : EncryptedConverter<byte>
    {
        private static readonly string _purpose = typeof(BoolConverter).FullName;

        public ByteConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new ByteByteConverter())
        {
        }
    }
}
