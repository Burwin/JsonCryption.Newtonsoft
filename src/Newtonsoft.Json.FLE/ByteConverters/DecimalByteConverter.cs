using System;
using System.Linq;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class DecimalByteConverter : IByteConverter<decimal>
    {
        public decimal FromBytes(byte[] bytes)
        {
            var bits = new int[4]
                .Select((_, index) => BitConverter.ToInt32(bytes, index * 4))
                .ToArray();

            return new decimal(bits);
        }

        public byte[] ToBytes(decimal value)
        {
            return decimal
                .GetBits(value)
                .SelectMany(i => BitConverter.GetBytes(i))
                .ToArray();
        }
    }
}
