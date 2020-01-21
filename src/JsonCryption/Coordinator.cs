using JsonCryption.Converters;
using JsonCryption.Encrypters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    public class Coordinator
    {
        private readonly Dictionary<Type, Func<Encrypter, JsonSerializerOptions, JsonConverter>> _converterFactories;
        private readonly Dictionary<Type, JsonConverter> _converters; 
        
        public static Coordinator Singleton { get; private set; }
        public CoordinatorOptions Options { get; }
        internal Encrypter Encrypter => Options.Encrypter;
        public JsonSerializerOptions JsonSerializerOptions => Options.JsonSerializerOptions;

        private Coordinator(CoordinatorOptions options)
        {
            Options = options;

            _converterFactories = BuildConverterFactories();

            _converters = _converterFactories
                .Select(kvp => (kvp.Key, Converter: kvp.Value.Invoke(Encrypter, null)))
                .ToDictionary(x => x.Key, x => x.Converter);
        }

        internal JsonConverter GetConverter(Type typeToConvert, JsonSerializerOptions options)
            => options is null ? _converters[typeToConvert] : CreateConverter(typeToConvert, options);

        private JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (!_converterFactories.TryGetValue(typeToConvert, out var factoryMethod))
                throw new InvalidOperationException($"No Converter for type {typeToConvert.FullName}");

            return factoryMethod.Invoke(Encrypter, options);
        }

        internal bool HasConverter(Type type) => _converters.ContainsKey(type);

        public static Coordinator Configure(Action<CoordinatorOptions> configure)
        {
            var options = CoordinatorOptions.Empty;
            configure(options);
            options.Validate();

            Singleton = new Coordinator(options);
            return Singleton;
        }

        public static Coordinator ConfigureDefault(byte[] key) => Configure(options => options.Encrypter = new AesManagedEncrypter(key));

        private Dictionary<Type, Func<Encrypter, JsonSerializerOptions, JsonConverter>> BuildConverterFactories()
        {
            return new Dictionary<Type, Func<Encrypter, JsonSerializerOptions, JsonConverter>>
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
            };
        }
    }
}
