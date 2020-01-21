using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DoubleConverter : EncryptedConverter<double>
    {
        public DoubleConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override double FromBytes(byte[] bytes) => BitConverter.ToDouble(bytes, 0);
        protected override byte[] ToBytes(double value) => BitConverter.GetBytes(value);
    }
}
