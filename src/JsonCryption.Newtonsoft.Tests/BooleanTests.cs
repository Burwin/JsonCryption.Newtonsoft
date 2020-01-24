using JsonCryption.Newtonsoft.Encryption;
using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class BooleanTests
    {
        [Fact]
        public void Public_properties_are_encrypted_and_decrypted()
        {
            var encrypter = Helpers.GetTestEncrypter();
            var contractResolver = new ContractResolver(encrypter);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBool { MyBool = true };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
            serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBool>(reader);

            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBool
        {
            [Encrypt]
            public bool MyBool { get; set; }
        }

        [Fact]
        public void Public_properties_not_decorated_with_EncryptAttribute_should_not_be_encrypted()
        {
            var encrypter = Helpers.GetTestEncrypter();
            var contractResolver = new ContractResolver(encrypter);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolNoEncrypt { MyBool = true };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldBe("{\"MyBool\":true}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolNoEncrypt>(reader);

            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBoolNoEncrypt
        {
            public bool MyBool { get; set; }
        }
    }
}
