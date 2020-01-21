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
            Coordinator.CreateDefault(GenerateRandomKey());

            var foo = new FooBool { MyBool = true };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var myBool = jsonDoc.RootElement.GetProperty(nameof(FooBool.MyBool));
                myBool.ValueKind.ShouldBe(JsonValueKind.String);
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
            throw new NotImplementedException();
        }

        [Fact]
        public void Byte_array_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Char_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DateTime_works()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DateTimeOffset_works()
        {
            throw new NotImplementedException();
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
    }
}
