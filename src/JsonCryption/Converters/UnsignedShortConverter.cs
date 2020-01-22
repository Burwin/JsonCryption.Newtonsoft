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

        public override ushort FromBytes(byte[] bytes) => BitConverter.ToUInt16(bytes, 0);
        public override byte[] ToBytes(ushort value) => BitConverter.GetBytes(value);
    }
}
