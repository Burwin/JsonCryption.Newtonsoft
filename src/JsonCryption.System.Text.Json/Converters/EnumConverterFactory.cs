using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal sealed class EnumConverterFactory : EncryptedConverterFactory
    {
        private readonly Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter> _cachedConverters;
        private readonly Dictionary<Type, object> _cachedByteConverters;

        public EnumConverterFactory(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider, options)
        {
            _cachedConverters = new Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverter>();
            _cachedByteConverters = new Dictionary<Type, object>();
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (_cachedConverters.TryGetValue((typeToConvert, options), out var converter))
                return converter;

            converter = (JsonConverter)Activator.CreateInstance(
                    typeof(EnumConverter<>).MakeGenericType(typeToConvert),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { _dataProtectionProvider, _options },
                    culture: null);

            _cachedConverters[(typeToConvert, options)] = converter;

            return converter;
        }
    }
}
