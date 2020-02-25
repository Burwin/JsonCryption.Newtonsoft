using Microsoft.AspNetCore.DataProtection;
using Shouldly;
using System;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;
using Xunit;

namespace JsonCryption.Utf8Json.Tests
{
    public class PrimitiveTests
    {
        [Fact]
        public void Public_properties()
        {
            var instance = new FooPublic
            {
                MyBool = true,
                MyByte = byte.MaxValue,
                MyBytes = new byte[] { byte.MinValue, 4, byte.MaxValue },
                MyChar = 'q',
                MyDateTime = DateTime.Parse("2020/02/24"),
                MyDateTimeOffset = new DateTimeOffset(DateTime.Parse("2020/02/25")),
                MyDecimal = 1.234m,
                MyDouble = 5.678,
                MyFloat = 9.1011f,
                MyFruit = Fruit.Banana,
                MyGuid = Guid.NewGuid(),
                MyInt = 75,
                MyLong = long.MaxValue,
                MySByte = sbyte.MaxValue,
                MyShort = short.MaxValue,
                MyString = "something secret",
                MyUInt = uint.MaxValue,
                MyULong = ulong.MaxValue,
                MyUShort = ushort.MaxValue
            };

            JsonSerializer.SetDefaultResolver(
                new EncryptedResolver(StandardResolver.AllowPrivate,
                DataProtectionProvider.Create(nameof(SmokeTests)).CreateProtector("test")));

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            //json.ShouldBeNull();

            json.ShouldNotContain($"\"MyBool\":{instance.MyBool}");
            json.ShouldNotContain($"\"MyByte\":{instance.MyByte}");
            json.ShouldNotContain($"\"MyBytes\":[{byte.MinValue},4,{byte.MaxValue}]");
            json.ShouldNotContain($"\"MyChar\":'{instance.MyChar}'");
            json.ShouldNotContain($"\"MyChar\":\"{instance.MyChar}\"");
            json.ShouldNotContain($"\"MyDateTime\":\"{instance.MyDateTime.ToString("s")}\"");
            json.ShouldNotContain($"\"MyDateTimeOffset\":{instance.MyDateTimeOffset.ToString()}");
            json.ShouldNotContain($"\"MyDecimal\":{instance.MyDecimal}");
            json.ShouldNotContain($"\"MyDouble\":{instance.MyDouble}");
            json.ShouldNotContain($"\"MyFloat\":{instance.MyFloat}");
            json.ShouldNotContain($"\"MyFruit\":\"Banana\"");
            json.ShouldNotContain($"\"MyFruit\":{instance.MyFruit}");
            json.ShouldNotContain($"\"MyGuid\":\"{instance.MyGuid.ToString()}\"");
            json.ShouldNotContain($"\"MyInt\":{instance.MyInt}");
            json.ShouldNotContain($"\"MyLong\":{instance.MyLong}");
            json.ShouldNotContain($"\"MySByte\":{instance.MySByte}");
            json.ShouldNotContain($"\"MyShort\":{instance.MyShort}");
            json.ShouldNotContain($"\"MyString\":\"{instance.MyString}\"");
            json.ShouldNotContain($"\"MyUInt\":{instance.MyUInt}");
            json.ShouldNotContain($"\"MyULong\":{instance.MyULong}");
            json.ShouldNotContain($"\"MyUShort\":{instance.MyUShort}");

            var deserialized = JsonSerializer.Deserialize<FooPublic>(json);

            deserialized.MyBool.ShouldBe(instance.MyBool);
            deserialized.MyByte.ShouldBe(instance.MyByte);
            deserialized.MyBytes.ShouldBe(instance.MyBytes);
            deserialized.MyChar.ShouldBe(instance.MyChar);
            deserialized.MyDateTime.ShouldBe(instance.MyDateTime);
            deserialized.MyDateTimeOffset.ShouldBe(instance.MyDateTimeOffset);
            deserialized.MyDecimal.ShouldBe(instance.MyDecimal);
            deserialized.MyDouble.ShouldBe(instance.MyDouble);
            deserialized.MyFloat.ShouldBe(instance.MyFloat);
            deserialized.MyFruit.ShouldBe(instance.MyFruit);
            deserialized.MyGuid.ShouldBe(instance.MyGuid);
            deserialized.MyInt.ShouldBe(instance.MyInt);
            deserialized.MyLong.ShouldBe(instance.MyLong);
            deserialized.MySByte.ShouldBe(instance.MySByte);
            deserialized.MyShort.ShouldBe(instance.MyShort);
            deserialized.MyString.ShouldBe(instance.MyString);
            deserialized.MyUInt.ShouldBe(instance.MyUInt);
            deserialized.MyULong.ShouldBe(instance.MyULong);
            deserialized.MyUShort.ShouldBe(instance.MyUShort);
        }

        class FooPublic
        {
            [Encrypt]
            public bool MyBool { get; set; }
            [Encrypt]
            public byte[] MyBytes { get; set; }
            [Encrypt]
            public byte MyByte { get; set; }
            [Encrypt]
            public char MyChar { get; set; }
            [Encrypt]
            public DateTimeOffset MyDateTimeOffset { get; set; }
            [Encrypt]
            public DateTime MyDateTime { get; set; }
            [Encrypt]
            public decimal MyDecimal { get; set; }
            [Encrypt]
            public double MyDouble { get; set; }
            [Encrypt]
            public Fruit MyFruit { get; set; }
            [Encrypt]
            public float MyFloat { get; set; }
            [Encrypt]
            public Guid MyGuid { get; set; }
            [Encrypt]
            public int MyInt { get; set; }
            [Encrypt]
            public long MyLong { get; set; }
            [Encrypt]
            public sbyte MySByte { get; set; }
            [Encrypt]
            public short MyShort { get; set; }
            [Encrypt]
            public string MyString { get; set; }
            [Encrypt]
            public uint MyUInt { get; set; }
            [Encrypt]
            public ulong MyULong { get; set; }
            [Encrypt]
            public ushort MyUShort { get; set; }
        }

        public enum Fruit
        {
            Apple,
            Orange,
            Banana
        }

        [Fact]
        public void Internal_properties_succeed_with_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Internal_properties_fail_without_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_properties_succeed_with_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_properties_fail_without_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Public_fields()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Internal_fields_succeed_with_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Internal_fields_fail_without_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_fields_succeed_with_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Private_fields_fail_without_AllowPrivate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Mix_of_public_and_private_properties_and_fields()
        {
            throw new NotImplementedException();
        }
    }
}
