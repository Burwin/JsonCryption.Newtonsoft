using Shouldly;
using System;
using System.Text.Json;
using Xunit;

namespace JsonCryption.Tests.AcceptanceTests
{
    public class Built_in_types
    {
        [Fact]
        public void Boolean_works()
        {
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

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
            Coordinator.ConfigureDefault("test");

            var myDecimal = 123.456m;
            var foo = new FooDecimal { MyDecimal = myDecimal };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooDecimal.MyDecimal));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyDecimal));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooDecimal>(json);
            decrypted.MyDecimal.ShouldBe(myDecimal);
        }

        private class FooDecimal
        {
            [Encrypt]
            public decimal MyDecimal { get; set; }
        }

        [Fact]
        public void Double_works()
        {
            Coordinator.ConfigureDefault("test");

            var myDouble = 123.456d;
            var foo = new FooDouble { MyDouble = myDouble };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooDouble.MyDouble));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyDouble));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooDouble>(json);
            decrypted.MyDouble.ShouldBe(myDouble);
        }

        private class FooDouble
        {
            [Encrypt]
            public double MyDouble { get; set; }
        }

        [Fact]
        public void Guid_works()
        {
            Coordinator.ConfigureDefault("test");

            var myGuid = Guid.NewGuid();
            var foo = new FooGuid { MyGuid = myGuid };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooGuid.MyGuid));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(myGuid.ToString());
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooGuid>(json);
            decrypted.MyGuid.ShouldBe(myGuid);
        }

        private class FooGuid
        {
            [Encrypt]
            public Guid MyGuid { get; set; }
        }

        [Fact]
        public void Int16_works()
        {
            Coordinator.ConfigureDefault("test");

            var myShort = (short)357;
            var foo = new FooShort { MyShort = myShort };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooShort.MyShort));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyShort));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooShort>(json);
            decrypted.MyShort.ShouldBe(myShort);
        }

        private class FooShort
        {
            [Encrypt]
            public short MyShort { get; set; }
        }

        [Fact]
        public void Int32_works()
        {
            Coordinator.ConfigureDefault("test");

            var myInt = 357;
            var foo = new FooInt { MyInt = myInt };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooInt.MyInt));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyInt));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooInt>(json);
            decrypted.MyInt.ShouldBe(myInt);
        }

        private class FooInt
        {
            [Encrypt]
            public int MyInt { get; set; }
        }

        [Fact]
        public void Int64_works()
        {
            Coordinator.ConfigureDefault("test");

            var myLong = 357L;
            var foo = new FooLong { MyLong = myLong };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooLong.MyLong));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyLong));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooLong>(json);
            decrypted.MyLong.ShouldBe(myLong);
        }

        private class FooLong
        {
            [Encrypt]
            public long MyLong { get; set; }
        }

        [Fact]
        public void SByte_works()
        {
            Coordinator.ConfigureDefault("test");

            var mySByte = (sbyte)5;
            var foo = new FooSByte { MySByte = mySByte };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooSByte.MySByte));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MySByte));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooSByte>(json);
            decrypted.MySByte.ShouldBe(mySByte);
        }

        private class FooSByte
        {
            [Encrypt]
            public sbyte MySByte { get; set; }
        }

        [Fact]
        public void Float_works()
        {
            Coordinator.ConfigureDefault("test");

            var myFloat = 123.456f;
            var foo = new FooFloat { MyFloat = myFloat };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooFloat.MyFloat));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyFloat));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooFloat>(json);
            decrypted.MyFloat.ShouldBe(myFloat);
        }

        private class FooFloat
        {
            [Encrypt]
            public float MyFloat { get; set; }
        }

        [Fact]
        public void String_works()
        {
            Coordinator.ConfigureDefault("test");

            var myString = "something secret";
            var foo = new FooString { MyString = myString };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooString.MyString));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyString));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooString>(json);
            decrypted.MyString.ShouldBe(myString);
        }

        private class FooString
        {
            [Encrypt]
            public string MyString { get; set; }
        }

        [Fact]
        public void UInt16_works()
        {
            Coordinator.ConfigureDefault("test");

            var myUnsignedShort = ushort.MaxValue;
            var foo = new FooUnsignedShort { MyUnsignedShort = myUnsignedShort };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooUnsignedShort.MyUnsignedShort));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyUnsignedShort));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooUnsignedShort>(json);
            decrypted.MyUnsignedShort.ShouldBe(myUnsignedShort);
        }

        private class FooUnsignedShort
        {
            [Encrypt]
            public ushort MyUnsignedShort { get; set; }
        }

        [Fact]
        public void UInt32_works()
        {
            Coordinator.ConfigureDefault("test");

            var myUnsignedInt = uint.MaxValue;
            var foo = new FooUnsignedInt { MyUnsignedInt = myUnsignedInt };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooUnsignedInt.MyUnsignedInt));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyUnsignedInt));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooUnsignedInt>(json);
            decrypted.MyUnsignedInt.ShouldBe(myUnsignedInt);
        }

        private class FooUnsignedInt
        {
            [Encrypt]
            public uint MyUnsignedInt { get; set; }
        }

        [Fact]
        public void UInt64_works()
        {
            Coordinator.ConfigureDefault("test");

            var myUnsignedLong = ulong.MaxValue;
            var foo = new FooUnsignedLong { MyUnsignedLong = myUnsignedLong };
            var json = JsonSerializer.Serialize(foo);

            // make sure it's encrypted
            using (var jsonDoc = JsonDocument.Parse(json))
            {
                var jsonProperty = jsonDoc.RootElement.GetProperty(nameof(FooUnsignedLong.MyUnsignedLong));
                jsonProperty.ValueKind.ShouldBe(JsonValueKind.String);
                jsonProperty.GetString().ShouldNotBe(JsonSerializer.Serialize(foo.MyUnsignedLong));
            }

            // decrypt and check
            var decrypted = JsonSerializer.Deserialize<FooUnsignedLong>(json);
            decrypted.MyUnsignedLong.ShouldBe(myUnsignedLong);
        }

        private class FooUnsignedLong
        {
            [Encrypt]
            public ulong MyUnsignedLong { get; set; }
        }
    }
}
