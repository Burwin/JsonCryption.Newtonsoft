using JsonCryption.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class DateTimeOffsetConverter : EncryptedConverter<DateTimeOffset>
    {
        private static readonly string _purpose = typeof(DateTimeOffsetConverter).FullName;
        public DateTimeOffsetConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new DateTimeOffsetByteConverter())
        {
        }
    }
}
