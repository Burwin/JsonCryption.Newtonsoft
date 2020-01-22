using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class FloatConverter : EncryptedConverter<float>
    {
        public FloatConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override float FromBytes(byte[] bytes) => BitConverter.ToSingle(bytes, 0);
        public override byte[] ToBytes(float value) => BitConverter.GetBytes(value);
    }
}
