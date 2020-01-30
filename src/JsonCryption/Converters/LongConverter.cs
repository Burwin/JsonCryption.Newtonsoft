using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class LongConverter : EncryptedConverter<long>
    {
        private static readonly string _purpose = typeof(LongConverter).FullName;

        public LongConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);
        public override byte[] ToBytes(long value) => BitConverter.GetBytes(value);
    }
}
