using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class DateTimeOffsetTests
    {
        private static readonly string ApplicationName = typeof(DateTimeOffset).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDate = new DateTimeOffset(DateTime.Parse("2020-01-27"));
            var instance = new FooDateTimeOffsetPublic { MyDate = myDate };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDateTimeOffsetPublic>(reader);

            decrypted.MyDate.ShouldBe(myDate);
        }

        private class FooDateTimeOffsetPublic
        {
            [Encrypt]
            public DateTimeOffset MyDate { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new EncryptedContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDate = new DateTimeOffset(DateTime.Parse("2020-01-27"));
            var instance = new FooDateTimeOffsetPrivate(myDate);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDateTimeOffsetPrivate>(reader);

            decrypted.GetMyDate().ShouldBe(myDate);
        }

        private class FooDateTimeOffsetPrivate
        {
            [Encrypt]
            [JsonProperty]
            private DateTimeOffset _myDate;
            public DateTimeOffset GetMyDate() => _myDate;

            public FooDateTimeOffsetPrivate(DateTimeOffset myDate)
            {
                _myDate = myDate;
            }
        }
    }
}
