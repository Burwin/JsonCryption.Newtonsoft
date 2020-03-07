using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace Newtonsoft.Json.FLE.Tests
{
    public class FloatTests
    {
        private static readonly string ApplicationName = typeof(FloatTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myFloat = 123.4567f;
            var instance = new FooFloatPublic { MyFloat = myFloat };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooFloatPublic>(reader);

            decrypted.MyFloat.ShouldBe(myFloat);
        }

        private class FooFloatPublic
        {
            [Encrypt]
            public float MyFloat { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myFloat = 123.4567f;
            var instance = new FooFloatPrivate(myFloat);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooFloatPrivate>(reader);

            decrypted.GetMyFloat().ShouldBe(myFloat);
        }

        private class FooFloatPrivate
        {
            [Encrypt]
            [JsonProperty]
            private float _myFloat;
            public float GetMyFloat() => _myFloat;

            public FooFloatPrivate(float myFloat)
            {
                _myFloat = myFloat;
            }
        }
    }
}
