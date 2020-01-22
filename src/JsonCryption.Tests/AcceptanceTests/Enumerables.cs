using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class Enumerables
    {
        [Fact]
        public void String_enumerable_works()
        {
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

            var myStrings = new[] { "some", "strings", "to test" };
            var foo = new FooStringEnumerable { MyStrings = myStrings };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooStringEnumerable.MyStrings));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.Array);

                var jsonProperties = jsonProperty.EnumerateArray().ToList();
                jsonProperties.ForEach(el => el.ValueKind.ShouldBe(JsonValueKind.String));

                for (int i = 0; i < jsonProperties.Count; i++)
                {
                    jsonProperties[i].GetString().ShouldNotBe(myStrings[i]);
                }
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooStringEnumerable>(json);
            decrypted.MyStrings.ShouldBe(myStrings);
        }

        private class FooStringEnumerable
        {
            [Encrypt]
            public IEnumerable<string> MyStrings { get; set; }
        }

        [Fact]
        public void Int_enumerable_works()
        {
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

            var myInts = new[] { int.MinValue, -1, 0, 1, int.MaxValue };
            var foo = new FooIntEnumerable { MyInts = myInts };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooIntEnumerable.MyInts));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.Array);

                var jsonProperties = jsonProperty.EnumerateArray().ToList();
                jsonProperties.ForEach(el => el.ValueKind.ShouldBe(JsonValueKind.String));

                for (int i = 0; i < jsonProperties.Count; i++)
                {
                    jsonProperties[i].GetString().ShouldNotBe(myInts[i].ToString());
                }
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooIntEnumerable>(json);
            decrypted.MyInts.ShouldBe(myInts);;
        }

        private class FooIntEnumerable
        {
            [Encrypt]
            public IEnumerable<int> MyInts { get; set; }
        }
    }
}
