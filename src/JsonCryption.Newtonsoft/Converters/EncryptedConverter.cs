using JsonCryption.Newtonsoft.Encrypters;
using Newtonsoft.Json;
using System;

namespace JsonCryption.Newtonsoft
{
    internal abstract class EncryptedConverter<T> : JsonConverter<T>
    {
        protected readonly Encrypter _encrypter;

        protected EncryptedConverter(Encrypter encrypter)
        {
            _encrypter = encrypter;
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception();

            var encrypted = (string)reader.Value;
            var bytes = _encrypter.DecryptToByteArray(encrypted);
            return FromBytes(bytes);
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            var bytes = ToBytes(value);
            var encrypted = _encrypter.Encrypt(bytes);
            writer.WriteValue(encrypted);
        }

        public abstract T FromBytes(byte[] bytes);
        public abstract byte[] ToBytes(T value);
    }
}
