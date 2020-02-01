using Shouldly;
using System.Text.Json;
using Xunit;

namespace JsonCryption.System.Text.Json.Tests.AcceptanceTests
{
    public class Arrays
    {
        [Fact]
        public void String_array_works()
        {
            Coordinator.ConfigureDefault("test");

            var myStrings = new[] { "some", "strings", "to test" };
            var foo = new FooStringArray { MyStrings = myStrings };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooStringArray.MyStrings));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
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

        [Fact]
        public void Int_array_works()
        {
            Coordinator.ConfigureDefault("test");

            var myInts = new[] { int.MinValue, -1, 0, 1, int.MaxValue };
            var foo = new FooIntArray { MyInts = myInts };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooIntArray.MyInts));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooIntArray>(json);
            decrypted.MyInts.ShouldBe(myInts);
        }

        private class FooIntArray
        {
            [Encrypt]
            public int[] MyInts { get; set; }
        }
    }
}
