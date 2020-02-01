using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class IntConverter : EncryptedConverter<int>
    {
        private static readonly string _purpose = typeof(IntConverter).FullName;

        public IntConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new IntByteConverter())
        {
        }
    }
}
