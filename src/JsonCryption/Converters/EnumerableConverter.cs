using JsonCryption.Encrypters;
using System;
using System.Collections;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class EnumerableConverter : EncryptedConverter<IEnumerable>
    {
        public EnumerableConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override IEnumerable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
