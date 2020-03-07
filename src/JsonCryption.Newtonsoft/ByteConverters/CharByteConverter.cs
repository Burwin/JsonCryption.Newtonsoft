using System;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class CharByteConverter : IByteConverter<char>
    {
        public char FromBytes(byte[] bytes) => BitConverter.ToChar(bytes, 0);
        public byte[] ToBytes(char value) => BitConverter.GetBytes(value);
    }
}
