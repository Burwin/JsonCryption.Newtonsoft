using System;

namespace JsonCryption.ByteConverters
{
    public sealed class UShortByteConverter : IByteConverter<ushort>
    {
        public ushort FromBytes(byte[] bytes) => BitConverter.ToUInt16(bytes, 0);
        public byte[] ToBytes(ushort value) => BitConverter.GetBytes(value);
    }
}
