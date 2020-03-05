﻿using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ULongConverter : EncryptedConverter<ulong>
    {
        private static readonly string _purpose = typeof(ULongConverter).FullName;
        public ULongConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<ulong> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
