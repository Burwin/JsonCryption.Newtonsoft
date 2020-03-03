using Utf8Json;
using Utf8Json.Resolvers;

namespace JsonCryption.Utf8Json
{
    internal static class JsonFormatterResolverExtensions
    {
        public static bool AllowsPrivate(this IJsonFormatterResolver formatterResolver)
        {
            var thisType = formatterResolver.GetType();
            return thisType == StandardResolver.AllowPrivate.GetType()
                || thisType == StandardResolver.AllowPrivateCamelCase.GetType()
                || thisType == StandardResolver.AllowPrivateExcludeNull.GetType()
                || thisType == StandardResolver.AllowPrivateExcludeNullCamelCase.GetType()
                || thisType == StandardResolver.AllowPrivateExcludeNullSnakeCase.GetType()
                || thisType == StandardResolver.AllowPrivateSnakeCase.GetType();
        }
    }
}
