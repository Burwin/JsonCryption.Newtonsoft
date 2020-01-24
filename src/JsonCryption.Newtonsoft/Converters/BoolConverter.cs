using JsonCryption.Newtonsoft.Encryption;
using System;

namespace JsonCryption.Newtonsoft.Converters
{
    internal sealed class BoolConverter : EncryptedConverter<bool>
    {
        public BoolConverter(Encrypter encrypter) : base(encrypter)
        {
        }

        public override bool FromBytes(byte[] bytes) => BitConverter.ToBoolean(bytes, 0);
        public override byte[] ToBytes(bool value) => BitConverter.GetBytes(value);
    }
}
