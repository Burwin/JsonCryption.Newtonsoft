using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ByteConverter : EncryptedConverter<byte>
    {
        private static readonly string _purpose = typeof(ByteConverter).FullName;

        public ByteConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override byte FromBytes(byte[] bytes) => bytes[0];
        public override byte[] ToBytes(byte value) => new[] { value };
    }
}
