using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class SByteTests
    {
        private static readonly string ApplicationName = typeof(SByteTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var mySByte = (sbyte)69;
            var instance = new FooSBytePublic { MySByte = mySByte };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooSBytePublic>(reader);

            decrypted.MySByte.ShouldBe(mySByte);
        }

        private class FooSBytePublic
        {
            [Encrypt]
            public sbyte MySByte { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var mySByte = (sbyte)69;
            var instance = new FooSBytePrivate(mySByte);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooSBytePrivate>(reader);

            decrypted.GetMySByte().ShouldBe(mySByte);
        }

        private class FooSBytePrivate
        {
            [Encrypt]
            [JsonProperty]
            private sbyte _mySByte;
            public sbyte GetMySByte() => _mySByte;

            public FooSBytePrivate(sbyte mySByte)
            {
                _mySByte = mySByte;
            }
        }
    }
}
