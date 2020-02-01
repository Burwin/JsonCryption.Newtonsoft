using System;

namespace JsonCryption.ByteConverters
{
    public sealed class CharByteConverter : IByteConverter<char>
    {
        public char FromBytes(byte[] bytes) => BitConverter.ToChar(bytes, 0);
        public byte[] ToBytes(char value) => BitConverter.GetBytes(value);
    }
}
