﻿using Microsoft.AspNetCore.DataProtection;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class ByteConverter : EncryptedConverter<byte>
    {
        private static readonly string _purpose = typeof(ByteConverter).FullName;

        public ByteConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<byte> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
