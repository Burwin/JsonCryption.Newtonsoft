using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
namespace JsonCryption.Newtonsoft.Tests
{
    public class NestedObjectTests
    {
        class Parent
        {
            [Encrypt]
            public string EncryptedString { get; set; }

            [Encrypt]
            public FullChild FullChild { get; set; }

            public PartialChild PartialChild { get; set; }
        }
        
        class FullChild
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }

        class PartialChild
        {
            public int MyInt { get; set; }

            [Encrypt]
            public string MyString { get; set; }
        }

        [Fact]
        public void Single_level_nested_objects()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(nameof(NestedObjectTests));
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var instance = new Parent
            {
                EncryptedString = "this should be encrypted",
                FullChild = new FullChild
                {
                    MyInt = 37,
                    MyString = "full child string"
                },
                PartialChild = new PartialChild
                {
                    MyInt = 38,
                    MyString = "partial child string"
                }
            };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();
            var jObject = JObject.Parse(json);


            // make sure the string was encrypted
            jObject.Value<string>(nameof(Parent.EncryptedString)).ShouldNotBe(instance.EncryptedString);

            // This should throw because we shouldn't be able to convert the FullChild since its
            // 'MyInt' is an encrypted string
            Should.Throw<Exception>(() => jObject.GetValue(nameof(FullChild)).ToObject<FullChild>());
            
            // This should NOT throw because the entire FullChild is an encrypted string
            jObject.Value<string>(nameof(FullChild));

            // This should be the same since we aren't encrypting the 'MyInt' of PartialChild
            var partialInt = jObject.Property(nameof(PartialChild)).Values().Cast<JProperty>().First(v => v.Name == nameof(PartialChild.MyInt));
            partialInt.ToObject<int>().ShouldBe(instance.PartialChild.MyInt);

            // This should not be the same since we ARE encrypting the 'MyString' of PartialChild
            var partialString = jObject.Property(nameof(PartialChild)).Values().Cast<JProperty>().First(v => v.Name == nameof(PartialChild.MyString));
            partialString.ToObject<string>().ShouldNotBe(instance.PartialChild.MyString);


            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<Parent>(reader);

            decrypted.EncryptedString.ShouldBe(instance.EncryptedString);
            decrypted.FullChild.MyInt.ShouldBe(instance.FullChild.MyInt);
            decrypted.FullChild.MyString.ShouldBe(instance.FullChild.MyString);
        }
    }
}
