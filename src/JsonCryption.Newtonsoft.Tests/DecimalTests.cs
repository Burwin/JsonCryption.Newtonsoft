using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class DecimalTests
    {
        private static readonly string ApplicationName = typeof(DateTimeTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDecimal = 123.4567m;
            var instance = new FooDecimalPublic { MyDecimal = myDecimal };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDecimalPublic>(reader);

            decrypted.MyDecimal.ShouldBe(myDecimal);
        }

        private class FooDecimalPublic
        {
            [Encrypt]
            public decimal MyDecimal { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDecimal = 123.4567m;
            var instance = new FooDecimalPrivate(myDecimal);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDecimalPrivate>(reader);

            decrypted.GetMyDecimal().ShouldBe(myDecimal);
        }

        private class FooDecimalPrivate
        {
            [Encrypt]
            [JsonProperty]
            private decimal _myDecimal;
            public decimal GetMyDecimal() => _myDecimal;

            public FooDecimalPrivate(decimal myDecimal)
            {
                _myDecimal = myDecimal;
            }
        }
    }
}
