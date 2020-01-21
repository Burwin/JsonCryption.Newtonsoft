using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ByteArrayConverter : EncryptedConverter<byte[]>
    {
        public ByteArrayConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
