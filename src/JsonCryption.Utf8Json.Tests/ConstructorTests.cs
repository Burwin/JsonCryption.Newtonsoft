using Shouldly;
using System.Runtime.Serialization;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;
using Xunit;

namespace JsonCryption.Utf8Json.Tests
{
    public class ConstructorTests
    {
        [Fact]
        public void Use_default_constructor_if_nothing_else_exists()
        {
            var instance = new FooDefault(1.2) { MyInt = 75, MyString = "secret" };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyInt, StandardResolver.AllowPrivate, nameof(instance.MyInt)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyString, StandardResolver.AllowPrivate, nameof(instance.MyString)));

            var deserialized = JsonSerializer.Deserialize<FooDefault>(json);

            deserialized.MyInt.ShouldBe(instance.MyInt);
            deserialized.MyString.ShouldBe(instance.MyString);
            deserialized.RandomDouble().ShouldBe(-1.111);
        }

        class FooDefault
        {
            [Encrypt]
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }

            [IgnoreDataMember]
            private double _randomDouble;
            public double RandomDouble() => _randomDouble;

            // some other constructor that doesn't match anything
            public FooDefault(double randomDouble)
            {
                _randomDouble = -1.111;
            }
        }

        [Fact]
        public void Use_closest_matching_constructor_if_nothing_annotated()
        {
            var instance = new FooClosest { MyInt = 75, MyString = "secret" };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyInt, StandardResolver.AllowPrivate, nameof(instance.MyInt)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyString, StandardResolver.AllowPrivate, nameof(instance.MyString)));

            var deserialized = JsonSerializer.Deserialize<FooClosest>(json);

            deserialized.MyInt.ShouldBe(instance.MyInt);
            deserialized.MyString.ShouldBe(instance.MyString);
            deserialized.Token().ShouldBe("two");
        }

        class FooClosest
        {
            [Encrypt]
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }

            [IgnoreDataMember]
            private string _token;
            public string Token() => _token;

            public FooClosest()
            {
                _token = "zero";
            }

            private FooClosest(int myInt)
            {
                _token = "one";
            }

            // also works with private constructors by default (without annotations)
            private FooClosest(int myInt, string myString)
            {
                _token = "two";
                MyInt = myInt;
                MyString = myString;
            }
        }

        [Fact]
        public void Use_annotated_constructor_if_exists()
        {
            var instance = new FooAnnotated("something secret", 75);
            
            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyInt, StandardResolver.AllowPrivate, nameof(instance.MyInt)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyString, StandardResolver.AllowPrivate, nameof(instance.MyString)));

            var deserialized = JsonSerializer.Deserialize<FooAnnotated>(json);

            // 2 * 75
            deserialized.MyInt.ShouldBe(150);

            // the value(s) that the constructor doesn't set (by name according to constructor params) will still be set after the constructor is used
            deserialized.MyString.ShouldBe("something secret");
        }

        class FooAnnotated
        {
            [Encrypt]
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }

            public FooAnnotated(string myString, int myInt)
            {
                MyInt = myInt;
                MyString = myString;
            }

            [SerializationConstructor]
            public FooAnnotated(int myInt)
            {
                MyInt = myInt * 2;
            }
        }
    }
}
