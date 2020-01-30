using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class GuidConverter : EncryptedConverter<Guid>
    {
        private static readonly string _purpose = typeof(GuidConverter).FullName;

        public GuidConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override Guid FromBytes(byte[] bytes) => new Guid(bytes);
        public override byte[] ToBytes(Guid value) => value.ToByteArray();
    }
}
