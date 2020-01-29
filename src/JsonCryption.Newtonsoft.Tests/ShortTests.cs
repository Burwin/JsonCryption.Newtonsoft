using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class ShortTests
    {
        private static readonly string ApplicationName = typeof(ShortTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myShort = (short)69;
            var instance = new FooShortPublic { MyShort = myShort };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooShortPublic>(reader);

            decrypted.MyShort.ShouldBe(myShort);
        }

        private class FooShortPublic
        {
            [Encrypt]
            public short MyShort { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myShort = (short)69;
            var instance = new FooShortPrivate(myShort);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooShortPrivate>(reader);

            decrypted.GetMyShort().ShouldBe(myShort);
        }

        private class FooShortPrivate
        {
            [Encrypt]
            [JsonProperty]
            private short _myShort;
            public short GetMyShort() => _myShort;

            public FooShortPrivate(short myShort)
            {
                _myShort = myShort;
            }
        }
    }
}
