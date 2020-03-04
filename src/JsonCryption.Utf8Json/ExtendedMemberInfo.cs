using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Utf8Json;

namespace JsonCryption.Utf8Json
{
    internal sealed class ExtendedMemberInfo
    {
        public MemberInfo MemberInfo { get; }
        public string Name { get; }
        public Type Type { get; }
        public bool ShouldEncrypt { get; }
        public bool HasNestedEncryptedMembers { get; }
        public Func<object, object> Getter { get; }
        public Action<object, object> Setter { get; }
        public Func<string, IJsonFormatterResolver, object> EncryptedDeserializer { get; }
        public TypedDeserializer TypedDeserializer { get; }
        public FallbackSerializer FallbackSerializer { get; }
        public Func<object, dynamic> Converter { get; }

        private static readonly Type ObjectType = typeof(object);

        public ExtendedMemberInfo(MemberInfo memberInfo, Type parentType, IJsonFormatterResolver fallbackResolver)
        {
            MemberInfo = memberInfo;
            Type = MemberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : ((PropertyInfo)memberInfo).PropertyType;
            Name = MemberInfo.SerializedName();
            ShouldEncrypt = memberInfo.ShouldEncrypt();
            HasNestedEncryptedMembers = GetHasNestedEncryptedMembers(Type, fallbackResolver);
            Getter = BuildGetter(MemberInfo, parentType);
            Setter = BuildSetter(MemberInfo, parentType, Type);
            EncryptedDeserializer = BuildDeserializer(Type);
            TypedDeserializer = BuildTypedDeserializer(Type);
            FallbackSerializer = BuildFallbackSerializer(Type);
            Converter = BuildConverter(Type);
        }

        private TypedDeserializer BuildTypedDeserializer(Type type)
        {
            var method = typeof(JsonSerializer)
                .GetMethods()
                .Where(m => m.Name == "Deserialize")
                .Select(m => (MethodInfo: m, Params: m.GetParameters(), Args: m.GetGenericArguments()))
                .Where(x => x.Params.Length == 2)
                .Where(x => x.Params[0].ParameterType == typeof(JsonReader).MakeByRefType())
                .Where(x => x.Params[1].ParameterType == typeof(IJsonFormatterResolver))
                .Single().MethodInfo;

            var generic = method.MakeGenericMethod(type);

            var readerExpr = Expression.Parameter(typeof(JsonReader).MakeByRefType(), "reader");
            var resolverExpr = Expression.Parameter(typeof(IJsonFormatterResolver), "resolver");

            var body = Expression.Call(generic, readerExpr, resolverExpr);
            var convertBodyExpr = Expression.Convert(body, ObjectType);
            var lambda = Expression.Lambda<TypedDeserializer>(convertBodyExpr, readerExpr, resolverExpr);
            return lambda.Compile();
        }

        private bool GetHasNestedEncryptedMembers(Type type, IJsonFormatterResolver fallbackResolver)
        {
            if (!type.IsClass)
                return false;
            
            // loop through all nested levels until no more properties/fields are found
            var toProcess = new Stack<Type>();
            var processed = new HashSet<Type>();

            toProcess.Push(type);
            
            while (toProcess.Count > 0)
            {
                var nextType = toProcess.Pop();
                if (processed.Contains(nextType))
                    continue;

                processed.Add(nextType);

                // get all properties and fields
                var memberInfos = GetSerializableMemberInfos(nextType, fallbackResolver).ToArray();
                if (memberInfos.Any(m => m.ShouldEncrypt()))
                    return true;

                var memberInfoTypes = memberInfos
                    .Select(m => m.GetUnderlyingType())
                    .Where(m => !processed.Contains(m))
                    .ToArray();

                foreach (var memberType in memberInfoTypes.Where(m => m.IsClass))
                {
                    toProcess.Push(memberType);
                }
            }

            return false;
        }

        private Func<object, dynamic> BuildConverter(Type type)
        {
            var convertToType = Nullable.GetUnderlyingType(type) ?? type;

            Expression<Func<object, dynamic>> lambda = (object value) =>
                value == null
                ? null
                : value is IConvertible ? Convert.ChangeType(value, convertToType) : value;

            return lambda.Compile();
        }

