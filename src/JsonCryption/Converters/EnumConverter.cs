using JsonCryption.Encrypters;
using System;
using System.Text;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class EnumConverter<T> : EncryptedConverter<T>
        where T : struct, Enum
    {
        public EnumConverter(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        protected override T FromBytes(byte[] bytes) => (T)Enum.Parse(typeof(T), Encoding.UTF8.GetString(bytes));
        protected override byte[] ToBytes(T value) => Encoding.UTF8.GetBytes(Enum.GetName(typeof(T), value));
    }
}
