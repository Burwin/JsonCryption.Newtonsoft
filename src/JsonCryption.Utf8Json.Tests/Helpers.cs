using Utf8Json;

namespace JsonCryption.Utf8Json.Tests
{
    class Helpers
    {
        public static string SerializedValueOf<T>(T value, IJsonFormatterResolver resolver, string name)
        {
            var serialized = JsonSerializer.ToJsonString<T>(value, resolver);
            return $"\"{name}\":{serialized}";
        }
    }
}
