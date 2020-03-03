using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Utf8Json;

namespace JsonCryption.Utf8Json
{
    internal sealed class EncryptedFormatter<T> : IJsonFormatter<T>
    {
        private readonly IDataProtector _dataProtector;
        private readonly IJsonFormatterResolver _fallbackResolver;
        private readonly Dictionary<string, ExtendedMemberInfo> _memberInfos;
        private readonly ExtendedMemberInfo[] _constructorNonInitializedMembers;
        private readonly ConstructorInfo _constructor;

        private static readonly Type CachedType = typeof(T);

        internal EncryptedFormatter(IDataProtector dataProtector, IJsonFormatterResolver fallbackResolver, ExtendedMemberInfo[] extendedMemberInfos)
        {
            _dataProtector = dataProtector;
            _fallbackResolver = fallbackResolver;

            _memberInfos = extendedMemberInfos
                .Select(m => (m.Name, Value: m))
                .ToDictionary(x => x.Name, x => x.Value);

            var constructorResolver = new ConstructorResolver();
            _constructor = constructorResolver.GetConstructor(extendedMemberInfos, CachedType);

            var argumentNames = new HashSet<string>(_constructor.GetParameters().Select(p => p.Name.ToLowerInvariant()));
            var constructorInitializedMembers = extendedMemberInfos.Where(m => argumentNames.Contains(m.Name.ToLowerInvariant())).ToArray();
            _constructorNonInitializedMembers = extendedMemberInfos.Except(constructorInitializedMembers).ToArray();
        }

        public T Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
                return default;

            if (!reader.ReadIsBeginObject())
                throw new SerializationException();

            var values = new Dictionary<ExtendedMemberInfo, dynamic>();
            foreach (var memberInfo in _memberInfos.Values)
            {
                values[memberInfo] = null;
            }

            while (!reader.ReadIsEndObject() && reader.GetCurrentJsonToken() != JsonToken.None)
            {
                var propName = reader.ReadPropertyName();

                var memberInfo = _memberInfos[propName];
                var memberValue = memberInfo.ShouldEncrypt
                    ? ReadEncrypted(ref reader, memberInfo, _dataProtector, _fallbackResolver)
                    : ReadNormal(ref reader, memberInfo, _fallbackResolver);
                values[memberInfo] = memberValue;

                if (!reader.ReadIsEndObject())
                    reader.ReadIsValueSeparatorWithVerify();
            }

            return BuildObjectFromValues(values);
        }

        public void Serialize(ref JsonWriter writer, T value, IJsonFormatterResolver formatterResolver)
        {
            writer.WriteBeginObject();

            if (_memberInfos.Any())
            {
                WriteDataMember(ref writer, value, _memberInfos.First().Value, _fallbackResolver, _dataProtector);
            }
            foreach (var memberInfo in _memberInfos.Skip(1))
            {
                writer.WriteValueSeparator();
                WriteDataMember(ref writer, value, memberInfo.Value, _fallbackResolver, _dataProtector);
            }

            writer.WriteEndObject();
        }

        private T BuildObjectFromValues(Dictionary<ExtendedMemberInfo, dynamic> values)
        {
            // get object from constructor and any params
            var tValue = ConstructObject(values, _constructor);

            // assign any additional serialized params not set via constructor
            foreach (var member in _constructorNonInitializedMembers)
            {
                var val = values[member];
                var converted = member.Converter(val);
                member.Setter(tValue, converted);
            }

            return tValue;
        }

        private T ConstructObject(Dictionary<ExtendedMemberInfo, dynamic> values, ConstructorInfo constructor)
        {
            var valuesByName = values.ToDictionary(v => v.Key.Name.ToLowerInvariant());

            var arguments = constructor
                .GetParameters()
                .Select(p => p.Name.ToLowerInvariant())
                .Select(n => valuesByName.ContainsKey(n) ? valuesByName[n] : default)
                .ToArray();

            return (T)constructor.Invoke(arguments.Select(a => a.Value).ToArray());
        }

        private static object ReadEncrypted(ref JsonReader reader, ExtendedMemberInfo memberInfo, IDataProtector dataProtector, IJsonFormatterResolver fallbackResolver)
        {
            var ciphertext = reader.ReadString();
            var plaintext = dataProtector.Unprotect(ciphertext);

            dynamic deserialized = memberInfo.EncryptedDeserializer(plaintext, fallbackResolver);
            return deserialized;
        }

        private static dynamic ReadNormal(ref JsonReader reader, ExtendedMemberInfo memberInfo, IJsonFormatterResolver fallbackResolver)
        {
            if (!memberInfo.HasNestedEncryptedMembers)
                return JsonSerializer.Deserialize<dynamic>(ref reader, fallbackResolver);

            var localReader = new JsonReader(Encoding.UTF8.GetBytes(reader.ReadString()));
            return memberInfo.TypedDeserializer(ref localReader, JsonSerializer.DefaultResolver);
        }

        private static void WriteDataMember(ref JsonWriter writer, T value, ExtendedMemberInfo memberInfo, IJsonFormatterResolver fallbackResolver, IDataProtector dataProtector)
        {
            writer.WritePropertyName(memberInfo.Name);
            object memberValue = memberInfo.Getter(value);
            var valueToSerialize = memberInfo.ShouldEncrypt
                ? BuildEncryptedValue(memberValue, memberInfo, fallbackResolver, dataProtector)
                : BuildNormalValue(memberValue, memberInfo, memberInfo.HasNestedEncryptedMembers);
            JsonSerializer.Serialize(ref writer, valueToSerialize, fallbackResolver);
        }

        private static string BuildEncryptedValue(dynamic memberValue, ExtendedMemberInfo memberInfo, IJsonFormatterResolver fallbackResolver, IDataProtector dataProtector)
        {
            var localWriter = new JsonWriter();
            memberInfo.FallbackSerializer(ref localWriter, memberValue, fallbackResolver);
            return dataProtector.Protect(localWriter.ToString());
        }

        private static object BuildNormalValue(dynamic memberValue, ExtendedMemberInfo memberInfo, bool hasNestedEncryptedMembers)
        {
            if (!hasNestedEncryptedMembers)
                return memberValue;

            var localWriter = new JsonWriter();
            memberInfo.FallbackSerializer(ref localWriter, memberValue, JsonSerializer.DefaultResolver);
            return localWriter.ToString();
        }
    }
}
