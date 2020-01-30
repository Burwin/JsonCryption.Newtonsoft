using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class UIntTests
    {
        private static readonly string ApplicationName = typeof(UIntTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myUInt = (uint)69;
            var instance = new FooUIntPublic { MyUInt = myUInt };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooUIntPublic>(reader);

            decrypted.MyUInt.ShouldBe(myUInt);
        }

        private class FooUIntPublic
        {
            [Encrypt]
            public uint MyUInt { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myUInt = (uint)69;
            var instance = new FooUIntPrivate(myUInt);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooUIntPrivate>(reader);

            decrypted.GetMyUInt().ShouldBe(myUInt);
        }

        private class FooUIntPrivate
        {
            [Encrypt]
            [JsonProperty]
            private uint _myUInt;
            public uint GetMyUInt() => _myUInt;

            public FooUIntPrivate(uint myUInt)
            {
                _myUInt = myUInt;
            }
        }
    }
}
