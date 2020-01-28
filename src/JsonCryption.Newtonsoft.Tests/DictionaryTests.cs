using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class DictionaryTests
    {
        private static readonly string ApplicationName = typeof(DictionaryTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDoubles = new Dictionary<string, double>
            {
                { "first", 1.1 },
                { "second", 2.2 },
                { "third", 3.3 },
            };
            var instance = new FooDictionaryPublic { MyDoubles = myDoubles };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDictionaryPublic>(reader);

            decrypted.MyDoubles.ShouldBe(myDoubles);
        }

        private class FooDictionaryPublic
        {
            [Encrypt]
            public Dictionary<string, double> MyDoubles { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDoubles = new Dictionary<string, double>
            {
                { "first", 1.1 },
                { "second", 2.2 },
                { "third", 3.3 },
            };
            var instance = new FooDictionaryPrivate(myDoubles);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDictionaryPrivate>(reader);

            decrypted.GetMyDoubles().ShouldBe(myDoubles);
        }

        private class FooDictionaryPrivate
        {
            [Encrypt]
            [JsonProperty]
            private Dictionary<string, double> _myDoubles;
            public Dictionary<string, double> GetMyDoubles() => _myDoubles;

            public FooDictionaryPrivate(Dictionary<string, double> myDoubles)
            {
                _myDoubles = myDoubles;
            }
        }
    }
}
