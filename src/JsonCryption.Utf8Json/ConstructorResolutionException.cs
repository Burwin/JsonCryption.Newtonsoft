using System;

namespace JsonCryption.Utf8Json
{
    public sealed class ConstructorResolutionException : Exception
    {
        public ConstructorResolutionException(Type type)
            : base($"No suitable constructor found for type {type.Name}")
        {
        }
    }
}
