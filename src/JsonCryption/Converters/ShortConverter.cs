using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class ShortConverter : EncryptedConverter<short>
    {
        public ShortConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override short FromBytes(byte[] bytes) => BitConverter.ToInt16(bytes, 0);
        public override byte[] ToBytes(short value) => BitConverter.GetBytes(value);
    }
}
