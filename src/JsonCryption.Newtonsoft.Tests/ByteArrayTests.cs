using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class ByteArrayTests
    {
        private static readonly string ApplicationName = typeof(ByteArrayTests).AssemblyQualifiedName;

        [Fact]
        public void Public_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myBytes = new byte[] { 5, 6, 7 };
            var instance = new FooByteArrayPublic { MyBytes = myBytes };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
            serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyByte\":[5,6,7]}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooByteArrayPublic>(reader);

            decrypted.MyBytes.ShouldBe(myBytes);
        }

        private class FooByteArrayPublic
        {
            [Encrypt]
            public byte[] MyBytes { get; set; }
        }

        [Fact]
        public void Private_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myBytes = new byte[] { 5, 6, 7 };
            var instance = new FooByteArrayPrivate(myBytes);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
            serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyByte\":[5,6,7]}");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooByteArrayPrivate>(reader);

            decrypted.GetMyBytes().ShouldBe(myBytes);
        }

        private class FooByteArrayPrivate
        {
            [Encrypt]
            [JsonProperty]
            private byte[] _myBytes;
            public byte[] GetMyBytes() => _myBytes;

            public FooByteArrayPrivate(byte[] myBytes)
            {
                _myBytes = myBytes;
            }
        }
    }
}
