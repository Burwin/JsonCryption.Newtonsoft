using System;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class FloatByteConverter : IByteConverter<float>
    {
        public float FromBytes(byte[] bytes) => BitConverter.ToSingle(bytes, 0);
        public byte[] ToBytes(float value) => BitConverter.GetBytes(value);
    }
}
