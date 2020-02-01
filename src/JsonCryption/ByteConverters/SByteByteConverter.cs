using System;

namespace JsonCryption.ByteConverters
{
    public sealed class SByteByteConverter : IByteConverter<sbyte>
    {
        public sbyte FromBytes(byte[] bytes) => (sbyte)bytes[0];
        public byte[] ToBytes(sbyte value) => BitConverter.GetBytes(value);
    }
}
