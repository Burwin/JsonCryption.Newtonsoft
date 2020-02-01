using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class EnumConverter<T> : EncryptedConverter<T>
        where T : struct, Enum
    {
        private static readonly string _purpose = typeof(EnumConverter<T>).FullName;

        public EnumConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override T FromBytes(byte[] bytes) => (T)Enum.Parse(typeof(T), Encoding.UTF8.GetString(bytes));
        public override byte[] ToBytes(T value) => Encoding.UTF8.GetBytes(Enum.GetName(typeof(T), value));
    }
}
