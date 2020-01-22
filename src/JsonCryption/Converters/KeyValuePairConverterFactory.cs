using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal sealed class KeyValuePairConverterFactory : EncryptedConverterFactory
    {
        public KeyValuePairConverterFactory(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
        }

        public override bool CanConvert(Type typeToConvert) => CanConvertType(typeToConvert);

        public static bool CanConvertType(Type typeToConvert)
        {
            if (typeToConvert.GenericTypeArguments.Length != 2)
                return false;

            return typeof(KeyValuePair<,>).MakeGenericType(typeToConvert.GenericTypeArguments).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                    typeof(KeyValuePairConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { _encrypter, options },
                    culture: null);
        }
    }
}
