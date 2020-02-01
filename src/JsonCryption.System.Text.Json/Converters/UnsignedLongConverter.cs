using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedLongConverter : EncryptedConverter<ulong>
    {
        private static readonly string _purpose = typeof(UnsignedLongConverter).FullName;

        public UnsignedLongConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new ULongByteConverter())
        {
        }
    }
}
