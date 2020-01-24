using System;
using System.Security.Cryptography;

namespace JsonCryption.Newtonsoft.Encryption
{
    public sealed class AlgorithmBuilder
    {
        private readonly Func<SymmetricAlgorithm> _builder;
        
        public int ID { get; }
        public SymmetricAlgorithm Build() => _builder.Invoke();

        public AlgorithmBuilder(int id, Func<SymmetricAlgorithm> builder)
        {
            ID = id;
            _builder = builder;
        }
    }
}
