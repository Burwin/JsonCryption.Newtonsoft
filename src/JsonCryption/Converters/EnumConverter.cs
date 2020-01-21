using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    // TODO: I think this needs to be a factory?
    internal sealed class EnumConverter : EncryptedConverter<Enum>
    {
        public EnumConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override Enum FromBytes(byte[] bytes) => throw new NotImplementedException();
        protected override byte[] ToBytes(Enum value) => throw new NotImplementedException();
    }
}
