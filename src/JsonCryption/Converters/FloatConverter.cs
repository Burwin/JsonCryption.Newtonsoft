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

        protected override float FromBytes(byte[] bytes) => BitConverter.ToSingle(bytes, 0);
        protected override byte[] ToBytes(float value) => BitConverter.GetBytes(value);
    }
}
