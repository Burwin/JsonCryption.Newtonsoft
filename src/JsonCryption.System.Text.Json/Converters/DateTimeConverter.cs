using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class DateTimeConverter : EncryptedConverter<DateTime>
    {
        private static readonly string _purpose = typeof(DateTimeConverter).FullName;

        public DateTimeConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
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
