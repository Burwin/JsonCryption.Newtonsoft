using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class SByteConverter : EncryptedConverter<sbyte>
    {
        private static readonly string _purpose = typeof(SByteConverter).FullName;

        public SByteConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options)
        {
        }

        public override sbyte FromBytes(byte[] bytes) => (sbyte)bytes[0];
        public override byte[] ToBytes(sbyte value) => BitConverter.GetBytes(value);
    }
}
