using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace Newtonsoft.Json.FLE.Tests
{
    public class StringTests
    {
        private static readonly string ApplicationName = typeof(StringTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myString = "my string";
            var instance = new FooStringPublic { MyString = myString };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooStringPublic>(reader);

            decrypted.MyString.ShouldBe(myString);
        }

        private class FooStringPublic
        {
            [Encrypt]
            public string MyString { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myString = "my string";
            var instance = new FooStringPrivate(myString);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooStringPrivate>(reader);

            decrypted.GetMyString().ShouldBe(myString);
        }

        private class FooStringPrivate
        {
            [Encrypt]
            [JsonProperty]
            private string _myString;
            public string GetMyString() => _myString;

            public FooStringPrivate(string myString)
            {
                _myString = myString;
            }
        }
    }
}
