using JsonCryption.ByteConverters;
using JsonCryption.Converters;
using JsonCryption.System.Text.Json.ByteConverters;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.System.Text.Json
{
    public class Coordinator
    {
        private readonly Dictionary<Type, Func<IDataProtectionProvider, JsonSerializerOptions, JsonConverter>> _defaultConverterFactories;
        private readonly Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverterFactory> _specialConverterFactories;
        private readonly Dictionary<Type, JsonConverter> _converters;
        private static readonly ByteConverterRegistry _byteConverterRegistry = new ByteConverterRegistry();
        
        public static Coordinator Singleton { get; private set; }
        public CoordinatorOptions Options { get; }
        internal Func<IDataProtectionProvider> _dataProtectionProviderFactory => Options.DataProtectionProviderFactory;
        public JsonSerializerOptions JsonSerializerOptions => Options.JsonSerializerOptions;

        private Coordinator(CoordinatorOptions options)
        {
            Options = options;

            _specialConverterFactories = new Dictionary<(Type Type, JsonSerializerOptions Options), JsonConverterFactory>();

            _defaultConverterFactories = BuildConverterFactories();
            _converters = _defaultConverterFactories
                .Select(kvp => (kvp.Key, Converter: kvp.Value.Invoke(_dataProtectionProviderFactory.Invoke(), JsonSerializerOptions)))
                .ToDictionary(x => x.Key, x => x.Converter);
        }

        internal JsonConverter GetConverter(Type typeToConvert, JsonSerializerOptions options)
            => options is null ? _converters[typeToConvert] : CreateConverter(typeToConvert, options);

        internal bool HasConverter(Type typeToConvert)
        {
            if (typeToConvert.IsEnum)
                return true;

            if (EnumerableConverterFactory.CanConvertType(typeToConvert))
                return true;

            if (KeyValuePairConverterFactory.CanConvertType(typeToConvert))
                return true;

            return _converters.ContainsKey(typeToConvert);
        }

        public static Coordinator Configure(Action<CoordinatorOptions> configure)
        {
            var options = CoordinatorOptions.Empty;
            configure(options);
            options.Validate();

            Singleton = new Coordinator(options);
            return Singleton;
        }

        public static Coordinator ConfigureDefault(string applicationName)
            => Configure(options => options.DataProtectionProviderFactory = () => DataProtectionProvider.Create(applicationName));

        #region Private Helpers

        private JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            options ??= JsonSerializerOptions;

            if (typeToConvert.IsEnum)
                return CreateEnumConverter(typeToConvert, options);

            // byte arrays are handled more natively
            if (EnumerableConverterFactory.CanConvertType(typeToConvert) && typeToConvert != typeof(byte[]))
                return CreateEnumerableConverter(typeToConvert, options);

            if (KeyValuePairConverterFactory.CanConvertType(typeToConvert))
                return CreateKeyValuePairConverter(typeToConvert, options);

            if (!_defaultConverterFactories.TryGetValue(typeToConvert, out var factoryMethod))
                throw new InvalidOperationException($"No Converter for type {typeToConvert.FullName}");

            return factoryMethod.Invoke(_dataProtectionProviderFactory.Invoke(), options);
        }

        private JsonConverter CreateKeyValuePairConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var key = (typeToConvert, options);
            if (!_specialConverterFactories.TryGetValue(key, out var factory))
                _specialConverterFactories[key] = factory = new KeyValuePairConverterFactory(_dataProtectionProviderFactory.Invoke(), options);

            return factory.CreateConverter(typeToConvert, options);
        }

        internal static object GetByteConverter(Type type)
        {
            if (_byteConverterRegistry.TryGetByteConverter(type, out var converter))
                return converter;

            if (type.IsEnum)
                converter = _byteConverterRegistry.GetOrCreateEnumConverter(type);

            if (type.IsKeyValuePair())
                converter = GetKeyValuePairByteConverter(type);

            return converter;
        }

        internal static IByteConverter<T> GetByteConverter<T>() => (IByteConverter<T>)GetByteConverter(typeof(T));

        private static object GetKeyValuePairByteConverter(Type type)
        {
            var converter = Activator.CreateInstance(
                typeof(KeyValuePairByteConverter<,>).MakeGenericType(type.GenericTypeArguments),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            _byteConverterRegistry.Add(type, converter);

            return converter;
        }

        private JsonConverter CreateEnumerableConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var key = (typeToConvert, options);
            if (!_specialConverterFactories.TryGetValue(key, out var factory))
                _specialConverterFactories[key] = factory = new EnumerableConverterFactory(_dataProtectionProviderFactory.Invoke(), options);

            return factory.CreateConverter(typeToConvert, options);
        }

        private JsonConverter CreateEnumConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var key = (typeToConvert, options);
            if (!_specialConverterFactories.TryGetValue(key, out var factory))
                _specialConverterFactories[key] = factory = new EnumConverterFactory(_dataProtectionProviderFactory.Invoke(), options);

            return factory.CreateConverter(typeToConvert, options);
        }

        private Dictionary<Type, Func<IDataProtectionProvider, JsonSerializerOptions, JsonConverter>> BuildConverterFactories()
        {
            return new Dictionary<Type, Func<IDataProtectionProvider, JsonSerializerOptions, JsonConverter>>
            {
                {typeof(bool), (encrypter, options) => new BooleanConverter(encrypter, options) },
                {typeof(byte), (encrypter, options) => new ByteConverter(encrypter, options) },
                {typeof(byte[]), (encrypter, options) => new ByteArrayConverter(encrypter, options) },
                {typeof(char), (encrypter, options) => new CharConverter(encrypter, options) },
                {typeof(DateTime), (encrypter, options) => new DateTimeConverter(encrypter, options) },
                {typeof(DateTimeOffset), (encrypter, options) => new DateTimeOffsetConverter(encrypter, options) },
                {typeof(decimal), (encrypter, options) => new DecimalConverter(encrypter, options) },
                {typeof(double), (encrypter, options) => new DoubleConverter(encrypter, options) },
                {typeof(short), (encrypter, options) => new ShortConverter(encrypter, options) },
                {typeof(int), (encrypter, options) => new IntConverter(encrypter, options) },
                {typeof(long), (encrypter, options) => new LongConverter(encrypter, options) },
                {typeof(sbyte), (encrypter, options) => new SByteConverter(encrypter, options) },
                {typeof(float), (encrypter, options) => new FloatConverter(encrypter, options) },
                {typeof(string), (encrypter, options) => new StringConverter(encrypter, options) },
                {typeof(ushort), (encrypter, options) => new UnsignedShortConverter(encrypter, options) },
                {typeof(uint), (encrypter, options) => new UnsignedIntConverter(encrypter, options) },
                {typeof(ulong), (encrypter, options) => new UnsignedLongConverter(encrypter, options) },
                {typeof(Guid), (encrypter, options) => new GuidConverter(encrypter, options) },
            };
        }

        #endregion Private Helpers
    }
}
