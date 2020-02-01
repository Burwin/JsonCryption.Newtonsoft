using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeConverter : EncryptedConverter<DateTime>
    {
        private static readonly string _purpose = typeof(DateTimeConverter).FullName;

        public DateTimeConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<DateTime>())
        {
        }
    }
}
