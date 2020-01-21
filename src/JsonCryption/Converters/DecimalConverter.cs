using JsonCryption.Encrypters;
using System;
using System.Linq;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DecimalConverter : EncryptedConverter<decimal>
    {
        public DecimalConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) => throw new NotImplementedException();
        protected override decimal FromBytes(byte[] bytes)
        {
            var bits = new int[4]
                .Select((_, index) => BitConverter.ToInt32(bytes, index * 4))
                .ToArray();

            return new decimal(bits);
        }

        protected override byte[] ToBytes(decimal value)
            => decimal
                .GetBits(value)
                .SelectMany(i => BitConverter.GetBytes(i))
                .ToArray();
    }
}
