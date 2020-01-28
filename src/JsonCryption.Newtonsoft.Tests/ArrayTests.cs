using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace JsonCryption.Newtonsoft.Tests
{
    public class ArrayTests
    {
        private static readonly string ApplicationName = typeof(ArrayTests).AssemblyQualifiedName;

        [Fact]
        public void Public_property_is_encrypted_and_decrypted()
        {
            var dataProtectionProvider = Helpers.GetTestDataProtectionProvider(ApplicationName);
            var contractResolver = new ContractResolver(dataProtectionProvider);
            var serializer = new JsonSerializer() { ContractResolver = contractResolver };

            var myDoubles = new[] { 1.1, 2.2, 3.3 };
            var instance = new FooDoubleArrayPublic { MyDoubles = myDoubles };

            var builder = new StringBuilder();
            using (var textWriter = new StringWriter(builder))
                serializer.Serialize(textWriter, instance);

            var json = builder.ToString();

            json.ShouldNotBe(JsonConvert.SerializeObject(instance));

            using var textReader = new StringReader(json);
            using var reader = new JsonTextReader(textReader);
            var decrypted = serializer.Deserialize<FooDoubleArrayPublic>(reader);

            decrypted.MyDoubles.ShouldBe(myDoubles);
        }

        private class FooDoubleArrayPublic
        {
            [Encrypt]
            public double[] MyDoubles { get; set; }
        }

        [Fact]
        public void Private_field_is_encrypted_and_decrypted()
        {
            throw new NotImplementedException();
        }

        private class FooDoubleArrayPrivate
        {
            private double[] _myInts;
            public double[] GetMyDoubles() => _myInts;

            public FooDoubleArrayPrivate(double[] myInts)
            {
                _myInts = myInts;
            }
        }
    }
}
