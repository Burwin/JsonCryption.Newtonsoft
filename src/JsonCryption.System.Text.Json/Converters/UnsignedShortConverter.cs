using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedShortConverter : EncryptedConverter<ushort>
    {
        private static readonly string _purpose = typeof(UnsignedShortConverter).FullName;

        public UnsignedShortConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<ushort>())
        {
        }
    }
}
