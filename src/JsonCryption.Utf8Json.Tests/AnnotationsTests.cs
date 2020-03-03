using Shouldly;
using System.Runtime.Serialization;
using System.Text;
using Utf8Json;
using Xunit;

namespace JsonCryption.Utf8Json.Tests
{
    public class AnnotationsTests
    {
        [Fact]
        public void Use_property_and_field_names_when_not_annotated()
        {
            var instance = new Foo { MyInt = 75, MyString = "secret" };

            Helpers.SetJsonSerializerResolver();
            
            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldContain(nameof(Foo.MyInt));
            json.ShouldContain(nameof(Foo.MyString));
        }

        class Foo
        {
            [Encrypt]
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }
        }

        [Fact]
        public void Use_DataMember_names_when_annotated()
        {
            var instance = new FooAnnotated { MyInt = 75, MyString = "secret" };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldContain("MyCustomInt");
            json.ShouldContain("MyCustomString");
        }

        class FooAnnotated
        {
            [Encrypt]
            [DataMember(Name = "MyCustomInt")]
            public int MyInt { get; set; }

            [Encrypt]
            [DataMember(Name = "MyCustomString")]
            public string MyString { get; set; }
        }

        [Fact]
        public void Ignore_members_when_ignored()
        {
            var instance = new FooIgnored { MyInt = 75, MyString = "secret" };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(nameof(FooIgnored.MyInt));
            json.ShouldContain(nameof(FooIgnored.MyString));
        }

        class FooIgnored
        {
            [IgnoreDataMember]
            public int MyInt { get; set; }
            
            [Encrypt]
            public string MyString { get; set; }
        }
    }
}
