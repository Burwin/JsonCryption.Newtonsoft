using System;

namespace JsonCryption.ByteConverters
{
    internal sealed class ShortByteConverter : IByteConverter<short>
    {
        public short FromBytes(byte[] bytes) => BitConverter.ToInt16(bytes, 0);
        public byte[] ToBytes(short value) => BitConverter.GetBytes(value);
    }
}
