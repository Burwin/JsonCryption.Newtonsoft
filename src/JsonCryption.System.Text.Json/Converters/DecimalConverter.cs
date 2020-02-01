using Microsoft.AspNetCore.DataProtection;
using System;
using System.Linq;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DecimalConverter : EncryptedConverter<decimal>
    {
        private static readonly string _purpose = typeof(DecimalConverter).FullName;

        public DecimalConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
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
