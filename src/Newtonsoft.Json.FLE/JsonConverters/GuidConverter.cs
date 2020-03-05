using Microsoft.AspNetCore.DataProtection;
using System;

namespace Newtonsoft.Json.FLE.JsonConverters
{
    internal sealed class GuidConverter : EncryptedConverter<Guid>
    {
        private static readonly string _purpose = typeof(GuidConverter).FullName;
        public GuidConverter(IDataProtectionProvider dataProtectionProvider, IByteConverter<Guid> byteConverter)
            : base(dataProtectionProvider.CreateProtector(_purpose), byteConverter)
        {
        }
    }
}
