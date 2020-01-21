using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption
{
    public sealed class CoordinatorOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public Encrypter Encrypter { get; set; }

        public static CoordinatorOptions Empty => new CoordinatorOptions();

        private CoordinatorOptions()
        {

        }

        private CoordinatorOptions(Encrypter encrypter, JsonSerializerOptions options)
        {
            Encrypter = encrypter;
            JsonSerializerOptions = options;
        }

        public static CoordinatorOptions CreateDefault(byte[] key)
            => new CoordinatorOptions(new AesManagedEncrypter(key), new JsonSerializerOptions());

        public void Validate()
        {
            if (Encrypter is null)
                throw new NullReferenceException(nameof(Encrypter));
        }
    }
}