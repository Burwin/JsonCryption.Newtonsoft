using Shouldly;
using System;
using System.Security.Cryptography;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class Encrypting_and_Decrypting
    {
        private byte[] GenerateRandomKey()
        {
            using (var aes = Aes.Create())
            {
                return aes.Key;
            }
        }

        [Fact]
        public void Boolean_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var foo = new FooBool { MyBool = true };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooBool.MyBool));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooBool>(json);
            decrypted.MyBool.ShouldBeTrue();
        }

        private class FooBool
        {
            [Encrypt]
            public bool MyBool { get; set; }
        }

        [Fact]
        public void Single_byte_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var foo = new FooByte { MyByte = 5 };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooByte.MyByte));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(foo.MyByte.ToString());
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooByte>(json);
            decrypted.MyByte.ShouldBe((byte)5);
        }

        private class FooByte
        {
            [Encrypt]
            public byte MyByte { get; set; }
        }

        [Fact]
        public void Byte_array_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var foo = new FooByteArray { MyBytes = new byte[] { 1, 1, 2, 3, 5, 8 } };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooByteArray.MyBytes));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyBytes));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooByteArray>(json);
            decrypted.MyBytes.ShouldBe(new byte[] { 1, 1, 2, 3, 5, 8 });
        }

        private class FooByteArray
        {
            [Encrypt]
            public byte[] MyBytes { get; set; }
        }

        [Fact]
        public void Char_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var foo = new FooChar { MyChar = 'x' };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooChar.MyChar));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyChar));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooChar>(json);
            decrypted.MyChar.ShouldBe('x');
        }

        private class FooChar
        {
            [Encrypt]
            public char MyChar { get; set; }
        }

        [Fact]
        public void DateTime_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var myDateTime = DateTime.Parse("2020-01-21");
            var foo = new FooDateTime { MyDateTime = myDateTime };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooDateTime.MyDateTime));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyDateTime));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooDateTime>(json);
            decrypted.MyDateTime.ShouldBe(myDateTime);
        }

        private class FooDateTime
        {
            [Encrypt]
            public DateTime MyDateTime { get; set; }
        }

        [Fact]
        public void DateTimeOffset_works()
        {
            Coordinator.ConfigureDefault(GenerateRandomKey());

            var myDateTimeOffset = DateTimeOffset.Parse("2020-01-21");
            var foo = new FooDateTimeOffset { MyDateTimeOffset = myDateTimeOffset };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooDateTimeOffset.MyDateTimeOffset));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyDateTimeOffset));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooDateTimeOffset>(json);
            decrypted.MyDateTimeOffset.ShouldBe(myDateTimeOffset);
        }

        private class FooDateTimeOffset
        {
            [Encrypt]
            public DateTimeOffset MyDateTimeOffset { get; set; }
        }

        [Fact]
        public void Decimal_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Double_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Enum_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Guid_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Int16_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Int32_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Int64_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SByte_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Single_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void String_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UInt16_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UInt32_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UInt64_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Array_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Enumerable_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void KeyValuePair_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void One_property_but_not_the_other()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Fields_instead_of_properties()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Internal_properties_and_fields()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Protected_properties_and_fields()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_properties_and_fields()
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
