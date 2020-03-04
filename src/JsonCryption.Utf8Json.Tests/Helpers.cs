using Microsoft.AspNetCore.DataProtection;
using Utf8Json;
using Utf8Json.Resolvers;

namespace JsonCryption.Utf8Json.Tests
{
    class Helpers
    {
        public static string SerializedValueOf<T>(T value, IJsonFormatterResolver resolver, string name)
        {
            var serialized = JsonSerializer.ToJsonString<T>(value, resolver);
            return $"\"{name}\":{serialized}";
        }

        public static void SetJsonSerializerResolver(IJsonFormatterResolver fallbackResolver = null)
            => JsonSerializer.SetDefaultResolver(
                new EncryptedResolver(fallbackResolver ?? StandardResolver.AllowPrivate,
                DataProtectionProvider.Create("test").CreateProtector("test")));
    }
}
