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
        public Func<object, object> Getter { get; }
        public Action<object, object> Setter { get; }
        public Func<string, IJsonFormatterResolver, object> Deserializer { get; }
        public FallbackSerializer FallbackSerializer { get; }
        public Func<object, dynamic> Converter { get; }

        private static readonly Type ObjectType = typeof(object);

        public ExtendedMemberInfo(MemberInfo memberInfo, Type parentType)
        {
            MemberInfo = memberInfo;
            Type = MemberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : ((PropertyInfo)memberInfo).PropertyType;
            Name = GetNameForSerializing(MemberInfo);
            ShouldEncrypt = memberInfo.GetCustomAttribute<EncryptAttribute>() != null;
            Getter = BuildGetter(MemberInfo, parentType);
            Setter = BuildSetter(MemberInfo, parentType, Type);
            Deserializer = BuildDeserializer(Type);
            FallbackSerializer = BuildFallbackSerializer(Type);
            Converter = BuildConverter(Type);
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
                .Concat(fields)
                .Select(m => new ExtendedMemberInfo(m, type));
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

        private string GetNameForSerializing(MemberInfo memberInfo)
        {
            var dataMemberAttribute = memberInfo.GetCustomAttribute<DataMemberAttribute>();
            return dataMemberAttribute is null || !dataMemberAttribute.IsNameSetExplicitly
                ? memberInfo.Name
                : dataMemberAttribute.Name;
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
}
