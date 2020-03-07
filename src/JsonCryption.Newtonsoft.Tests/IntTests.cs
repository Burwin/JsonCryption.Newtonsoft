using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class IntTests
    {
        private static readonly string ApplicationName = typeof(IntTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myInt = 69;
            var instance = new FooIntPublic { MyInt = myInt };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooIntPublic>(reader);

            decrypted.MyInt.ShouldBe(myInt);
        }

        private class FooIntPublic
        {
            [Encrypt]
            public int MyInt { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myInt = 69;
            var instance = new FooIntPrivate(myInt);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooIntPrivate>(reader);

            decrypted.GetMyInt().ShouldBe(myInt);
        }

        private class FooIntPrivate
        {
            [Encrypt]
            [JsonProperty]
            private int _myInt;
            public int GetMyInt() => _myInt;

            public FooIntPrivate(int myInt)
            {
                _myInt = myInt;
            }
        }
    }
}
