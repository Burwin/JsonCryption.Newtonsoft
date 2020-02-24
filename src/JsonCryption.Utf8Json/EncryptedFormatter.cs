using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Utf8Json;

namespace JsonCryption.Utf8Json
{
    public sealed class EncryptedFormatter<T> : IJsonFormatter<T>
    {
        private readonly IDataProtector _dataProtector;
        private readonly IJsonFormatterResolver _fallbackResolver;
        private readonly Dictionary<string, ExtendedMemberInfo> _memberInfos;
        private readonly ExtendedMemberInfo[] _constructorInitializedMembers;
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

            _constructor = GetConstructor(extendedMemberInfos);

            var argumentNames = new HashSet<string>(_constructor.GetParameters().Select(p => p.Name.ToLowerInvariant()));
            _constructorInitializedMembers = extendedMemberInfos.Where(m => argumentNames.Contains(m.Name.ToLowerInvariant())).ToArray();
            _constructorNonInitializedMembers = extendedMemberInfos.Except(_constructorInitializedMembers).ToArray();
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
                var memberValue = memberInfo.ShouldEncrypt ? ReadEncrypted(ref reader, memberInfo, _dataProtector, _fallbackResolver) : ReadNormal(ref reader, memberInfo);
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
                var converted = Convert.ChangeType(val, member.Type);
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
                .Select(n => valuesByName[n])
                .ToArray();

            return (T)constructor.Invoke(arguments.Select(a => a.Value).ToArray());
        }

        private ConstructorInfo GetConstructor(IEnumerable<ExtendedMemberInfo> memberInfos)
        {
            var allConstructors = CachedType
                .GetConstructors()
                .Concat(CachedType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic))
                .ToArray();

            //  1 - look for constructor decorated by SerializationConstructorAttribute
            var constructor = allConstructors.FirstOrDefault(c => c.GetCustomAttribute<SerializationConstructorAttribute>() != null);
            if (constructor != null)
                return constructor;

            //  2 - look for constructor with most matched arguments by name (ignore case)
            (ConstructorInfo ConstructorInfo, int MatchingParameters) mostMatching = ((ConstructorInfo)null, -1);
            var orderedConstructors = allConstructors
                .Select(c => (ConstructorInfo: c, Parameters: c.GetParameters(), ParameterCount: c.GetParameters().Length))
                .OrderByDescending(x => x.ParameterCount);

            var serializedNames = memberInfos
                .Select(k => k.Name.ToLowerInvariant())
                .ToArray();

            foreach (var orderedConstructor in orderedConstructors)
            {
                if (orderedConstructor.ParameterCount < mostMatching.MatchingParameters)
                    break;

                var paramSet = new HashSet<string>(orderedConstructor.Parameters.Select(p => p.Name.ToLowerInvariant()));
                var matchCount = serializedNames.Where(n => paramSet.Contains(n)).Count();

                // check for ambiguous match
                if (matchCount == mostMatching.MatchingParameters)
                    return null;

                if (matchCount > mostMatching.MatchingParameters)
                    mostMatching = (orderedConstructor.ConstructorInfo, matchCount);
            }
            
            if (mostMatching.ConstructorInfo != null)
                return mostMatching.ConstructorInfo;

            //  3 - look for default constructor
            constructor = allConstructors.FirstOrDefault(c => c.IsPublic && !c.GetParameters().Any());
            if (constructor is null)
                throw new Exception($"No suitable constructor found for type {CachedType.Name}");

            return constructor;
        }

        private static object ReadEncrypted(ref JsonReader reader, ExtendedMemberInfo memberInfo, IDataProtector dataProtector, IJsonFormatterResolver fallbackResolver)
        {
            var ciphertext = reader.ReadString();
            var plaintext = dataProtector.Unprotect(ciphertext);

            dynamic deserialized = JsonSerializer.Deserialize<dynamic>(plaintext, fallbackResolver);
            return deserialized;
        }

        private static dynamic ReadNormal(ref JsonReader reader, ExtendedMemberInfo memberInfo) => throw new NotImplementedException();

        private static void WriteDataMember(ref JsonWriter writer, T value, ExtendedMemberInfo memberInfo, IJsonFormatterResolver fallbackResolver, IDataProtector dataProtector)
        {
            writer.WritePropertyName(memberInfo.Name);
            dynamic memberValue = memberInfo.Getter(value);
            var valueToSerialize = memberInfo.ShouldEncrypt
                ? BuildEncryptedValue(memberValue, fallbackResolver, dataProtector)
                : memberValue;
            JsonSerializer.Serialize(ref writer, valueToSerialize, fallbackResolver);
        }

        private static string BuildEncryptedValue(dynamic memberValue, IJsonFormatterResolver fallbackResolver, IDataProtector dataProtector)
        {
            var localWriter = new JsonWriter();
            JsonSerializer.Serialize(ref localWriter, memberValue, fallbackResolver);
            return dataProtector.Protect(localWriter.ToString());
        }
    }
}
