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

        protected override Guid FromBytes(byte[] bytes) => new Guid(bytes);
        protected override byte[] ToBytes(Guid value) => value.ToByteArray();
    }
}
