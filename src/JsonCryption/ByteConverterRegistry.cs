using JsonCryption.ByteConverters;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonCryption
{
    public sealed class ByteConverterRegistry
    {
        private readonly Dictionary<Type, object> _byteConverters = new Dictionary<Type, object>
        {
            { typeof(bool), new BoolByteConverter() },
                { typeof(byte[]), new ByteArrayByteConverter() },
                { typeof(byte), new ByteByteConverter() },
                { typeof(char), new CharByteConverter() },
                { typeof(DateTime), new DateTimeByteConverter() },
                { typeof(DateTimeOffset), new DateTimeOffsetByteConverter() },
                { typeof(decimal), new DecimalByteConverter() },
                { typeof(double), new DoubleByteConverter() },
                { typeof(float), new FloatByteConverter() },
                { typeof(Guid), new GuidByteConverter() },
                { typeof(int), new IntByteConverter() },
                { typeof(long), new LongByteConverter() },
                { typeof(sbyte), new SByteByteConverter() },
                { typeof(short), new ShortByteConverter() },
                { typeof(string), new StringByteConverter() },
                { typeof(uint), new UIntByteConverter() },
                { typeof(ulong), new ULongByteConverter() },
                { typeof(ushort), new UShortByteConverter() },
        };

        public IByteConverter<T> GetByteConverter<T>() => (IByteConverter<T>)_byteConverters[typeof(T)];

        public object GetByteConverter(Type type) => _byteConverters[type];

        public void Add<T>(IByteConverter<T> converter)
        {
            _byteConverters.Add(typeof(T), converter);
        }

        public void Add(Type type, object converter)
        {
            _byteConverters.Add(type, converter);
        }

        public bool TryGetByteConverter(Type type, out object converter) => _byteConverters.TryGetValue(type, out converter);

        public bool TryGetByteConverter<T>(out IByteConverter<T> converter)
        {
            if (_byteConverters.TryGetValue(typeof(T), out var objConverter) && objConverter is IByteConverter<T> typedConverter)
            {
                converter = typedConverter;
                return true;
            }

            converter = null;
            return false;
        }

        public object GetOrCreateEnumConverter(Type type)
        {
            if (TryGetByteConverter(type, out var converter))
                return converter;
            
            converter = Activator.CreateInstance(
                typeof(EnumByteConverter<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            Add(type, converter);

            return converter;
        }
    }
}
