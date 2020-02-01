using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class BooleanConverter : EncryptedConverter<bool>
    {
        private static readonly string _purpose = typeof(BooleanConverter).FullName;
        public BooleanConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public override byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
