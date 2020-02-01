using System;

namespace JsonCryption.ByteConverters
{
    public sealed class UIntByteConverter : IByteConverter<uint>
    {
        public uint FromBytes(byte[] bytes) => BitConverter.ToUInt32(bytes, 0);
        public byte[] ToBytes(uint value) => BitConverter.GetBytes(value);
    }
}
