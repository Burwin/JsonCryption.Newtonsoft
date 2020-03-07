using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class DoubleTests
    {
        private static readonly string ApplicationName = typeof(DoubleTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDouble = 123.4567;
            var instance = new FooDoublePublic { MyDouble = myDouble };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDoublePublic>(reader);

            decrypted.MyDouble.ShouldBe(myDouble);
        }

        private class FooDoublePublic
        {
            [Encrypt]
            public double MyDouble { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDouble = 123.4567;
            var instance = new FooDoublePrivate(myDouble);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDoublePrivate>(reader);

            decrypted.GetMyDouble().ShouldBe(myDouble);
        }

        private class FooDoublePrivate
        {
            [Encrypt]
            [JsonProperty]
            private double _myDouble;
            public double GetMyDouble() => _myDouble;

            public FooDoublePrivate(double myDouble)
            {
                _myDouble = myDouble;
            }
        }
    }
}
