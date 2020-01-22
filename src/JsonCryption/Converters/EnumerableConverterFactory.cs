using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal class EnumerableConverterFactory : EncryptedConverterFactory
    {
        private readonly Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter> _cachedConverters;
        
        public EnumerableConverterFactory(Encrypter encrypter, JsonSerializerOptions options) : base(encrypter, options)
        {
            _cachedConverters = new Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter>();
        }

        public override bool CanConvert(Type typeToConvert) => CanConvertType(typeToConvert);
        public static bool CanConvertType(Type typeToConvert) => typeToConvert.IsArray || IsEnumerable(typeToConvert);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            options ??= _options;
            
            if (_cachedConverters.TryGetValue((typeToConvert, options), out var converter))
                return converter;

            converter = CreateSpecificConverter(typeToConvert, options);

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

        private JsonConverter CreateSpecificConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsArray)
                return CreateTypedConverter(typeof(EncryptedArrayConverter<>), typeToConvert.GetElementType(), options);

            else
                return CreateTypedConverter(typeof(EnumerableConverter), typeToConvert.GetGenericArguments()[0], options);
        }

        private JsonConverter CreateTypedConverter(Type converterType, Type elementType, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                    converterType.MakeGenericType(elementType),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { _encrypter, options },
                    culture: null);
        }

        private static bool IsEnumerable(Type type)
        {
            if (type.IsArray)
                return true;

            if (!type.IsGenericType)
                return false;

            return typeof(IEnumerable<>).MakeGenericType(type.GenericTypeArguments).IsAssignableFrom(type);
        }
    }
}
