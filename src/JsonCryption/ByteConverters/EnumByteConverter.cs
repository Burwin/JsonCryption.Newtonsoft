using System;
using System.Text;

namespace JsonCryption.ByteConverters
{
    public sealed class EnumByteConverter<T> : IByteConverter<T>
        where T : struct, Enum
    {
        public T FromBytes(byte[] bytes) => (T)Enum.Parse(typeof(T), Encoding.UTF8.GetString(bytes));
        public byte[] ToBytes(T value) => Encoding.UTF8.GetBytes(Enum.GetName(typeof(T), value));
    }
}
