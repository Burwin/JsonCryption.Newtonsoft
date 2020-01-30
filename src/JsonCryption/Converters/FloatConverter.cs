using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class FloatConverter : EncryptedConverter<float>
    {
        private static readonly string _purpose = typeof(FloatConverter).FullName;

        public FloatConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override float FromBytes(byte[] bytes) => BitConverter.ToSingle(bytes, 0);
        public override byte[] ToBytes(float value) => BitConverter.GetBytes(value);
    }
}
