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

        protected override ulong FromBytes(byte[] bytes) => BitConverter.ToUInt64(bytes, 0);
        protected override byte[] ToBytes(ulong value) => BitConverter.GetBytes(value);
    }
}
