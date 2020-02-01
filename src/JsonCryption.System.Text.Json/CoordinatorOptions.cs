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

        public void Validate()
        {
            if (DataProtectionProvider is null)
                throw new NullReferenceException(nameof(DataProtectionProvider));
        }
    }
}