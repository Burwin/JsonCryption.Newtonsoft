using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class SByteConverter : EncryptedConverter<sbyte>
    {
        public SByteConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override sbyte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
