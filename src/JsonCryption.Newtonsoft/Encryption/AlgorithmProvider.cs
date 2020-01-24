using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace JsonCryption.Newtonsoft.Encryption
{
    internal class AlgorithmProvider : IAlgorithmProvider
    {
        private readonly Dictionary<int, Func<SymmetricAlgorithm>> _decrypters = new Dictionary<int, Func<SymmetricAlgorithm>>();
        private readonly Dictionary<int, Func<SymmetricAlgorithm>> _encrypters = new Dictionary<int, Func<SymmetricAlgorithm>>();
        private readonly List<int> _encrypterIds = new List<int>();

        public AlgorithmBuilder GetAnEncrypterBuilder()
        {
            var id = _encrypterIds[new Random().Next(0, _encrypterIds.Count - 1)];
            var builder = _encrypters[id];
            return new AlgorithmBuilder(id, builder);
        }

        public SymmetricAlgorithm GetEncrypter(int id) => _encrypters[id].Invoke();
        
        public SymmetricAlgorithm GetDecrypter(int id) => _decrypters[id].Invoke();
        
        public void Register(int id, Func<SymmetricAlgorithm> algorithmBuilder)
        {
            _encrypterIds.Add(id);
            _encrypters[id] = algorithmBuilder;
            _decrypters[id] = algorithmBuilder;
        }

        public void RegisterDecrypterOnly(int id, Func<SymmetricAlgorithm> algorithmBuilder) => _decrypters[id] = algorithmBuilder;
    }
}
