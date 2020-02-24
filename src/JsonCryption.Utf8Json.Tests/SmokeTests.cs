using Microsoft.AspNetCore.DataProtection;
using Shouldly;
using System;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;
using Xunit;

namespace JsonCryption.Utf8Json.Tests
{
    public class SmokeTests
    {
        [Fact]
        public void Primitives()
        {
            var instance = new Foo() { MyDecimal = 1.234m, MyInt = 75, MyString = "Unencrypted" };

            JsonSerializer.SetDefaultResolver(
                new EncryptedResolver(StandardResolver.AllowPrivate,
                DataProtectionProvider.Create(nameof(SmokeTests)).CreateProtector("test")));

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);
            json.ShouldNotContain(instance.MyString);

            var decrypted = JsonSerializer.Deserialize<Foo>(json);

            decrypted.MyDecimal.ShouldBe(instance.MyDecimal);
            decrypted.MyInt.ShouldBe(instance.MyInt);
            decrypted.MyString.ShouldBe(instance.MyString);
        }

        public class Foo
        {
            [Encrypt]
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }

            [Encrypt]
            public decimal MyDecimal { get; set; }
        }
    }
}
