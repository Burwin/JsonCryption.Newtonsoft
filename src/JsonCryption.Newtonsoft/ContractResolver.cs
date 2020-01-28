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
    public sealed class ContractResolver : DefaultContractResolver
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        private static readonly Dictionary<Type, Func<IDataProtectionProvider, JsonConverter>> _jsonConverterFactories
            = new Dictionary<Type, Func<IDataProtectionProvider, JsonConverter>>
            {
                { typeof(bool), dataProtectionProvider => new BoolConverter(dataProtectionProvider) },
                { typeof(byte), dataProtectionProvider => new ByteConverter(dataProtectionProvider) },
                { typeof(byte[]), dataProtectionProvider => new ByteArrayConverter(dataProtectionProvider) },
                { typeof(char), dataProtectionProvider => new CharConverter(dataProtectionProvider) },
                { typeof(DateTime), dataProtectionProvider => new DateTimeConverter(dataProtectionProvider) },
                { typeof(DateTimeOffset), dataProtectionProvider => new DateTimeOffsetConverter(dataProtectionProvider) },
                { typeof(decimal), dataProtectionProvider => new DecimalConverter(dataProtectionProvider) },
                { typeof(double), dataProtectionProvider => new DoubleConverter(dataProtectionProvider) },
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
            };

        public ContractResolver(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);
            if (contract is JsonObjectContract objectContract)
            {
                var p = objectContract.Properties.FirstOrDefault();
                if (p?.ValueProvider is EncryptedValueProvider)
                {
                    p.PropertyType = typeof(object);
                }
            }

            return contract;
        }

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

            return CreateEncryptedConverter(type, _dataProtectionProvider) as JsonConverter;
        }

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

            throw new NotImplementedException();
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

            throw new NotImplementedException();
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
