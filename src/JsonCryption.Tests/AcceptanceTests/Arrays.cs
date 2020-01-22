using Shouldly;
using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class Arrays
    {
        [Fact]
        public void String_array_works()
        {
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

            var myStrings = new[] { "some", "strings", "to test" };
            var foo = new FooStringArray { MyStrings = myStrings };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooStringArray.MyStrings));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.Array);

                var jsonProperties = jsonProperty.EnumerateArray().ToList();
                jsonProperties.ForEach(el => el.ValueKind.ShouldBe(JsonValueKind.String));

                for (int i = 0; i < jsonProperties.Count; i++)
                {
                    jsonProperties[i].GetString().ShouldNotBe(myStrings[i]);
                }
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooStringArray>(json);
            decrypted.MyStrings.ShouldBe(myStrings);
        }

        private class FooStringArray
        {
            [Encrypt]
            public string[] MyStrings { get; set; }
        }
    }
}
