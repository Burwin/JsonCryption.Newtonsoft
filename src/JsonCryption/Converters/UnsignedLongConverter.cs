using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedLongConverter : EncryptedConverter<ulong>
    {
        public UnsignedLongConverter(IEncrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) => throw new NotImplementedException();
    }
}
