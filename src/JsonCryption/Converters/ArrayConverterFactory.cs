using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal class ArrayConverterFactory : EncryptedConverterFactory
    {
        private readonly Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter> _cachedConverters;
        
        public ArrayConverterFactory(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
            _cachedConverters = new Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter>();
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsArray;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (_cachedConverters.TryGetValue((typeToConvert, options), out var converter))
                return converter;

            var elementType = typeToConvert.GetElementType();

            converter = (JsonConverter)Activator.CreateInstance(
                    typeof(EncryptedArrayConverter<>).MakeGenericType(elementType),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { _encrypter, _options },
                    culture: null);

            _cachedConverters[(typeToConvert, options)] = converter;

            return converter;
        }
    }
}
