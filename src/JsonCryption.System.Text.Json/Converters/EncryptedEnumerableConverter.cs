using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonCryption.Converters
{
    internal sealed class EncryptedEnumerableConverter<T> : JsonConverter<IEnumerable<T>>
    {
        private readonly IDataProtector _dataProtector;
        private readonly JsonSerializerOptions _options;
        private readonly EncryptedArrayConverter<T> _arrayConverter;
        private readonly Type _elementType;
        private readonly Type _arrayType;

        public EncryptedEnumerableConverter(IDataProtector dataProtector, JsonSerializerOptions options, EncryptedArrayConverter<T> arrayConverter)
        {
            _dataProtector = dataProtector;
            _options = options;
            _arrayConverter = arrayConverter;

            _elementType = typeof(T);
            _arrayType = _elementType.MakeArrayType();
        }

        public override IEnumerable<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => _arrayConverter.Read(ref reader, _arrayType, options ?? _options);

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
            => _arrayConverter.Write(writer, value.ToArray(), options ?? _options);
    }
}
