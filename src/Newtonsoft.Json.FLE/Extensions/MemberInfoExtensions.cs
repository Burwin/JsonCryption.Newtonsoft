using System;
using System.Reflection;

namespace Newtonsoft.Json.FLE.Extensions
{
    internal static class MemberInfoExtensions
    {
        public static bool ShouldEncrypt(this MemberInfo info) => info.GetCustomAttribute<EncryptAttribute>(inherit: true) != null;
        
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Custom:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                case MemberTypes.Constructor:
                case MemberTypes.All:
                default:
                    throw new ArgumentException("MemberInfo must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
            }
        }
    }
}
