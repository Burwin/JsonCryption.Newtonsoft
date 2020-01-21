using JsonCryption.Encrypters;
using System;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class StringConverter : EncryptedConverter<string>
    {
        public StringConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override string FromBytes(byte[] bytes) => BitConverter.ToString(bytes);
        protected override byte[] ToBytes(string value) => Encoding.UTF8.GetBytes(value);
    }
}
