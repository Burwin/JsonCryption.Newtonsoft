using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class EnumConverter<T> : EncryptedConverter<T>
        where T : struct, Enum
    {
        private static readonly string _purpose = typeof(EnumConverter<T>).FullName;

        public EnumConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new EnumByteConverter<T>())
        {
        }
    }
}
