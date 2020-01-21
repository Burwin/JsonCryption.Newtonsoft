using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class UnsignedShortConverter : EncryptedConverter<ushort>
    {
        public UnsignedShortConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override ushort FromBytes(byte[] bytes) => BitConverter.ToUInt16(bytes, 0);
        protected override byte[] ToBytes(ushort value) => BitConverter.GetBytes(value);
    }
}
