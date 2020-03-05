using System;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class DoubleByteConverter : IByteConverter<double>
    {
        public double FromBytes(byte[] bytes) => BitConverter.ToDouble(bytes, 0);
        public byte[] ToBytes(double value) => BitConverter.GetBytes(value);
    }
}
