using Shouldly;
using System;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class Special_cases
    {
        [Fact]
        public void One_property_but_not_the_other()
        {
            Coordinator.ConfigureDefault("test");

            var foo = new FooMixedEncryption { EncryptedInt = 57, UnencryptedString = "foo"};
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var encryptedInt = jsonDoc.RootElement.GetProperty(nameof(FooMixedEncryption.EncryptedInt));
                encryptedInt.ValueKind.ShouldBe(JsonValueKind.String);
                encryptedInt.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.EncryptedInt));

                var unencryptedString = jsonDoc.RootElement.GetProperty(nameof(FooMixedEncryption.UnencryptedString));
                unencryptedString.ValueKind.ShouldBe(JsonValueKind.String);
                unencryptedString.GetString().ShouldBe(foo.UnencryptedString);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooMixedEncryption>(json);
            decrypted.UnencryptedString.ShouldBe(foo.UnencryptedString);
            decrypted.EncryptedInt.ShouldBe(foo.EncryptedInt);
        }

        private class FooMixedEncryption
        {
            [Encrypt]
            public int EncryptedInt { get; set; }
            public string UnencryptedString { get; set; }
        }

        [Fact(Skip = "Waiting to be supported by System.Text.Json")]
        public void Fields_instead_of_properties()
        {
            Coordinator.ConfigureDefault("test");

            var foo = new FooPublicFields();
            foo.EncryptedInt = 57;
            foo.UnencryptedString = "foo";

            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var encryptedInt = jsonDoc.RootElement.GetProperty(nameof(FooPublicFields.EncryptedInt));
                encryptedInt.ValueKind.ShouldBe(JsonValueKind.String);
                encryptedInt.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.EncryptedInt));

                var unencryptedString = jsonDoc.RootElement.GetProperty(nameof(FooPublicFields.UnencryptedString));
                unencryptedString.ValueKind.ShouldBe(JsonValueKind.String);
                unencryptedString.GetString().ShouldBe(foo.UnencryptedString);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooPublicFields>(json);
            decrypted.UnencryptedString.ShouldBe(foo.UnencryptedString);
            decrypted.EncryptedInt.ShouldBe(foo.EncryptedInt);
        }

        private class FooPublicFields
        {
            [Encrypt]
            public int EncryptedInt;
            public string UnencryptedString;
        }

        [Fact(Skip = "Waiting to be supported by System.Text.Json")]
        public void Internal_properties()
        {
            Coordinator.ConfigureDefault("test");

            var foo = new FooInternalProperties { MyInt = 57, MyString = "foo" };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var encryptedInt = jsonDoc.RootElement.GetProperty(nameof(FooInternalProperties.MyInt));
                encryptedInt.ValueKind.ShouldBe(JsonValueKind.String);
                encryptedInt.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyInt));

                var unencryptedString = jsonDoc.RootElement.GetProperty(nameof(FooInternalProperties.MyString));
                unencryptedString.ValueKind.ShouldBe(JsonValueKind.String);
                unencryptedString.GetString().ShouldBe(foo.MyString);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooInternalProperties>(json);
            decrypted.MyString.ShouldBe(foo.MyString);
            decrypted.MyInt.ShouldBe(foo.MyInt);
        }

        private class FooInternalProperties
        {
            [Encrypt]
            internal int MyInt { get; set; }
            
            [Encrypt]
            internal string MyString { get; set; }
        }

        [Fact(Skip = "Waiting to be supported by System.Text.Json")]
        public void Protected_properties()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "Waiting to be supported by System.Text.Json")]
        public void Private_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Structs_with_supported_properties_should_encrypt_and_decrypt()
        {
            Coordinator.ConfigureDefault("test");

            var foo = new BarPublicProperties { MyInt = 57, MyString = "foo" };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var encryptedInt = jsonDoc.RootElement.GetProperty(nameof(BarPublicProperties.MyInt));
                encryptedInt.ValueKind.ShouldBe(JsonValueKind.String);
                encryptedInt.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyInt));

                var encryptedString = jsonDoc.RootElement.GetProperty(nameof(BarPublicProperties.MyString));
                encryptedString.ValueKind.ShouldBe(JsonValueKind.String);
                encryptedString.GetString().ShouldNotBe(foo.MyString);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<BarPublicProperties>(json);
            decrypted.MyString.ShouldBe(foo.MyString);
            decrypted.MyInt.ShouldBe(foo.MyInt);
        }

        private struct BarPublicProperties
        {
            [Encrypt]
            public int MyInt { get; set; }
            
            [Encrypt]
            public string MyString { get; set; }
        }
    }
}
