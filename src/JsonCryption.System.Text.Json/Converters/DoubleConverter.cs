using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DoubleConverter : EncryptedConverter<double>
    {
        private static readonly string _purpose = typeof(DoubleConverter).FullName;

        public DoubleConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<double>())
        {
        }
    }
}
