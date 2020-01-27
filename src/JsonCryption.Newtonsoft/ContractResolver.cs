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
            };

        private static readonly Dictionary<Type, Func<IEncryptedConverter, IValueProvider, IValueProvider>> _valueProviderFactories
            = new Dictionary<Type, Func<IEncryptedConverter, IValueProvider, IValueProvider>>
            {
                { typeof(bool), (converter, innerProvider) => new BoolValueProvider((IEncryptedConverter<bool>)converter, innerProvider) },
                { typeof(byte), (converter, innerProvider) => new ByteValueProvider((IEncryptedConverter<byte>)converter, innerProvider) },
                { typeof(byte[]), (converter, innerProvider) => new ByteArrayValueProvider((IEncryptedConverter<byte[]>)converter, innerProvider) },
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

            return _jsonConverterFactories[type].Invoke(_dataProtectionProvider);
        }

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
            => member.ShouldEncrypt() ? CreateEncryptedValueProvider(member) : base.CreateMemberValueProvider(member);

        private IValueProvider CreateEncryptedValueProvider(MemberInfo member)
        {
            var type = member.GetUnderlyingType();
            var converter = _jsonConverterFactories[type].Invoke(_dataProtectionProvider) as IEncryptedConverter;
            var innerProvider = base.CreateMemberValueProvider(member);
            return _valueProviderFactories[type].Invoke(converter, innerProvider);
        }
    }
}
