using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal abstract class EncryptedConverterFactory : JsonConverterFactory
    {
        protected readonly IDataProtectionProvider _dataProtectionProvider;
        protected readonly JsonSerializerOptions _options;

        protected EncryptedConverterFactory(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _options = options;
        }
    }
}
