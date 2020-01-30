using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class BooleanTests
    {
        private static readonly string ApplicationName = typeof(BooleanTests).AssemblyQualifiedName;
        
        [Fact]
        public void Public_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
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
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
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

        [Fact]
        public void Internal_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolInternalProperty { MyBool = true };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolInternalProperty>(reader);

            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBoolInternalProperty
        {
            [Encrypt]
            [JsonProperty]
            internal bool MyBool { get; set; }
        }

        [Fact]
        public void Protected_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolProtectedProperty(true);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolProtectedProperty>(reader);

            decrypted.GetMyBool().ShouldBeTrue();
        }

        private class FooBoolProtectedProperty
        {
            [Encrypt]
            [JsonProperty]
            protected bool MyBool { get; set; }
            public bool GetMyBool() => MyBool;

            public FooBoolProtectedProperty(bool myBool)
            {
                MyBool = myBool;
            }
        }

        [Fact]
        public void Private_properties_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolPrivateProperty(true);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolPrivateProperty>(reader);

            decrypted.GetMyBool().ShouldBeTrue();
        }

        private class FooBoolPrivateProperty
        {
            [Encrypt]
            [JsonProperty]
            private bool MyBool { get; set; }
            public bool GetMyBool() => MyBool;

            public FooBoolPrivateProperty(bool myBool)
            {
                MyBool = myBool;
            }
        }

        [Fact]
        public void Public_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolPublicField { MyBool = true };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolPublicField>(reader);

            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBoolPublicField
        {
            [Encrypt]
            public bool MyBool;
        }

        [Fact]
        public void Internal_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolInternalField { MyBool = true };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolInternalField>(reader);

            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBoolInternalField
        {
            [Encrypt]
            [JsonProperty]
            internal bool MyBool;
        }

        [Fact]
        public void Protected_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolProtectedField(true);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolProtectedField>(reader);

            decrypted.GetMyBool().ShouldBeTrue();
        }

        private class FooBoolProtectedField
        {
            [Encrypt]
            [JsonProperty]
            protected bool MyBool;
            public bool GetMyBool() => MyBool;

            public FooBoolProtectedField(bool myBool)
            {
                MyBool = myBool;
            }
        }

        [Fact]
        public void Private_fields_are_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new FooBoolPrivateField(true);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe("{\"MyBool\":true}");
            json.ShouldContain("MyBool");

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooBoolPrivateField>(reader);

            decrypted.GetMyBool().ShouldBeTrue();
        }

        private class FooBoolPrivateField
        {
            [Encrypt]
            [JsonProperty]
            protected bool MyBool;
            public bool GetMyBool() => MyBool;

            public FooBoolPrivateField(bool myBool)
            {
                MyBool = myBool;
            }
        }
    }
}
