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
        }

        private Func<string, IJsonFormatterResolver, object> BuildDeserializer(Type type)
        {
            var method = typeof(JsonSerializer).GetMethod(nameof(JsonSerializer.Deserialize), new[] { typeof(string), typeof(IJsonFormatterResolver) });
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
            var properties = type
                .GetProperties(bindingFlags)
                .Where(m => m.GetCustomAttribute<IgnoreDataMemberAttribute>() == null)
                .ToArray();

            var propInfoNames = new HashSet<string>(properties.Select(p => p.Name));

            // fields
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
}
