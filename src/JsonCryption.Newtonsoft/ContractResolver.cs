using JsonCryption.Newtonsoft.Encryption;
using JsonCryption.Newtonsoft.ValueProviders;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace JsonCryption.Newtonsoft
{
    public sealed class ContractResolver : DefaultContractResolver
    {
        private readonly Encrypter _encrypter;

        public ContractResolver(Encrypter encrypter)
        {
            _encrypter = encrypter;
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

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if (member.ShouldEncrypt())
                return new BoolValueProvider(_encrypter,base.CreateMemberValueProvider(member));
            
            return base.CreateMemberValueProvider(member);
        }

        //protected override JsonConverter ResolveContractConverter(Type objectType)
        //{
        //    if (objectType == typeof(bool))
        //        return new BoolConverter(_encrypter);

        //    return base.ResolveContractConverter(objectType);
        //}
    }

    internal static class MemberInfoExtensions
    {
        public static bool ShouldEncrypt(this MemberInfo info) => info.GetCustomAttribute<EncryptAttribute>(inherit: true) != null;
    }
}
