using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedShortConverter : EncryptedConverter<ushort>
    {
        private static readonly string _purpose = typeof(UnsignedShortConverter).FullName;

        public UnsignedShortConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override ushort FromBytes(byte[] bytes) => BitConverter.ToUInt16(bytes, 0);
        public override byte[] ToBytes(ushort value) => BitConverter.GetBytes(value);
    }
}
