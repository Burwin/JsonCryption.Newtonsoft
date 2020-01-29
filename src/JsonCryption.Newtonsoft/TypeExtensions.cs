using System;
using System.Collections;

namespace JsonCryption.Newtonsoft
{
    public static class TypeExtensions
    {
        public static bool IsIDictionary(this Type @this) => typeof(IDictionary).IsAssignableFrom(@this);
    }
}
