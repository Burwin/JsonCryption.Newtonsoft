using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class GuidTests
    {
        private static readonly string ApplicationName = typeof(GuidTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myGuid = Guid.NewGuid();
            var instance = new FooGuidPublic { MyGuid = myGuid };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooGuidPublic>(reader);

            decrypted.MyGuid.ShouldBe(myGuid);
        }

        private class FooGuidPublic
        {
            [Encrypt]
            public Guid MyGuid { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myGuid = Guid.NewGuid();
            var instance = new FooGuidPrivate(myGuid);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooGuidPrivate>(reader);

            decrypted.GetMyGuid().ShouldBe(myGuid);
        }

        private class FooGuidPrivate
        {
            [Encrypt]
            [JsonProperty]
            private Guid _myGuid;
            public Guid GetMyGuid() => _myGuid;

            public FooGuidPrivate(Guid myGuid)
            {
                _myGuid = myGuid;
            }
        }
    }
}
