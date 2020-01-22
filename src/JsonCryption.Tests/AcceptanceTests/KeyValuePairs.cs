using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class KeyValuePairs
    {
        [Fact]
        public void KeyValuePair_of_string_string_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void KeyValuePair_of_int_int_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void KeyValuePair_of_Guid_string_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Dictionary_of_string_string_works()
        {
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

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
