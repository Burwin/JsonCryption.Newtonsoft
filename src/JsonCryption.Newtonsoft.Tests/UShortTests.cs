using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class UShortTests
    {
        private static readonly string ApplicationName = typeof(UShortTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myUShort = (ushort)75;
            var instance = new FooUShortPublic { MyUShort = myUShort };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooUShortPublic>(reader);

            decrypted.MyUShort.ShouldBe(myUShort);
        }

        private class FooUShortPublic
        {
            [Encrypt]
            public ushort MyUShort { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myUShort = (ushort)75;
            var instance = new FooUShortPrivate(myUShort);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooUShortPrivate>(reader);

            decrypted.GetMyUShort().ShouldBe(myUShort);
        }

        private class FooUShortPrivate
        {
            [Encrypt]
            [JsonProperty]
            private ushort _myUShort;
            public ushort GetMyUShort() => _myUShort;

            public FooUShortPrivate(ushort myUShort)
            {
                _myUShort = myUShort;
            }
        }
    }
}
