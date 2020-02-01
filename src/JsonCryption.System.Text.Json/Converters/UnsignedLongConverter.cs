using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedLongConverter : EncryptedConverter<ulong>
    {
        private static readonly string _purpose = typeof(UnsignedLongConverter).FullName;

        public UnsignedLongConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override ulong FromBytes(byte[] bytes) => BitConverter.ToUInt64(bytes, 0);
        public override byte[] ToBytes(ulong value) => BitConverter.GetBytes(value);
    }
}
