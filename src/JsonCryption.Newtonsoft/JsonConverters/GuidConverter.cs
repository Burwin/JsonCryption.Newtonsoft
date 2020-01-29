using JsonCryption.Newtonsoft.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace JsonCryption.Newtonsoft.JsonConverters
{
    internal sealed class GuidConverter : EncryptedConverter<Guid>
    {
        private static readonly string _purpose = typeof(GuidConverter).FullName;
        public GuidConverter(IDataProtectionProvider dataProtectionProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), new GuidByteConverter())
        {
        }
    }
}
