using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class BooleanConverter : EncryptedConverter<bool>
    {
        public BooleanConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public override byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
