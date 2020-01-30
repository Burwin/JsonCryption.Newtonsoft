using Microsoft.AspNetCore.DataProtection;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class StringConverter : EncryptedConverter<string>
    {
        private static readonly string _purpose = typeof(StringConverter).FullName;

        public StringConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override string FromBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public override byte[] ToBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
