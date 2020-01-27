using JsonCryption.Newtonsoft.JsonConverters;
using JsonCryption.Newtonsoft.ValueProviders;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace JsonCryption.Newtonsoft
{
    public sealed class ContractResolver : DefaultContractResolver
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

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
            
            if (type == typeof(bool))
                return new BoolConverter(_dataProtectionProvider);

            throw new NotImplementedException();
        }

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if (member.ShouldEncrypt())
                return new BoolValueProvider(new BoolConverter(_dataProtectionProvider), base.CreateMemberValueProvider(member));
            
            return base.CreateMemberValueProvider(member);
        }
    }
}
