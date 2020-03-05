﻿using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class DecimalConverter : EncryptedConverter<decimal>
    {
        private static readonly string _purpose = typeof(DecimalConverter).FullName;
        public DecimalConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<decimal> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
