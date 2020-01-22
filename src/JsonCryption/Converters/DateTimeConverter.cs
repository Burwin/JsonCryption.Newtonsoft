using JsonCryption.Encrypters;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeConverter : EncryptedConverter<DateTime>
    {
        public DateTimeConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override DateTime FromBytes(byte[] bytes)
        {
            var dateAsLong = BitConverter.ToInt64(bytes, 0);
            return DateTime.FromBinary(dateAsLong);
        }

        public override byte[] ToBytes(DateTime value) => BitConverter.GetBytes(value.ToBinary());
    }
}
