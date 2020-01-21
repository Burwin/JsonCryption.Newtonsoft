using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ArrayConverter : EncryptedConverter<Array>
    {
        public ArrayConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override Array FromBytes(byte[] bytes) => throw new NotImplementedException();
        protected override byte[] ToBytes(Array value) => throw new NotImplementedException();
    }
}
