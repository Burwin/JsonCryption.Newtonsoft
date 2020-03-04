using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utf8Json;

namespace JsonCryption.Utf8Json
{
    internal sealed class ConstructorResolver
    {
        public ConstructorInfo GetConstructor(IEnumerable<ExtendedMemberInfo> memberInfos, Type type)
        {
            ConstructorInfo[] allConstructors = GetAllConstructors(type);

            //  1 - look for constructor decorated by SerializationConstructorAttribute
            ConstructorInfo constructor = GetDecoratedConstructor(allConstructors);
            if (constructor != null)
                return constructor;

            //  2 - look for constructor with most matched arguments by name (ignore case)
            constructor = GetMostMatchingConstructor(allConstructors, memberInfos);
            if (constructor != null)
                return constructor;

            //  3 - look for default constructor
            constructor = GetDefaultConstructor(allConstructors);
            if (constructor is null)
                throw new ConstructorResolutionException(type);

            return constructor;
        }

        private static ConstructorInfo GetDefaultConstructor(ConstructorInfo[] allConstructors) => allConstructors.FirstOrDefault(c => c.IsPublic && !c.GetParameters().Any());

        private static ConstructorInfo GetMostMatchingConstructor(IEnumerable<ConstructorInfo> allConstructors, IEnumerable<ExtendedMemberInfo> memberInfos)
        {
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
                var matchCount = serializedNames.Count(n => paramSet.Contains(n));

                // check for ambiguous match
                if (matchCount == mostMatching.MatchingParameters)
                    return null;

                if (matchCount > mostMatching.MatchingParameters)
                    mostMatching = (orderedConstructor.ConstructorInfo, matchCount);
            }

            return mostMatching.ConstructorInfo;
        }

        private static ConstructorInfo GetDecoratedConstructor(ConstructorInfo[] allConstructors) => allConstructors.FirstOrDefault(c => c.GetCustomAttribute<SerializationConstructorAttribute>() != null);

        private static ConstructorInfo[] GetAllConstructors(Type type)
        {
            return type
                .GetConstructors()
                .Concat(type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic))
                .ToArray();
        }
    }
}
