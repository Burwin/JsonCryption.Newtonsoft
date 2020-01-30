using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption
{
    public sealed class CoordinatorOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public Func<IDataProtectionProvider> DataProtectionProviderFactory { get; set; }

        public static CoordinatorOptions Empty => new CoordinatorOptions();

        private CoordinatorOptions()
        {

        }

        private CoordinatorOptions(Func<IDataProtectionProvider> dataProtectionProviderFactory, JsonSerializerOptions options)
        {
            DataProtectionProviderFactory = dataProtectionProviderFactory;
            JsonSerializerOptions = options;
        }

        public static CoordinatorOptions CreateDefault(byte[] key)
        {
            //return new CoordinatorOptions(new AesManagedEncrypter(key), new JsonSerializerOptions());
            throw new NotImplementedException();
        }

        public void Validate()
        {
            if (DataProtectionProviderFactory is null)
                throw new NullReferenceException(nameof(DataProtectionProviderFactory));
        }
    }
}