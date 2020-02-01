using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class IntConverter : EncryptedConverter<int>
    {
        private static readonly string _purpose = typeof(IntConverter).FullName;

        public IntConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override int FromBytes(byte[] bytes) => BitConverter.ToInt32(bytes, 0);
        public override byte[] ToBytes(int value) => BitConverter.GetBytes(value);
    }
}
