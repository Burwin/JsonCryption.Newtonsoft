using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedIntConverter : EncryptedConverter<uint>
    {
        public UnsignedIntConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override uint FromBytes(byte[] bytes) => BitConverter.ToUInt32(bytes, 0);
        public override byte[] ToBytes(uint value) => BitConverter.GetBytes(value);
    }
}
