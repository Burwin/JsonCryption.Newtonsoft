﻿using Microsoft.AspNetCore.DataProtection;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class ByteArrayConverter : EncryptedConverter<byte[]>
    {
        private static readonly string _purpose = typeof(ByteArrayConverter).FullName;

        public ByteArrayConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<byte[]> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
