using System;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class IntByteConverter : IByteConverter<int>
    {
        public int FromBytes(byte[] bytes) => BitConverter.ToInt32(bytes, 0);
        public byte[] ToBytes(int value) => BitConverter.GetBytes(value);
    }
}
