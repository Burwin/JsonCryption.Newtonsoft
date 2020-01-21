using JsonCryption.Encrypters;
using System;
using System.Linq;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        public DateTimeOffsetConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override DateTimeOffset FromBytes(byte[] bytes)
        {
            var dateTimeTicks = BitConverter.ToInt64(bytes, 0);
            var timeSpanTicks = BitConverter.ToInt64(bytes, 8);

            var offset = new TimeSpan(timeSpanTicks);
            return new DateTimeOffset(dateTimeTicks, offset);
        }

        protected override byte[] ToBytes(DateTimeOffset value)
            => BitConverter
                .GetBytes(value.Ticks)
                .Concat(BitConverter.GetBytes(value.Offset.Ticks))
                .ToArray();
    }
}
