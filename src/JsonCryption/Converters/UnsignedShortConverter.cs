using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedShortConverter : EncryptedConverter<ushort>
    {
        public UnsignedShortConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
