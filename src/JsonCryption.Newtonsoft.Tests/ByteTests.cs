using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class ByteTests
    {
        private static readonly string ApplicationName = typeof(ByteTests).AssemblyQualifiedName;

        [Fact]
        public void Public_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBytePublic { MyByte = 5 };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyByte\":5}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBytePublic>(reader);

            decrypted.MyByte.ShouldBe((byte)5);
        }

        private class FooBytePublic
        {
            [Encrypt]
            public byte MyByte { get; set; }
        }

        [Fact]
        public void Private_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBytePrivate(5);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyByte\":5}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBytePrivate>(reader);

            decrypted.GetMyByte().ShouldBe((byte)5);
        }

        private class FooBytePrivate
        {
            [Encrypt]
            [JsonProperty]
            private byte _myByte;
            public byte GetMyByte() => _myByte;

            public FooBytePrivate(byte myByte)
            {
                _myByte = myByte;
            }
        }
    }
}
