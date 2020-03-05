using System;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class ULongByteConverter : IByteConverter<ulong>
    {
        public ulong FromBytes(byte[] bytes) => BitConverter.ToUInt64(bytes, 0);
        public byte[] ToBytes(ulong value) => BitConverter.GetBytes(value);
    }
}
