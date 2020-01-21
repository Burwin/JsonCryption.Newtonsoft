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

        protected override IEnumerable FromBytes(byte[] bytes) => throw new NotImplementedException();
        protected override byte[] ToBytes(IEnumerable value) => throw new NotImplementedException();
    }
}
