using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedIntConverter : EncryptedConverter<uint>
    {
        private static readonly string _purpose = typeof(UnsignedIntConverter).FullName;

        public UnsignedIntConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override uint FromBytes(byte[] bytes) => BitConverter.ToUInt32(bytes, 0);
        public override byte[] ToBytes(uint value) => BitConverter.GetBytes(value);
    }
}
