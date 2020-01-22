using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class SByteConverter : EncryptedConverter<sbyte>
    {
        public SByteConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override sbyte FromBytes(byte[] bytes) => (sbyte)bytes[0];
        public override byte[] ToBytes(sbyte value) => BitConverter.GetBytes(value);
    }
}
