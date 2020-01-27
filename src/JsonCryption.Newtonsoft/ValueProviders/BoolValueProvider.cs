using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class BoolValueProvider : EncryptedValueProvider<bool>
    {
        private static readonly string _purpose = typeof(BoolValueProvider).FullName;
        
        public BoolValueProvider(IDataProtectionProvider dataProtectionProvider, IValueProvider innerProvider)
            : base(dataProtectionProvider.CreateProtector(_purpose), innerProvider)
        {
        }

        public override bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public override byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
