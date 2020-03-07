using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class EnumTests
    {
        private static readonly string ApplicationName = typeof(EnumTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myEnum = Fruit.Banana;
            var instance = new FooEnumPublic { MyEnum = myEnum };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooEnumPublic>(reader);

            decrypted.MyEnum.ShouldBe(myEnum);
        }

        private class FooEnumPublic
        {
            [Encrypt]
            public Fruit MyEnum { get; set; }
        }

        private enum Fruit
        {
            Apple,
            Orange,
            Banana
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myEnum = Sports.Basketball;
            var instance = new FooEnumPrivate(myEnum);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooEnumPrivate>(reader);

            decrypted.GetMyEnum().ShouldBe(myEnum);
        }

        private class FooEnumPrivate
        {
            [Encrypt]
            [JsonProperty]
            private Sports _myEnum;
            public Sports GetMyEnum() => _myEnum;

            public FooEnumPrivate(Sports myEnum)
            {
                _myEnum = myEnum;
            }
        }

        private enum Sports
        {
            Baseball,
            Basketball,
            Football,
            Hockey
        }
    }
}
