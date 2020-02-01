using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DoubleConverter : EncryptedConverter<double>
    {
        private static readonly string _purpose = typeof(DoubleConverter).FullName;

        public DoubleConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override double FromBytes(byte[] bytes) => BitConverter.ToDouble(bytes, 0);
        public override byte[] ToBytes(double value) => BitConverter.GetBytes(value);
    }
}
