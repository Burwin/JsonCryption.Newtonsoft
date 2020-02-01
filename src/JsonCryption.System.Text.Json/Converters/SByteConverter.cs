using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class SByteConverter : EncryptedConverter<sbyte>
    {
        private static readonly string _purpose = typeof(SByteConverter).FullName;

        public SByteConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<sbyte>())
        {
        }
    }
}
