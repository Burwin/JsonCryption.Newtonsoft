using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class IntConverter : EncryptedConverter<int>
    {
        public IntConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override int FromBytes(byte[] bytes) => BitConverter.ToInt32(bytes, 0);
        protected override byte[] ToBytes(int value) => BitConverter.GetBytes(value);
    }
}