        private FallbackSerializer BuildFallbackSerializer(Type type)
        {
            var method = typeof(JsonSerializer)
                .GetMethods()
                .Where(m => m.Name == "Serialize")
                .Select(m => (MethodInfo: m, Params: m.GetParameters(), Args: m.GetGenericArguments()))
                .Where(x => x.Params.Length == 3)
                .Where(x => x.Params[0].ParameterType == typeof(JsonWriter).MakeByRefType())
                .Where(x => x.Params[1].ParameterType == x.Args[0])
                .Where(x => x.Params[2].ParameterType == typeof(IJsonFormatterResolver))
                .Single().MethodInfo;

            var generic = method.MakeGenericMethod(type);

            var writerExpr = Expression.Parameter(typeof(JsonWriter).MakeByRefType(), "writer");
            var valueExpr = Expression.Parameter(ObjectType, "obj");
            var resolverExpr = Expression.Parameter(typeof(IJsonFormatterResolver), "resolver");

            var typedValueExpr = Expression.Convert(valueExpr, type);
            var body = Expression.Call(generic, writerExpr, typedValueExpr, resolverExpr);
            var lambda = Expression.Lambda<FallbackSerializer>(body, writerExpr, valueExpr, resolverExpr);
            return lambda.Compile();
        }

        private Func<string, IJsonFormatterResolver, object> BuildDeserializer(Type type)
        {
            var method = typeof(JsonSerializer).GetMethod(
                nameof(JsonSerializer.Deserialize),
                new[]
                {
                    typeof(string),
                    typeof(IJsonFormatterResolver)
                });
            var generic = method.MakeGenericMethod(type);

            var jsonParamExpr = Expression.Parameter(typeof(string), "json");
            var resolverExpr = Expression.Parameter(typeof(IJsonFormatterResolver), "resolver");

            var body = Expression.Call(generic, jsonParamExpr, resolverExpr);
            var objectifiedBody = Expression.Convert(body, ObjectType);
            var lambda = Expression.Lambda<Func<string, IJsonFormatterResolver, object>>(objectifiedBody, jsonParamExpr, resolverExpr);
            return lambda.Compile();
        }

        public static IEnumerable<ExtendedMemberInfo> GetFrom(Type type, IJsonFormatterResolver fallbackResolver)
        {
            return GetSerializableMemberInfos(type, fallbackResolver)
                .Select(m => new ExtendedMemberInfo(m, type, fallbackResolver));
        }

        private static IEnumerable<MemberInfo> GetSerializableMemberInfos(Type type, IJsonFormatterResolver fallbackResolver)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (fallbackResolver.AllowsPrivate()) bindingFlags |= BindingFlags.NonPublic;

            // properties
            var allProperties = type
                .GetProperties(bindingFlags)
                .ToArray();

            var properties = allProperties
                .Where(m => m.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                .ToArray();

            // fields
            var explicitlyIgnoredProperties = allProperties
                .Where(m => m.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
                .Select(p => p.Name);

            var propInfoNames = new HashSet<string>(properties.Select(p => p.Name).Concat(explicitlyIgnoredProperties));

            var fields = type
                .GetFields(bindingFlags)
                .Where(m => m.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                .Where(f => !IsBackingField(f) || !IsBackingFieldAlreadyIncluded(f, propInfoNames));

            return properties
                .Cast<MemberInfo>()
                .Concat(fields);
        }

        private static bool IsBackingFieldAlreadyIncluded(FieldInfo f, HashSet<string> propInfoNames)
        {
            var propName = Regex.Match(f.Name, "(?<=<).+(?=>k__BackingField)").Value;
            return propInfoNames.Contains(propName);
        }

        private static bool IsBackingField(FieldInfo f) => f.IsPrivate && f.Name.Contains("k__BackingField");

        private Action<object, object> BuildSetter(MemberInfo memberInfo, Type parentType, Type type)
        {
            var targetExp = Expression.Parameter(ObjectType, "target");
            var typedTargetExp = Expression.Convert(targetExp, parentType);
            var valueExp = Expression.Parameter(ObjectType, "value");
            var typedValueExp = Expression.Convert(valueExp, type);

            var memberExp = Expression.PropertyOrField(typedTargetExp, memberInfo.Name);
            var assignExp = Expression.Assign(memberExp, typedValueExp);

            return Expression.Lambda<Action<object, object>>(assignExp, targetExp, valueExp).Compile();
        }

        private Func<object, object> BuildGetter(MemberInfo memberInfo, Type parentType)
        {
            var parameter = Expression.Parameter(ObjectType, "obj");
            var typedParameter = Expression.Convert(parameter, parentType);
            var body = Expression.MakeMemberAccess(typedParameter, memberInfo);
            var objectifiedBody = Expression.Convert(body, ObjectType);
            var lambda = Expression.Lambda<Func<object, object>>(objectifiedBody, parameter);
            return lambda.Compile();
        }
    }

    internal delegate void FallbackSerializer(ref JsonWriter writer, object value, IJsonFormatterResolver fallbackResolver);
    internal delegate object TypedDeserializer(ref JsonReader reader, IJsonFormatterResolver resolver);
}
