using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class GuidConverter : EncryptedConverter<Guid>
    {
        public GuidConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override Guid FromBytes(byte[] bytes) => throw new NotImplementedException();
        protected override byte[] ToBytes(Guid value) => throw new NotImplementedException();
    }
}
