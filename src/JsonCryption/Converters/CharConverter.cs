using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class CharConverter : EncryptedConverter<char>
    {
        private static readonly string _purpose = typeof(CharConverter).FullName;

        public CharConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override char FromBytes(byte[] bytes) => BitConverter.ToChar(bytes, 0);
        public override byte[] ToBytes(char value) => BitConverter.GetBytes(value);
    }
}
