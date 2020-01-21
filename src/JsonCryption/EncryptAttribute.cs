using System.Text.Json.Serialization;

namespace JsonCryption
{
    public sealed class EncryptAttribute : JsonConverterAttribute
    {
        public EncryptAttribute() : base(typeof(EncryptedJsonConverterFactory))
        {
        }
    }
}
