using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.FLE.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsIDictionary(this Type @this) => typeof(IDictionary).IsAssignableFrom(@this);

        public static bool IsKeyValuePair(this Type @this)
        {
            if (!@this.IsGenericType)
                return false;

            if (@this.GenericTypeArguments.Length != 2)
                return false;

            return typeof(KeyValuePair<,>).MakeGenericType(@this.GenericTypeArguments).IsAssignableFrom(@this);
        }
    }
}
