using System;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    /// <summary>
    /// Decorate public properties to encrypt/decrypt when serializing/deserializing
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class EncryptAttribute : JsonConverterAttribute
    {
        /// <summary>
        /// Create a new <see cref="EncryptAttribute"/>
        /// </summary>
        public EncryptAttribute() : base(typeof(EncryptedJsonConverterFactory))
        {
        }
    }
}
