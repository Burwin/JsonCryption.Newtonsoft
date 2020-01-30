using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption
{
    public sealed class CoordinatorOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public IDataProtectionProvider DataProtectionProvider { get; set; }

        public static CoordinatorOptions Empty => new CoordinatorOptions();

        private CoordinatorOptions()
        {

        }

        private CoordinatorOptions(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
        {
            DataProtectionProvider = dataProtectionProvider;
            JsonSerializerOptions = options;
        }

        public static CoordinatorOptions CreateDefault(byte[] key)
        {
            //return new CoordinatorOptions(new AesManagedEncrypter(key), new JsonSerializerOptions());
            throw new NotImplementedException();
        }

        public void Validate()
        {
            if (DataProtectionProvider is null)
                throw new NullReferenceException(nameof(DataProtectionProvider));
        }
    }
}