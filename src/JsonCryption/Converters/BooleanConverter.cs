using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class BooleanConverter : EncryptedConverter<bool>
    {
        public BooleanConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
