using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedLongConverter : EncryptedConverter<ulong>
    {
        public UnsignedLongConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override ulong FromBytes(byte[] bytes) => BitConverter.ToUInt64(bytes, 0);
        public override byte[] ToBytes(ulong value) => BitConverter.GetBytes(value);
    }
}
