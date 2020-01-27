using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class CharTests
    {
        private static readonly string ApplicationName = typeof(CharTests).AssemblyQualifiedName;

        [Fact]
        public void Public_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooCharPublic { MyChar = (char)5 };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
            serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooCharPublic>(reader);

            decrypted.MyChar.ShouldBe((char)5);
        }

        private class FooCharPublic
        {
            [Encrypt]
            public char MyChar { get; set; }
        }

        [Fact]
        public void Private_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooCharPrivate((char)5);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
            serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooCharPrivate>(reader);

            decrypted.GetMyChar().ShouldBe((char)5);
        }

        private class FooCharPrivate
        {
            [Encrypt]
            [JsonProperty]
            private char _myChar;
            public char GetMyChar() => _myChar;

            public FooCharPrivate(char myChar)
            {
                _myChar = myChar;
            }
        }
    }
}
