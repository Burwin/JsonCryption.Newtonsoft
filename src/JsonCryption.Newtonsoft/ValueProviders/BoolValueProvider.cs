using JsonCryption.Newtonsoft.Encryption;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class BoolValueProvider : EncryptedValueProvider<bool>
    {
        public BoolValueProvider(Encrypter encrypter, IValueProvider innerProvider) : base(encrypter, innerProvider)
        {
        }

        public override bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public override byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
