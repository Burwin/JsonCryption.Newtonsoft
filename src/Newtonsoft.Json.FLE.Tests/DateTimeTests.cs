using Shouldly;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Newtonsoft.Json.FLE.Tests
{
    public class DateTimeTests
    {
        private static readonly string ApplicationName = typeof(DateTimeTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDate = DateTime.Parse("2020-01-27");
            var instance = new FooDateTimePublic { MyDate = myDate };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDateTimePublic>(reader);

            decrypted.MyDate.ShouldBe(myDate);
        }

        private class FooDateTimePublic
        {
            [Encrypt]
            public DateTime MyDate { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDate = DateTime.Parse("2020-01-27");
            var instance = new FooDateTimePrivate(myDate);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDateTimePrivate>(reader);

            decrypted.GetMyDate().ShouldBe(myDate);
        }

        private class FooDateTimePrivate
        {
            [Encrypt]
            [JsonProperty]
            private DateTime _myDate;
            public DateTime GetMyDate() => _myDate;

            public FooDateTimePrivate(DateTime myDate)
            {
                _myDate = myDate;
            }
        }
    }
}
