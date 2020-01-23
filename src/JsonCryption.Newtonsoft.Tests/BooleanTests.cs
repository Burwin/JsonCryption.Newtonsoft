using JsonCryption.Newtonsoft.Encrypters;
using Newtonsoft.Json;
using Shouldly;
using System;
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
            var encrypter = new AesManagedEncrypter(Helpers.GenerateRandomKey());
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
    }
}
