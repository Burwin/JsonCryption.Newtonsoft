using Microsoft.AspNetCore.DataProtection;
using System;
using System.Linq;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        private static readonly string _purpose = typeof(DateTimeOffsetConverter).FullName;

        public DateTimeOffsetConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override DateTimeOffset FromBytes(byte[] bytes)
        {
            var dateTimeTicks = BitConverter.ToInt64(bytes, 0);
            var timeSpanTicks = BitConverter.ToInt64(bytes, 8);

            var offset = new TimeSpan(timeSpanTicks);
            return new DateTimeOffset(dateTimeTicks, offset);
        }

        public override byte[] ToBytes(DateTimeOffset value)
            => BitConverter
                .GetBytes(value.Ticks)
                .Concat(BitConverter.GetBytes(value.Offset.Ticks))
                .ToArray();
    }
}
