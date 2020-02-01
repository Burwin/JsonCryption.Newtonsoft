using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace JsonCryption.System.Text.Json.Tests.AcceptanceTests
{
    public class KeyValuePairs
    {
        [Fact]
        public void KeyValuePair_of_string_string_works()
        {
            Coordinator.ConfigureDefault("test");

            var myKvp = new KeyValuePair<string, string>("foo", "bar");
            var foo = new FooKvpStringString { MyKvps = myKvp };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooKvpStringString.MyKvps));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyKvps));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooKvpStringString>(json);
            decrypted.MyKvps.ShouldBe(myKvp);
        }

        private class FooKvpStringString
        {
            [Encrypt]
            public KeyValuePair<string, string> MyKvps { get; set; }
        }

        [Fact]
        public void KeyValuePair_of_int_int_works()
        {
            Coordinator.ConfigureDefault("test");

            var myKvp = new KeyValuePair<int, int>(int.MinValue, int.MaxValue);
            var foo = new FooKvpIntInt { MyKvps = myKvp };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooKvpIntInt.MyKvps));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyKvps));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooKvpIntInt>(json);
            decrypted.MyKvps.ShouldBe(myKvp);
        }

        private class FooKvpIntInt
        {
            [Encrypt]
            public KeyValuePair<int, int> MyKvps { get; set; }
        }

        [Fact]
        public void KeyValuePair_of_Guid_string_works()
        {
            Coordinator.ConfigureDefault("test");

            var myKvp = new KeyValuePair<Guid, string>(Guid.NewGuid(), "foo");
            var foo = new FooKvpGuidString { MyKvps = myKvp };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooKvpGuidString.MyKvps));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyKvps));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooKvpGuidString>(json);
            decrypted.MyKvps.ShouldBe(myKvp);
        }

        private class FooKvpGuidString
        {
            [Encrypt]
            public KeyValuePair<Guid, string> MyKvps { get; set; }
        }

        [Fact]
        public void Dictionary_of_string_string_works()
        {
            Coordinator.ConfigureDefault("test");

            var myStrings = new Dictionary<string, string>
            {
                {"first_key", "first_value" },
                {"another key", "another value" },
            };

            var foo = new FooStringDictionary { MyStrings = myStrings };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooStringDictionary.MyStrings));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.Array);

                var jsonProperties = jsonProperty.EnumerateArray().ToList();
                jsonProperties.ForEach(el => el.ValueKind.ShouldBe(JsonValueKind.String));

                // shouldn't match any keys or values
                jsonProperties.ForEach(el => myStrings.Keys.ToList().ForEach(key => key.ShouldNotBe(el.GetString())));
                jsonProperties.ForEach(el => myStrings.Values.ToList().ForEach(key => key.ShouldNotBe(el.GetString())));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooStringDictionary>(json);
            decrypted.MyStrings.ShouldBe(myStrings);
        }

        private class FooStringDictionary
        {
            [Encrypt]
            public Dictionary<string, string> MyStrings { get; set; }
        }
    }
}
