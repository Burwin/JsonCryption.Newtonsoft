using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class CharConverter : EncryptedConverter<char>
    {
        private static readonly string _purpose = typeof(CharConverter).FullName;

        public CharConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new CharByteConverter())
        {
        }
    }
}
