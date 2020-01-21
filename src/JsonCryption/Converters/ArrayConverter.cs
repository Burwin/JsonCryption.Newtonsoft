using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ArrayConverter : EncryptedConverter<Array>
    {
        public ArrayConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override Array Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, Array value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
