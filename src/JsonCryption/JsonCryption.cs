using JsonCryption.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption
{
    public class JsonCryption
    {
        private readonly Dictionary<Type, Func<IEncrypter, JsonSerializerOptions, JsonConverter>> _converterFactories;
        private readonly Dictionary<Type, JsonConverter> _converters; 
        
        public static JsonCryption Singleton { get; private set; }
        public JsonCryptionOptions Options { get; }
        public IEncrypter Encrypter => Options.Encrypter;
        public JsonSerializerOptions JsonSerializerOptions => Options.JsonSerializerOptions;

        private JsonCryption(JsonCryptionOptions options)
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

        public static JsonCryption Create(Action<JsonCryptionOptions> configure)
        {
            if (Singleton != null)
                throw new InvalidOperationException("Can only configure once");

            var options = new JsonCryptionOptions();
            configure(options);

            Singleton = new JsonCryption(options);
            return Singleton;
        }

        private Dictionary<Type, Func<IEncrypter, JsonSerializerOptions, JsonConverter>> BuildConverterFactories()
        {
            return new Dictionary<Type, Func<IEncrypter, JsonSerializerOptions, JsonConverter>>
            {
                {typeof(byte), (encrypter, options) => new ByteConverter(encrypter, options) }
            };
        }
    }
}
