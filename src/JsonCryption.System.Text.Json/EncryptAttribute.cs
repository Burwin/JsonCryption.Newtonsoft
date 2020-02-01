using System;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class EncryptAttribute : JsonConverterAttribute
    {
        public EncryptAttribute() : base(typeof(EncryptedJsonConverterFactory))
        {
        }
    }
}
