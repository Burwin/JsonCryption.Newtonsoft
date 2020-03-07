﻿using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class EnumConverter<T> : EncryptedConverter<T>
    {
        private static readonly string _purpose = typeof(EnumConverter<T>).FullName;

        public EnumConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<T> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
