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

        public override decimal FromBytes(byte[] bytes)
        {
            var bits = new int[4]
                .Select((_, index) => BitConverter.ToInt32(bytes, index * 4))
                .ToArray();

            return new decimal(bits);
        }

        public override byte[] ToBytes(decimal value)
            => decimal
                .GetBits(value)
                .SelectMany(i => BitConverter.GetBytes(i))
                .ToArray();
    }
}
