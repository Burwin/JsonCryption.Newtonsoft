﻿using Newtonsoft.Json;
using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class LongTests
    {
        private static readonly string ApplicationName = typeof(LongTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myLong = 69;
            var instance = new FooLongPublic { MyLong = myLong };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooLongPublic>(reader);

            decrypted.MyLong.ShouldBe(myLong);
        }

        private class FooLongPublic
        {
            [Encrypt]
            public long MyLong { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new JsonCryptionContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myLong = 69;
            var instance = new FooLongPrivate(myLong);

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooLongPrivate>(reader);

            decrypted.GetMyLong().ShouldBe(myLong);
        }

        private class FooLongPrivate
        {
            [Encrypt]
            [JsonProperty]
            private long _myLong;
            public long GetMyLong() => _myLong;

            public FooLongPrivate(long myLong)
            {
                _myLong = myLong;
            }
        }
    }
}
