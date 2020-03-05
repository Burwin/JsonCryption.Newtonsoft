using System;
using System.Linq;

namespace JsonCryption.ByteConverters
{
    internal sealed class DateTimeOffsetByteConverter : IByteConverter<DateTimeOffset>
    {
        public DateTimeOffset FromBytes(byte[] bytes)
        {
            var dateTimeTicks = BitConverter.ToInt64(bytes, 0);
            var timeSpanTicks = BitConverter.ToInt64(bytes, 8);

            var offset = new TimeSpan(timeSpanTicks);
            return new DateTimeOffset(dateTimeTicks, offset);
        }

        public byte[] ToBytes(DateTimeOffset value)
        {
            return BitConverter
                .GetBytes(value.Ticks)
                .Concat(BitConverter.GetBytes(value.Offset.Ticks))
                .ToArray();
        }
    }
}
