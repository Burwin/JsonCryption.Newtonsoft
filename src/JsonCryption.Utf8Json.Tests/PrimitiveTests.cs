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

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

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
            var instance = new FooInternal
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

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

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

            var deserialized = JsonSerializer.Deserialize<FooInternal>(json);

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

        class FooInternal
        {
            [Encrypt]
            internal bool MyBool { get; set; }
            [Encrypt]
            internal byte[] MyBytes { get; set; }
            [Encrypt]
            internal byte MyByte { get; set; }
            [Encrypt]
            internal char MyChar { get; set; }
            [Encrypt]
            internal DateTimeOffset MyDateTimeOffset { get; set; }
            [Encrypt]
            internal DateTime MyDateTime { get; set; }
            [Encrypt]
            internal decimal MyDecimal { get; set; }
            [Encrypt]
            internal double MyDouble { get; set; }
            [Encrypt]
            internal Fruit MyFruit { get; set; }
            [Encrypt]
            internal float MyFloat { get; set; }
            [Encrypt]
            internal Guid MyGuid { get; set; }
            [Encrypt]
            internal int MyInt { get; set; }
            [Encrypt]
            internal long MyLong { get; set; }
            [Encrypt]
            internal sbyte MySByte { get; set; }
            [Encrypt]
            internal short MyShort { get; set; }
            [Encrypt]
            internal string MyString { get; set; }
            [Encrypt]
            internal uint MyUInt { get; set; }
            [Encrypt]
            internal ulong MyULong { get; set; }
            [Encrypt]
            internal ushort MyUShort { get; set; }
        }

        [Fact]
        public void Internal_properties_are_ignored_without_AllowPrivate()
        {
            var instance = new FooInternal
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

            Helpers.SetJsonSerializerResolver(StandardResolver.Default);

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(nameof(instance.MyBool));
            json.ShouldNotContain(nameof(instance.MyByte));
            json.ShouldNotContain(nameof(instance.MyBytes));
            json.ShouldNotContain(nameof(instance.MyChar));
            json.ShouldNotContain(nameof(instance.MyDateTime));
            json.ShouldNotContain(nameof(instance.MyDateTimeOffset));
            json.ShouldNotContain(nameof(instance.MyDecimal));
            json.ShouldNotContain(nameof(instance.MyDouble));
            json.ShouldNotContain(nameof(instance.MyFloat));
            json.ShouldNotContain(nameof(instance.MyFruit));
            json.ShouldNotContain(nameof(instance.MyGuid));
            json.ShouldNotContain(nameof(instance.MyInt));
            json.ShouldNotContain(nameof(instance.MyLong));
            json.ShouldNotContain(nameof(instance.MySByte));
            json.ShouldNotContain(nameof(instance.MyShort));
            json.ShouldNotContain(nameof(instance.MyString));
            json.ShouldNotContain(nameof(instance.MyUInt));
            json.ShouldNotContain(nameof(instance.MyULong));
            json.ShouldNotContain(nameof(instance.MyUShort));

            var deserialized = JsonSerializer.Deserialize<FooInternal>(json);

            deserialized.MyBool.ShouldBe(default);
            deserialized.MyByte.ShouldBe(default);
            deserialized.MyBytes.ShouldBe(default);
            deserialized.MyChar.ShouldBe(default);
            deserialized.MyDateTime.ShouldBe(default);
            deserialized.MyDateTimeOffset.ShouldBe(default);
            deserialized.MyDecimal.ShouldBe(default);
            deserialized.MyDouble.ShouldBe(default);
            deserialized.MyFloat.ShouldBe(default);
            deserialized.MyFruit.ShouldBe(default);
            deserialized.MyGuid.ShouldBe(default);
            deserialized.MyInt.ShouldBe(default);
            deserialized.MyLong.ShouldBe(default);
            deserialized.MySByte.ShouldBe(default);
            deserialized.MyShort.ShouldBe(default);
            deserialized.MyString.ShouldBe(default);
            deserialized.MyUInt.ShouldBe(default);
            deserialized.MyULong.ShouldBe(default);
            deserialized.MyUShort.ShouldBe(default);
        }

        [Fact]
        public void Private_properties_succeed_with_AllowPrivate()
        {
            var instance = new FooPrivate(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain($"\"MyBool\":{instance.Bool()}");
            json.ShouldNotContain($"\"MyByte\":{instance.Byte()}");
            json.ShouldNotContain($"\"MyBytes\":[{byte.MinValue},4,{byte.MaxValue}]");
            json.ShouldNotContain($"\"MyChar\":'{instance.Char()}'");
            json.ShouldNotContain($"\"MyChar\":\"{instance.Char()}\"");
            json.ShouldNotContain($"\"MyDateTime\":\"{instance.DateTime().ToString("s")}\"");
            json.ShouldNotContain($"\"MyDateTimeOffset\":{instance.DateTimeOffset().ToString()}");
            json.ShouldNotContain($"\"MyDecimal\":{instance.Decimal()}");
            json.ShouldNotContain($"\"MyDouble\":{instance.Double()}");
            json.ShouldNotContain($"\"MyFloat\":{instance.Float()}");
            json.ShouldNotContain($"\"MyFruit\":\"Banana\"");
            json.ShouldNotContain($"\"MyFruit\":{instance.Fruit()}");
            json.ShouldNotContain($"\"MyGuid\":\"{instance.Guid().ToString()}\"");
            json.ShouldNotContain($"\"MyInt\":{instance.Int()}");
            json.ShouldNotContain($"\"MyLong\":{instance.Long()}");
            json.ShouldNotContain($"\"MySByte\":{instance.SByte()}");
            json.ShouldNotContain($"\"MyShort\":{instance.Short()}");
            json.ShouldNotContain($"\"MyString\":\"{instance.String()}\"");
            json.ShouldNotContain($"\"MyUInt\":{instance.UInt()}");
            json.ShouldNotContain($"\"MyULong\":{instance.ULong()}");
            json.ShouldNotContain($"\"MyUShort\":{instance.UShort()}");

            var deserialized = JsonSerializer.Deserialize<FooPrivate>(json);

            deserialized.Bool().ShouldBe(instance.Bool());
            deserialized.Byte().ShouldBe(instance.Byte());
            deserialized.Bytes().ShouldBe(instance.Bytes());
            deserialized.Char().ShouldBe(instance.Char());
            deserialized.DateTime().ShouldBe(instance.DateTime());
            deserialized.DateTimeOffset().ShouldBe(instance.DateTimeOffset());
            deserialized.Decimal().ShouldBe(instance.Decimal());
            deserialized.Double().ShouldBe(instance.Double());
            deserialized.Float().ShouldBe(instance.Float());
            deserialized.Fruit().ShouldBe(instance.Fruit());
            deserialized.Guid().ShouldBe(instance.Guid());
            deserialized.Int().ShouldBe(instance.Int());
            deserialized.Long().ShouldBe(instance.Long());
            deserialized.SByte().ShouldBe(instance.SByte());
            deserialized.Short().ShouldBe(instance.Short());
            deserialized.String().ShouldBe(instance.String());
            deserialized.UInt().ShouldBe(instance.UInt());
            deserialized.ULong().ShouldBe(instance.ULong());
            deserialized.UShort().ShouldBe(instance.UShort());
        }

        [Fact]
        public void Private_properties_are_ignored_without_AllowPrivate()
        {
            var instance = new FooPrivate(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver(StandardResolver.Default);

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain("MyBool");
            json.ShouldNotContain("MyByte");
            json.ShouldNotContain("MyBytes");
            json.ShouldNotContain("MyChar");
            json.ShouldNotContain("MyDateTime");
            json.ShouldNotContain("MyDateTimeOffset");
            json.ShouldNotContain("MyDecimal");
            json.ShouldNotContain("MyDouble");
            json.ShouldNotContain("MyFloat");
            json.ShouldNotContain("MyFruit");
            json.ShouldNotContain("MyGuid");
            json.ShouldNotContain("MyInt");
            json.ShouldNotContain("MyLong");
            json.ShouldNotContain("MySByte");
            json.ShouldNotContain("MyShort");
            json.ShouldNotContain("MyString");
            json.ShouldNotContain("MyUInt");
            json.ShouldNotContain("MyULong");
            json.ShouldNotContain("MyUShort");

            var deserialized = JsonSerializer.Deserialize<FooPrivate>(json);

            deserialized.Bool().ShouldBe(default);
            deserialized.Byte().ShouldBe(default);
            deserialized.Bytes().ShouldBe(default);
            deserialized.Char().ShouldBe(default);
            deserialized.DateTime().ShouldBe(default);
            deserialized.DateTimeOffset().ShouldBe(default);
            deserialized.Decimal().ShouldBe(default);
            deserialized.Double().ShouldBe(default);
            deserialized.Float().ShouldBe(default);
            deserialized.Fruit().ShouldBe(default);
            deserialized.Guid().ShouldBe(default);
            deserialized.Int().ShouldBe(default);
            deserialized.Long().ShouldBe(default);
            deserialized.SByte().ShouldBe(default);
            deserialized.Short().ShouldBe(default);
            deserialized.String().ShouldBe(default);
            deserialized.UInt().ShouldBe(default);
            deserialized.ULong().ShouldBe(default);
            deserialized.UShort().ShouldBe(default);
        }

        class FooPrivate
        {
            public FooPrivate(bool myBool, byte[] myBytes, byte myByte, char myChar, DateTimeOffset myDateTimeOffset, DateTime myDateTime, decimal myDecimal, double myDouble, Fruit myFruit, float myFloat, Guid myGuid, int myInt, long myLong, sbyte mySByte, short myShort, string myString, uint myUInt, ulong myULong, ushort myUShort)
            {
                MyBool = myBool;
                MyBytes = myBytes;
                MyByte = myByte;
                MyChar = myChar;
                MyDateTimeOffset = myDateTimeOffset;
                MyDateTime = myDateTime;
                MyDecimal = myDecimal;
                MyDouble = myDouble;
                MyFruit = myFruit;
                MyFloat = myFloat;
                MyGuid = myGuid;
                MyInt = myInt;
                MyLong = myLong;
                MySByte = mySByte;
                MyShort = myShort;
                MyString = myString;
                MyUInt = myUInt;
                MyULong = myULong;
                MyUShort = myUShort;
            }

            [Encrypt]
            private bool MyBool { get; set; }
            public bool Bool() => MyBool;
            [Encrypt]
            private byte[] MyBytes { get; set; }
            public byte[] Bytes() => MyBytes;
            [Encrypt]
            private byte MyByte { get; set; }
            public byte Byte() => MyByte;
            [Encrypt]
            private char MyChar { get; set; }
            public char Char() => MyChar;
            [Encrypt]
            private DateTimeOffset MyDateTimeOffset { get; set; }
            public DateTimeOffset DateTimeOffset() => MyDateTimeOffset;
            [Encrypt]
            private DateTime MyDateTime { get; set; }
            public DateTime DateTime() => MyDateTime;
            [Encrypt]
            private decimal MyDecimal { get; set; }
            public decimal Decimal() => MyDecimal;
            [Encrypt]
            private double MyDouble { get; set; }
            public double Double() => MyDouble;
            [Encrypt]
            private Fruit MyFruit { get; set; }
            public Fruit Fruit() => MyFruit;
            [Encrypt]
            private float MyFloat { get; set; }
            public float Float() => MyFloat;
            [Encrypt]
            private Guid MyGuid { get; set; }
            public Guid Guid() => MyGuid;
            [Encrypt]
            private int MyInt { get; set; }
            public int Int() => MyInt;
            [Encrypt]
            private long MyLong { get; set; }
            public long Long() => MyLong;
            [Encrypt]
            private sbyte MySByte { get; set; }
            public sbyte SByte() => MySByte;
            [Encrypt]
            private short MyShort { get; set; }
            public short Short() => MyShort;
            [Encrypt]
            private string MyString { get; set; }
            public string String() => MyString;
            [Encrypt]
            private uint MyUInt { get; set; }
            public uint UInt() => MyUInt;
            [Encrypt]
            private ulong MyULong { get; set; }
            public ulong ULong() => MyULong;
            [Encrypt]
            private ushort MyUShort { get; set; }
            public ushort UShort() => MyUShort;
        }

        [Fact]
        public void Public_fields()
        {
            var instance = new FooPublicFields
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

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

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

            var deserialized = JsonSerializer.Deserialize<FooPublicFields>(json);

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

        class FooPublicFields
        {
            [Encrypt]
            public bool MyBool;
            [Encrypt]
            public byte[] MyBytes;
            [Encrypt]
            public byte MyByte;
            [Encrypt]
            public char MyChar;
            [Encrypt]
            public DateTimeOffset MyDateTimeOffset;
            [Encrypt]
            public DateTime MyDateTime;
            [Encrypt]
            public decimal MyDecimal;
            [Encrypt]
            public double MyDouble;
            [Encrypt]
            public Fruit MyFruit;
            [Encrypt]
            public float MyFloat;
            [Encrypt]
            public Guid MyGuid;
            [Encrypt]
            public int MyInt;
            [Encrypt]
            public long MyLong;
            [Encrypt]
            public sbyte MySByte;
            [Encrypt]
            public short MyShort;
            [Encrypt]
            public string MyString;
            [Encrypt]
            public uint MyUInt;
            [Encrypt]
            public ulong MyULong;
            [Encrypt]
            public ushort MyUShort;
        }

        [Fact]
        public void Internal_fields_succeed_with_AllowPrivate()
        {
            var instance = new FooInternalFields
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

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

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

            var deserialized = JsonSerializer.Deserialize<FooInternalFields>(json);

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

        [Fact]
        public void Internal_fields_fail_without_AllowPrivate()
        {
            var instance = new FooInternalFields
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

            Helpers.SetJsonSerializerResolver(StandardResolver.Default);

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(nameof(instance.MyBool));
            json.ShouldNotContain(nameof(instance.MyByte));
            json.ShouldNotContain(nameof(instance.MyBytes));
            json.ShouldNotContain(nameof(instance.MyChar));
            json.ShouldNotContain(nameof(instance.MyDateTime));
            json.ShouldNotContain(nameof(instance.MyDateTimeOffset));
            json.ShouldNotContain(nameof(instance.MyDecimal));
            json.ShouldNotContain(nameof(instance.MyDouble));
            json.ShouldNotContain(nameof(instance.MyFloat));
            json.ShouldNotContain(nameof(instance.MyFruit));
            json.ShouldNotContain(nameof(instance.MyGuid));
            json.ShouldNotContain(nameof(instance.MyInt));
            json.ShouldNotContain(nameof(instance.MyLong));
            json.ShouldNotContain(nameof(instance.MySByte));
            json.ShouldNotContain(nameof(instance.MyShort));
            json.ShouldNotContain(nameof(instance.MyString));
            json.ShouldNotContain(nameof(instance.MyUInt));
            json.ShouldNotContain(nameof(instance.MyULong));
            json.ShouldNotContain(nameof(instance.MyUShort));

            var deserialized = JsonSerializer.Deserialize<FooInternalFields>(json);

            deserialized.MyBool.ShouldBe(default);
            deserialized.MyByte.ShouldBe(default);
            deserialized.MyBytes.ShouldBe(default);
            deserialized.MyChar.ShouldBe(default);
            deserialized.MyDateTime.ShouldBe(default);
            deserialized.MyDateTimeOffset.ShouldBe(default);
            deserialized.MyDecimal.ShouldBe(default);
            deserialized.MyDouble.ShouldBe(default);
            deserialized.MyFloat.ShouldBe(default);
            deserialized.MyFruit.ShouldBe(default);
            deserialized.MyGuid.ShouldBe(default);
            deserialized.MyInt.ShouldBe(default);
            deserialized.MyLong.ShouldBe(default);
            deserialized.MySByte.ShouldBe(default);
            deserialized.MyShort.ShouldBe(default);
            deserialized.MyString.ShouldBe(default);
            deserialized.MyUInt.ShouldBe(default);
            deserialized.MyULong.ShouldBe(default);
            deserialized.MyUShort.ShouldBe(default);
        }

        class FooInternalFields
        {
            [Encrypt]
            internal bool MyBool;
            [Encrypt]
            internal byte[] MyBytes;
            [Encrypt]
            internal byte MyByte;
            [Encrypt]
            internal char MyChar;
            [Encrypt]
            internal DateTimeOffset MyDateTimeOffset;
            [Encrypt]
            internal DateTime MyDateTime;
            [Encrypt]
            internal decimal MyDecimal;
            [Encrypt]
            internal double MyDouble;
            [Encrypt]
            internal Fruit MyFruit;
            [Encrypt]
            internal float MyFloat;
            [Encrypt]
            internal Guid MyGuid;
            [Encrypt]
            internal int MyInt;
            [Encrypt]
            internal long MyLong;
            [Encrypt]
            internal sbyte MySByte;
            [Encrypt]
            internal short MyShort;
            [Encrypt]
            internal string MyString;
            [Encrypt]
            internal uint MyUInt;
            [Encrypt]
            internal ulong MyULong;
            [Encrypt]
            internal ushort MyUShort;
        }

        [Fact]
        public void Private_fields_succeed_with_AllowPrivate()
        {
            var instance = new FooPrivateFields(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain($"\"MyBool\":{instance.Bool()}");
            json.ShouldNotContain($"\"MyByte\":{instance.Byte()}");
            json.ShouldNotContain($"\"MyBytes\":[{byte.MinValue},4,{byte.MaxValue}]");
            json.ShouldNotContain($"\"MyChar\":'{instance.Char()}'");
            json.ShouldNotContain($"\"MyChar\":\"{instance.Char()}\"");
            json.ShouldNotContain($"\"MyDateTime\":\"{instance.DateTime().ToString("s")}\"");
            json.ShouldNotContain($"\"MyDateTimeOffset\":{instance.DateTimeOffset().ToString()}");
            json.ShouldNotContain($"\"MyDecimal\":{instance.Decimal()}");
            json.ShouldNotContain($"\"MyDouble\":{instance.Double()}");
            json.ShouldNotContain($"\"MyFloat\":{instance.Float()}");
            json.ShouldNotContain($"\"MyFruit\":\"Banana\"");
            json.ShouldNotContain($"\"MyFruit\":{instance.Fruit()}");
            json.ShouldNotContain($"\"MyGuid\":\"{instance.Guid().ToString()}\"");
            json.ShouldNotContain($"\"MyInt\":{instance.Int()}");
            json.ShouldNotContain($"\"MyLong\":{instance.Long()}");
            json.ShouldNotContain($"\"MySByte\":{instance.SByte()}");
            json.ShouldNotContain($"\"MyShort\":{instance.Short()}");
            json.ShouldNotContain($"\"MyString\":\"{instance.String()}\"");
            json.ShouldNotContain($"\"MyUInt\":{instance.UInt()}");
            json.ShouldNotContain($"\"MyULong\":{instance.ULong()}");
            json.ShouldNotContain($"\"MyUShort\":{instance.UShort()}");

            var deserialized = JsonSerializer.Deserialize<FooPrivateFields>(json);

            deserialized.Bool().ShouldBe(instance.Bool());
            deserialized.Byte().ShouldBe(instance.Byte());
            deserialized.Bytes().ShouldBe(instance.Bytes());
            deserialized.Char().ShouldBe(instance.Char());
            deserialized.DateTime().ShouldBe(instance.DateTime());
            deserialized.DateTimeOffset().ShouldBe(instance.DateTimeOffset());
            deserialized.Decimal().ShouldBe(instance.Decimal());
            deserialized.Double().ShouldBe(instance.Double());
            deserialized.Float().ShouldBe(instance.Float());
            deserialized.Fruit().ShouldBe(instance.Fruit());
            deserialized.Guid().ShouldBe(instance.Guid());
            deserialized.Int().ShouldBe(instance.Int());
            deserialized.Long().ShouldBe(instance.Long());
            deserialized.SByte().ShouldBe(instance.SByte());
            deserialized.Short().ShouldBe(instance.Short());
            deserialized.String().ShouldBe(instance.String());
            deserialized.UInt().ShouldBe(instance.UInt());
            deserialized.ULong().ShouldBe(instance.ULong());
            deserialized.UShort().ShouldBe(instance.UShort());
        }

        [Fact]
        public void Private_fields_fail_without_AllowPrivate()
        {
            var instance = new FooPrivateFields(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver(StandardResolver.Default);

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain("MyBool");
            json.ShouldNotContain("MyByte");
            json.ShouldNotContain("MyBytes");
            json.ShouldNotContain("MyChar");
            json.ShouldNotContain("MyDateTime");
            json.ShouldNotContain("MyDateTimeOffset");
            json.ShouldNotContain("MyDecimal");
            json.ShouldNotContain("MyDouble");
            json.ShouldNotContain("MyFloat");
            json.ShouldNotContain("MyFruit");
            json.ShouldNotContain("MyGuid");
            json.ShouldNotContain("MyInt");
            json.ShouldNotContain("MyLong");
            json.ShouldNotContain("MySByte");
            json.ShouldNotContain("MyShort");
            json.ShouldNotContain("MyString");
            json.ShouldNotContain("MyUInt");
            json.ShouldNotContain("MyULong");
            json.ShouldNotContain("MyUShort");

            var deserialized = JsonSerializer.Deserialize<FooPrivateFields>(json);

            deserialized.Bool().ShouldBe(default);
            deserialized.Byte().ShouldBe(default);
            deserialized.Bytes().ShouldBe(default);
            deserialized.Char().ShouldBe(default);
            deserialized.DateTime().ShouldBe(default);
            deserialized.DateTimeOffset().ShouldBe(default);
            deserialized.Decimal().ShouldBe(default);
            deserialized.Double().ShouldBe(default);
            deserialized.Float().ShouldBe(default);
            deserialized.Fruit().ShouldBe(default);
            deserialized.Guid().ShouldBe(default);
            deserialized.Int().ShouldBe(default);
            deserialized.Long().ShouldBe(default);
            deserialized.SByte().ShouldBe(default);
            deserialized.Short().ShouldBe(default);
            deserialized.String().ShouldBe(default);
            deserialized.UInt().ShouldBe(default);
            deserialized.ULong().ShouldBe(default);
            deserialized.UShort().ShouldBe(default);
        }

        class FooPrivateFields
        {
            public FooPrivateFields(bool myBool, byte[] myBytes, byte myByte, char myChar, DateTimeOffset myDateTimeOffset, DateTime myDateTime, decimal myDecimal, double myDouble, Fruit myFruit, float myFloat, Guid myGuid, int myInt, long myLong, sbyte mySByte, short myShort, string myString, uint myUInt, ulong myULong, ushort myUShort)
            {
                MyBool = myBool;
                MyBytes = myBytes;
                MyByte = myByte;
                MyChar = myChar;
                MyDateTimeOffset = myDateTimeOffset;
                MyDateTime = myDateTime;
                MyDecimal = myDecimal;
                MyDouble = myDouble;
                MyFruit = myFruit;
                MyFloat = myFloat;
                MyGuid = myGuid;
                MyInt = myInt;
                MyLong = myLong;
                MySByte = mySByte;
                MyShort = myShort;
                MyString = myString;
                MyUInt = myUInt;
                MyULong = myULong;
                MyUShort = myUShort;
            }

            [Encrypt]
            private bool MyBool;
            public bool Bool() => MyBool;
            [Encrypt]
            private byte[] MyBytes;
            public byte[] Bytes() => MyBytes;
            [Encrypt]
            private byte MyByte;
            public byte Byte() => MyByte;
            [Encrypt]
            private char MyChar;
            public char Char() => MyChar;
            [Encrypt]
            private DateTimeOffset MyDateTimeOffset;
            public DateTimeOffset DateTimeOffset() => MyDateTimeOffset;
            [Encrypt]
            private DateTime MyDateTime;
            public DateTime DateTime() => MyDateTime;
            [Encrypt]
            private decimal MyDecimal;
            public decimal Decimal() => MyDecimal;
            [Encrypt]
            private double MyDouble;
            public double Double() => MyDouble;
            [Encrypt]
            private Fruit MyFruit;
            public Fruit Fruit() => MyFruit;
            [Encrypt]
            private float MyFloat;
            public float Float() => MyFloat;
            [Encrypt]
            private Guid MyGuid;
            public Guid Guid() => MyGuid;
            [Encrypt]
            private int MyInt;
            public int Int() => MyInt;
            [Encrypt]
            private long MyLong;
            public long Long() => MyLong;
            [Encrypt]
            private sbyte MySByte;
            public sbyte SByte() => MySByte;
            [Encrypt]
            private short MyShort;
            public short Short() => MyShort;
            [Encrypt]
            private string MyString;
            public string String() => MyString;
            [Encrypt]
            private uint MyUInt;
            public uint UInt() => MyUInt;
            [Encrypt]
            private ulong MyULong;
            public ulong ULong() => MyULong;
            [Encrypt]
            private ushort MyUShort;
            public ushort UShort() => MyUShort;
        }

        [Fact]
        public void Mix_of_public_and_private_properties_and_fields_with_AllowPrivate()
        {
            var instance = new FooMixed(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain($"\"MyBool\":{instance.Bool()}");
            json.ShouldNotContain($"\"MyByte\":{instance.Byte()}");
            json.ShouldNotContain($"\"MyBytes\":[{byte.MinValue},4,{byte.MaxValue}]");
            json.ShouldNotContain($"\"MyChar\":'{instance.Char()}'");
            json.ShouldNotContain($"\"MyChar\":\"{instance.Char()}\"");
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

            var deserialized = JsonSerializer.Deserialize<FooMixed>(json);

            deserialized.Bool().ShouldBe(instance.Bool());
            deserialized.Byte().ShouldBe(instance.Byte());
            deserialized.Bytes().ShouldBe(instance.Bytes());
            deserialized.Char().ShouldBe(instance.Char());
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

        [Fact]
        public void Mix_of_public_and_private_properties_and_fields_without_AllowPrivate()
        {
            var instance = new FooMixed(true, new byte[] { byte.MinValue, 4, byte.MaxValue }, byte.MaxValue, 'q', new DateTimeOffset(DateTime.Parse("2020/02/25")),
                DateTime.Parse("2020/02/24"), 1.234m, 5.678, Fruit.Banana, 9.1011f, Guid.NewGuid(), 75, long.MaxValue, sbyte.MaxValue, short.MaxValue, "something secret",
                uint.MaxValue, ulong.MaxValue, ushort.MaxValue);

            Helpers.SetJsonSerializerResolver(StandardResolver.Default);

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain($"\"MyBool\":{instance.Bool()}");
            json.ShouldNotContain($"\"MyByte\":{instance.Byte()}");
            json.ShouldNotContain($"\"MyBytes\":[{byte.MinValue},4,{byte.MaxValue}]");
            json.ShouldNotContain($"\"MyChar\":'{instance.Char()}'");
            json.ShouldNotContain($"\"MyChar\":\"{instance.Char()}\"");
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

            var deserialized = JsonSerializer.Deserialize<FooMixed>(json);

            deserialized.Bool().ShouldBe(default);
            deserialized.Byte().ShouldBe(default);
            deserialized.Bytes().ShouldBe(default);
            deserialized.Char().ShouldBe(default);
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

        class FooMixed
        {
            public FooMixed(bool myBool, byte[] myBytes, byte myByte, char myChar, DateTimeOffset myDateTimeOffset, DateTime myDateTime, decimal myDecimal, double myDouble, Fruit myFruit, float myFloat, Guid myGuid, int myInt, long myLong, sbyte mySByte, short myShort, string myString, uint myUInt, ulong myULong, ushort myUShort)
            {
                MyBool = myBool;
                MyBytes = myBytes;
                MyByte = myByte;
                MyChar = myChar;
                MyDateTimeOffset = myDateTimeOffset;
                MyDateTime = myDateTime;
                MyDecimal = myDecimal;
                MyDouble = myDouble;
                MyFruit = myFruit;
                MyFloat = myFloat;
                MyGuid = myGuid;
                MyInt = myInt;
                MyLong = myLong;
                MySByte = mySByte;
                MyShort = myShort;
                MyString = myString;
                MyUInt = myUInt;
                MyULong = myULong;
                MyUShort = myUShort;
            }

            [Encrypt]
            private bool MyBool;
            public bool Bool() => MyBool;
            [Encrypt]
            private byte[] MyBytes;
            public byte[] Bytes() => MyBytes;
            [Encrypt]
            private byte MyByte;
            public byte Byte() => MyByte;
            [Encrypt]
            private char MyChar;
            public char Char() => MyChar;

            [Encrypt]
            public DateTimeOffset MyDateTimeOffset;
            [Encrypt]
            public DateTime MyDateTime;
            [Encrypt]
            public decimal MyDecimal;
            [Encrypt]
            public double MyDouble;

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
    }
}
