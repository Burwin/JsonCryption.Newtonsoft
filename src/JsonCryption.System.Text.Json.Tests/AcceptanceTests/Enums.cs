using Microsoft.AspNetCore.DataProtection;
using Shouldly;
using System.Text.Json;
using Xunit;

namespace JsonCryption.System.Text.Json.Tests.AcceptanceTests
{
    public class Enums
    {
        [Fact]
        public void Enum_works_zero_based()
        {
            Coordinator.Configure(options => options.DataProtectionProvider = DataProtectionProvider.Create("test"));

            var myEnum = Sports.Football;
            var foo = new FooSportsEnum { MySports = myEnum };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooSportsEnum.MySports));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MySports));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooSportsEnum>(json);
            decrypted.MySports.ShouldBe(myEnum);
        }

        [Fact]
        public void Enum_works_one_based()
        {
            Coordinator.Configure(options => options.DataProtectionProvider = DataProtectionProvider.Create("test"));

            var myEnum = Fruit.Banana;
            var foo = new FooFruitEnum { MyFruit = myEnum };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooFruitEnum.MyFruit));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyFruit));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooFruitEnum>(json);
            decrypted.MyFruit.ShouldBe(myEnum);
        }

        private enum Fruit
        {
            Apple = 1,
            Orange,
            Banana
        }

        private enum Sports
        {
            Baseball = 0,
            Basketball,
            Football,
            Hockey
        }

        private class FooFruitEnum
        {
            [Encrypt]
            public Fruit MyFruit { get; set; }
        }

        private class FooSportsEnum
        {
            [Encrypt]
            public Sports MySports { get; set; }
        }
    }
}
