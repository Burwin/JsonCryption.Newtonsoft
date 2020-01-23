using JsonCryption.Newtonsoft.Converters;
using JsonCryption.Newtonsoft.Encrypters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft
{
    public sealed class ContractResolver : DefaultContractResolver
    {
        private readonly Encrypter _encrypter;

        public ContractResolver(Encrypter encrypter)
        {
            _encrypter = encrypter;
        }

        public override JsonContract ResolveContract(Type type) => base.ResolveContract(type);

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (objectType == typeof(bool))
                return new BoolConverter(_encrypter);

            return base.ResolveContractConverter(objectType);
        }
    }
}
