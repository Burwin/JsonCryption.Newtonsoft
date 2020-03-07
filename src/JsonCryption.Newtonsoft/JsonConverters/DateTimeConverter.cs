﻿using Microsoft.AspNetCore.DataProtection;
using System;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class DateTimeConverter : EncryptedConverter<DateTime>
    {
        private static readonly string _purpose = typeof(DateTimeConverter).FullName;

        public DateTimeConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<DateTime> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
