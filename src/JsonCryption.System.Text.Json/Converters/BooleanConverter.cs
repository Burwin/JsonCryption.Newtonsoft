using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class BooleanConverter : EncryptedConverter<bool>
    {
        private static readonly string _purpose = typeof(BooleanConverter).FullName;
        public BooleanConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<bool>())
        {
        }
    }
}
