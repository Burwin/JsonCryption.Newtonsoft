using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class FloatConverter : EncryptedConverter<float>
    {
        private static readonly string _purpose = typeof(FloatConverter).FullName;

        public FloatConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, new FloatByteConverter())
        {
        }
    }
}
