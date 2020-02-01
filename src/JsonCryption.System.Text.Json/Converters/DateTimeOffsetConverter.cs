using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        private static readonly string _purpose = typeof(DateTimeOffsetConverter).FullName;

        public DateTimeOffsetConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<DateTimeOffset>())
        {
        }
    }
}
