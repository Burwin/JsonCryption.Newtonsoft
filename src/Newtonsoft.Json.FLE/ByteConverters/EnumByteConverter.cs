using System;
using System.Text;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class EnumByteConverter<T> : IByteConverter<T>
        where T : struct, Enum
    {
        public T FromBytes(byte[] bytes) => (T)Enum.Parse(typeof(T), Encoding.UTF8.GetString(bytes));
        public byte[] ToBytes(T value) => Encoding.UTF8.GetBytes(Enum.GetName(typeof(T), value));
    }
}
