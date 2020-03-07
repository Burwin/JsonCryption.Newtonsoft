using JsonCryption.Newtonsoft.Extensions;
using JsonCryption.Newtonsoft.JsonConverters;
using JsonCryption.Newtonsoft.ValueProviders;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonCryption.Newtonsoft
{
    /// <summary>
    /// When set as the <see cref="IContractResolver"/> for a <see cref="JsonSerializer"/>, and
    /// properly configured, it enables the automatic encryption/decryption of fields and properties
    /// decorated with <see cref="EncryptAttribute"/> during serialization/deserialization
    /// </summary>
    public sealed class EncryptedContractResolver : DefaultContractResolver
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly Dictionary<Type, IEncryptedConverter> _converters;
        private readonly IEncryptedConverter _genericConverter;

        private static readonly ByteConverterRegistry _byteConverterRegistry = new ByteConverterRegistry();

        private static readonly Dictionary<Type, Func<IDataProtectionProvider, JsonConverter>> _jsonConverterFactories
            = new Dictionary<Type, Func<IDataProtectionProvider, JsonConverter>>
            {
                { typeof(bool), dataProtectionProvider => new BoolConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<bool>()) },
                { typeof(byte), dataProtectionProvider => new ByteConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<byte>()) },
                { typeof(byte[]), dataProtectionProvider => new ByteArrayConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<byte[]>()) },
                { typeof(char), dataProtectionProvider => new CharConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<char>()) },
                { typeof(DateTime), dataProtectionProvider => new DateTimeConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<DateTime>()) },
                { typeof(DateTimeOffset), dataProtectionProvider => new DateTimeOffsetConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<DateTimeOffset>()) },
                { typeof(decimal), dataProtectionProvider => new DecimalConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<decimal>()) },
                { typeof(double), dataProtectionProvider => new DoubleConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<double>()) },
                { typeof(float), dataProtectionProvider => new FloatConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<float>()) },
                { typeof(Guid), dataProtectionProvider => new GuidConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<Guid>()) },
                { typeof(int), dataProtectionProvider => new IntConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<int>()) },
                { typeof(long), dataProtectionProvider => new LongConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<long>()) },
                { typeof(sbyte), dataProtectionProvider => new SByteConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<sbyte>()) },
                { typeof(short), dataProtectionProvider => new ShortConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<short>()) },
                { typeof(string), dataProtectionProvider => new StringConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<string>()) },
                { typeof(uint), dataProtectionProvider => new UIntConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<uint>()) },
                { typeof(ulong), dataProtectionProvider => new ULongConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<ulong>()) },
                { typeof(ushort), dataProtectionProvider => new UShortConverter(dataProtectionProvider, _byteConverterRegistry.GetByteConverter<ushort>()) },
            };

        private static readonly Dictionary<Type, Func<IEncryptedConverter, IValueProvider, IValueProvider>> _valueProviderFactories
            = new Dictionary<Type, Func<IEncryptedConverter, IValueProvider, IValueProvider>>
            {
                { typeof(bool), (converter, innerProvider) => new BoolValueProvider((IEncryptedConverter<bool>)converter, innerProvider) },
                { typeof(byte), (converter, innerProvider) => new ByteValueProvider((IEncryptedConverter<byte>)converter, innerProvider) },
                { typeof(byte[]), (converter, innerProvider) => new ByteArrayValueProvider((IEncryptedConverter<byte[]>)converter, innerProvider) },
                { typeof(char), (converter, innerProvider) => new CharValueProvider((IEncryptedConverter<char>)converter, innerProvider) },
                { typeof(DateTime), (converter, innerProvider) => new DateTimeValueProvider((IEncryptedConverter<DateTime>)converter, innerProvider) },
                { typeof(DateTimeOffset), (converter, innerProvider) => new DateTimeOffsetValueProvider((IEncryptedConverter<DateTimeOffset>)converter, innerProvider) },
                { typeof(decimal), (converter, innerProvider) => new DecimalValueProvider((IEncryptedConverter<decimal>)converter, innerProvider) },
                { typeof(double), (converter, innerProvider) => new DoubleValueProvider((IEncryptedConverter<double>)converter, innerProvider) },
                { typeof(float), (converter, innerProvider) => new FloatValueProvider((IEncryptedConverter<float>)converter, innerProvider) },
                { typeof(Guid), (converter, innerProvider) => new GuidValueProvider((IEncryptedConverter<Guid>)converter, innerProvider) },
                { typeof(int), (converter, innerProvider) => new IntValueProvider((IEncryptedConverter<int>)converter, innerProvider) },
                { typeof(long), (converter, innerProvider) => new LongValueProvider((IEncryptedConverter<long>)converter, innerProvider) },
                { typeof(sbyte), (converter, innerProvider) => new SByteValueProvider((IEncryptedConverter<sbyte>)converter, innerProvider) },
                { typeof(short), (converter, innerProvider) => new ShortValueProvider((IEncryptedConverter<short>)converter, innerProvider) },
                { typeof(string), (converter, innerProvider) => new StringValueProvider((IEncryptedConverter<string>)converter, innerProvider) },
                { typeof(uint), (converter, innerProvider) => new UIntValueProvider((IEncryptedConverter<uint>)converter, innerProvider) },
                { typeof(ulong), (converter, innerProvider) => new ULongValueProvider((IEncryptedConverter<ulong>)converter, innerProvider) },
                { typeof(ushort), (converter, innerProvider) => new UShortValueProvider((IEncryptedConverter<ushort>)converter, innerProvider) },
            };

        /// <summary>
        /// Creates a new <see cref="EncryptedContractResolver"/> with the root <see cref="IDataProtectionProvider"/>
        /// </summary>
        /// <param name="dataProtectionProvider"></param>
        public EncryptedContractResolver(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _converters = new Dictionary<Type, IEncryptedConverter>();
            _genericConverter = new GenericConverter(dataProtectionProvider.CreateProtector(typeof(GenericConverter).FullName));
        }

        /// <summary>
        /// Resolves a <see cref="JsonContract"/> for the given Type, enabling encryption/decryption if necessary
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);
            if (contract is JsonObjectContract objectContract)
            {
                foreach (var property in objectContract.Properties.Where(p => p.Converter is IEncryptedConverter))
                {
                    property.PropertyType = typeof(object);
                }
            }

            return contract;
        }

        /// <summary>
        /// Creates a <see cref="JsonProperty"/> from the given <paramref name="memberSerialization"/> and <paramref name="member"/>,
        /// assigning an <see cref="IEncryptedConverter"/> as the property's Converter if the <paramref name="member"/> is
        /// decorated with an <see cref="EncryptAttribute"/>
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.ShouldEncrypt())
            {
                property.Converter = ResolveEncryptedConverter(property.PropertyType, property.Converter);
            }

            return property;
        }

        private JsonConverter ResolveEncryptedConverter(Type type, JsonConverter converter)
        {
            if (converter is IEncryptedConverter)
                return converter;

            if (_converters.TryGetValue(type, out var encryptedConverter))
                return (JsonConverter)encryptedConverter;

            encryptedConverter = CreateEncryptedConverter(type, _dataProtectionProvider);
            _converters.Add(type, encryptedConverter);
            return (JsonConverter)encryptedConverter;
        }

        /// <summary>
        /// Creates an encrypted or normal <see cref="IValueProvider"/> depending on whether the
        /// <paramref name="member"/> is decorated with an <see cref="EncryptAttribute"/> or not
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
            => member.ShouldEncrypt() ? CreateEncryptedValueProvider(member) : base.CreateMemberValueProvider(member);

        private IValueProvider CreateEncryptedValueProvider(MemberInfo member)
        {
            var type = member.GetUnderlyingType();
            var converter = CreateEncryptedConverter(type, _dataProtectionProvider);
            var innerProvider = base.CreateMemberValueProvider(member);
            var valueProvider = ResolveValueProvider(type, converter, innerProvider);
            return valueProvider;
        }

        private IEncryptedConverter CreateEncryptedConverter(Type type, IDataProtectionProvider dataProtectionProvider)
        {
            if (_jsonConverterFactories.TryGetValue(type, out var factory))
                return factory.Invoke(dataProtectionProvider) as IEncryptedConverter;

            if (type.IsArray)
                return ResolveArrayEncryptedConverter(type, dataProtectionProvider);

            if (type.IsIDictionary())
                return ResolveDictionaryEncryptedConverter(type, dataProtectionProvider);

            if (type.IsEnum)
                return ResolveEnumEncryptedConverter(type, dataProtectionProvider);

            return _genericConverter;
        }

        private IEncryptedConverter ResolveEnumEncryptedConverter(Type type, IDataProtectionProvider dataProtectionProvider)
        {
            var byteConverter = _byteConverterRegistry.GetOrCreateEnumConverter(type);

            return (IEncryptedConverter)Activator.CreateInstance(
                typeof(EnumConverter<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { dataProtectionProvider, byteConverter },
                culture: null);
        }

        private IEncryptedConverter ResolveDictionaryEncryptedConverter(Type type, IDataProtectionProvider dataProtectionProvider)
        {
            return (IEncryptedConverter)Activator.CreateInstance(
                typeof(EncryptedDictionaryConverter<,>).MakeGenericType(type.GenericTypeArguments),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { dataProtectionProvider },
                culture: null);
        }

        private IEncryptedConverter ResolveArrayEncryptedConverter(Type type, IDataProtectionProvider dataProtectionProvider)
        {
            return (IEncryptedConverter)Activator.CreateInstance(
                typeof(EncryptedArrayConverter<>).MakeGenericType(type.GetElementType()),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { dataProtectionProvider },
                culture: null);
        }

        private IValueProvider ResolveValueProvider(Type type, IEncryptedConverter converter, IValueProvider innerProvider)
        {
            if (_valueProviderFactories.TryGetValue(type, out var factory))
                return factory.Invoke(converter, innerProvider);

            if (type.IsArray)
                return ResolveArrayValueProvider(type, converter, innerProvider);

            if (type.IsIDictionary())
                return ResolveDictionaryValueProvider(type, converter, innerProvider);

            if (type.IsEnum)
                return ResolveEnumValueProvider(type, converter, innerProvider);

            return ResolveGenericValueProvider(type, innerProvider);
        }

        private IValueProvider ResolveGenericValueProvider(Type type, IValueProvider innerProvider)
        {
            return (IValueProvider)Activator.CreateInstance(
                typeof(GenericValueProvider<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _genericConverter, innerProvider, new JsonSerializer() },
                culture: null);
        }

        private IValueProvider ResolveEnumValueProvider(Type type, IEncryptedConverter converter, IValueProvider innerProvider)
        {
            return (IValueProvider)Activator.CreateInstance(
                typeof(EnumValueProvider<>).MakeGenericType(type),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { converter, innerProvider },
                culture: null);
        }

        private IValueProvider ResolveDictionaryValueProvider(Type type, IEncryptedConverter converter, IValueProvider innerProvider)
        {
            var genericArgTypes = type.GenericTypeArguments;
            var defaultValue = Activator.CreateInstance(
                typeof(Dictionary<,>).MakeGenericType(genericArgTypes),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);
            
            return (IValueProvider)Activator.CreateInstance(
                typeof(EncryptedDictionaryValueProvider<,>).MakeGenericType(genericArgTypes),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { converter, innerProvider, defaultValue, new JsonSerializer() },
                culture: null);
        }

        private IValueProvider ResolveArrayValueProvider(Type type, IEncryptedConverter converter, IValueProvider innerProvider)
        {
            var elementType = type.GetElementType();
            
            return (IValueProvider)Activator.CreateInstance(
                typeof(EncryptedArrayValueProvider<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { converter, innerProvider, Array.CreateInstance(elementType, 0), new JsonSerializer() },
                culture: null);
        }
    }
}
