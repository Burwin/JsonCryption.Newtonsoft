using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class CharConverter : EncryptedConverter<char>
    {
        public CharConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override char FromBytes(byte[] bytes) => BitConverter.ToChar(bytes, 0);
        public override byte[] ToBytes(char value) => BitConverter.GetBytes(value);
    }
}
