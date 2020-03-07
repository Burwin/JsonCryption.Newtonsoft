using System;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class BoolByteConverter : IByteConverter<bool>
    {
        public bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
