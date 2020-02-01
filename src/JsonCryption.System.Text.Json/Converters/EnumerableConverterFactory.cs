using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
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
        private readonly Dictionary<Type, object> _cachedByteConverters;
        
        public EnumerableConverterFactory(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider, options)
        {
            _cachedConverters = new Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter>();
            _cachedByteConverters = new Dictionary<Type, object>();
        }

        public override bool CanConvert(Type typeToConvert) => CanConvertType(typeToConvert);
        public static bool CanConvertType(Type typeToConvert) => typeToConvert.IsArray || IsEnumerable(typeToConvert);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            options ??= _options;
            
            if (_cachedConverters.TryGetValue((typeToConvert, options), out var converter))
                return converter;

            converter = CreateSpecificConverter(typeToConvert, options);

            _cachedConverters[(typeToConvert, options)] = converter;

            return converter;
        }

        private JsonConverter CreateSpecificConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsArray)
                return CreateArrayConverter(typeToConvert, options);

            else if (typeToConvert.GenericTypeArguments.Length == 2)
                return CreateDictionaryConverter(typeToConvert, options);

            else
                return CreateEnumerableConverter(typeToConvert, options);
        }

        private JsonConverter CreateEnumerableConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = GetEnumerableGenericArgument(typeToConvert);
            var arrayType = elementType.MakeArrayType();
            var key = (arrayType, options);
            if (!_cachedConverters.TryGetValue(key, out var arrayConverter))
                _cachedConverters[key] = arrayConverter = CreateConverter(arrayType, options);

            return (JsonConverter)Activator.CreateInstance(
                typeof(EncryptedEnumerableConverter<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _dataProtectionProvider, options, arrayConverter },
                culture: null);
        }

        private JsonConverter CreateDictionaryConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = GetEnumerableGenericArgument(typeToConvert);
            var arrayType = elementType.MakeArrayType();
            var key = (arrayType, options);
            if (!_cachedConverters.TryGetValue(key, out var arrayConverter))
                _cachedConverters[key] = arrayConverter = CreateConverter(arrayType, options);

            return (JsonConverter)Activator.CreateInstance(
                typeof(EncryptedDictionaryConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _dataProtectionProvider, options, arrayConverter },
                culture: null);
        }

        private JsonConverter CreateArrayConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.GetElementType();
            var byteConverter = Coordinator.Singleton.GetByteConverter(elementType, options);
            
            return (JsonConverter)Activator.CreateInstance(
                                typeof(EncryptedArrayConverter<>).MakeGenericType(typeToConvert.GetElementType()),
                                BindingFlags.Instance | BindingFlags.Public,
                                binder: null,
                                args: new object[] { _dataProtectionProvider, options, byteConverter },
                                culture: null);
        }

        private Type GetEnumerableGenericArgument(Type typeToConvert)
        {
            // assume Dictionary if 2 generic arguments
            if (typeToConvert.GenericTypeArguments.Length == 2)
                return typeof(KeyValuePair<,>).MakeGenericType(typeToConvert.GenericTypeArguments);

            return typeToConvert.GenericTypeArguments[0];
        }

        private JsonConverter CreateTypedConverter(Type converterType, Type elementType, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                    converterType.MakeGenericType(elementType),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { _dataProtectionProvider, options },
                    culture: null);
        }

        private static bool IsEnumerable(Type type)
        {
            if (type.IsArray)
                return true;

            if (!type.IsGenericType)
                return false;

            // if two generic arguments, assume Dictionary
            if (type.GenericTypeArguments.Length == 2)
            {
                var kvpType = typeof(KeyValuePair<,>).MakeGenericType(type.GenericTypeArguments);
                return typeof(IEnumerable<>).MakeGenericType(kvpType).IsAssignableFrom(type);
            }

            return typeof(IEnumerable<>).MakeGenericType(type.GenericTypeArguments).IsAssignableFrom(type);
        }
    }
}
