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
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

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

        // field support not yet available for System.Text.Json
        //[Fact]
        //public void Fields_instead_of_properties()
        //{
        //    Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

        //    var foo = new FooPublicFields();
        //    foo.EncryptedInt = 57;
        //    foo.UnencryptedString = "foo";

        //    var json = JsonSerializer.Serialize(foo);

        //    // make sure it's encrypted
        //    using (var jsonDoc = JsonDocument.Parse(json))
        //    {
        //        var encryptedInt = jsonDoc.RootElement.GetProperty(nameof(FooPublicFields.EncryptedInt));
        //        encryptedInt.ValueKind.ShouldBe(JsonValueKind.String);
        //        encryptedInt.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.EncryptedInt));

        //        var unencryptedString = jsonDoc.RootElement.GetProperty(nameof(FooPublicFields.UnencryptedString));
        //        unencryptedString.ValueKind.ShouldBe(JsonValueKind.String);
        //        unencryptedString.GetString().ShouldBe(foo.UnencryptedString);
        //    }

        //    // decrypt and check
        //    var decrypted = JsonSerializer.Deserialize<FooPublicFields>(json);
        //    decrypted.UnencryptedString.ShouldBe(foo.UnencryptedString);
        //    decrypted.EncryptedInt.ShouldBe(foo.EncryptedInt);
        //}

        //private class FooPublicFields
        //{
        //    [Encrypt]
        //    public int EncryptedInt;
        //    public string UnencryptedString;
        //}

        [Fact]
        public void Internal_properties()
        {
            Coordinator.ConfigureDefault(Helpers.GenerateRandomKey());

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

            throw new NotImplementedException();
        }

        private class FooInternalProperties
        {
            [Encrypt]
            internal int MyInt { get; set; }
            
            [Encrypt]
            internal string MyString { get; set; }
        }

        [Fact]
        public void Protected_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Structs_with_supported_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Structs_with_unsupported_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Structs_with_mix_of_supported_and_unsupported_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Classes_with_supported_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Classes_with_unsupported_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Classes_with_mix_of_supported_and_unsupported_properties()
        {
            throw new NotImplementedException();
        }
    }
}
