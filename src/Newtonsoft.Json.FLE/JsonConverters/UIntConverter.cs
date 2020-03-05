﻿using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class UIntConverter : EncryptedConverter<uint>
    {
        private static readonly string _purpose = typeof(UIntConverter).FullName;
        public UIntConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<uint> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
