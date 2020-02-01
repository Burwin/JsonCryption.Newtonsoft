using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class GuidConverter : EncryptedConverter<Guid>
    {
        private static readonly string _purpose = typeof(GuidConverter).FullName;

        public GuidConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<Guid>())
        {
        }
    }
}
