using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class ULongTests
    {
        private static readonly string ApplicationName = typeof(ULongTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myULong = (ulong)69;
            var instance = new FooULongPublic { MyULong = myULong };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooULongPublic>(reader);

            decrypted.MyULong.ShouldBe(myULong);
        }

        private class FooULongPublic
        {
            [Encrypt]
            public ulong MyULong { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myULong = (ulong)69;
            var instance = new FooULongPrivate(myULong);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooULongPrivate>(reader);

            decrypted.GetMyULong().ShouldBe(myULong);
        }

        private class FooULongPrivate
        {
            [Encrypt]
            [JsonProperty]
            private ulong _myULong;
            public ulong GetMyULong() => _myULong;

            public FooULongPrivate(ulong myULong)
            {
                _myULong = myULong;
            }
        }
    }
}
