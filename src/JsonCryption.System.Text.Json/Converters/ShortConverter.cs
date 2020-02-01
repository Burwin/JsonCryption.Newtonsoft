using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ShortConverter : EncryptedConverter<short>
    {
        private static readonly string _purpose = typeof(ShortConverter).FullName;

        public ShortConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override short FromBytes(byte[] bytes) => BitConverter.ToInt16(bytes, 0);
        public override byte[] ToBytes(short value) => BitConverter.GetBytes(value);
    }
}
