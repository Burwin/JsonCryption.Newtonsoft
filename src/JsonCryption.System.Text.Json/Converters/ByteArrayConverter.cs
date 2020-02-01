using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ByteArrayConverter : EncryptedConverter<byte[]>
    {
        private static readonly string _purpose = typeof(ByteArrayConverter).FullName;
        public ByteArrayConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override byte[] FromBytes(byte[] bytes) => bytes;
        public override byte[] ToBytes(byte[] value) => value;
    }
}
