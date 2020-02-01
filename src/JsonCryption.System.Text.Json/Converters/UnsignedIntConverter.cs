using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedIntConverter : EncryptedConverter<uint>
    {
        private static readonly string _purpose = typeof(UnsignedIntConverter).FullName;

        public UnsignedIntConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new UIntByteConverter())
        {
        }
    }
}
