using System;
using System.Collections.Generic;

namespace JsonCryption.Newtonsoft.Encryption
{
    public class KeyProvider : IKeyProvider
    {
        private readonly Dictionary<int, byte[]> _encryptingKeys = new Dictionary<int, byte[]>();
        private readonly Dictionary<int, byte[]> _decryptingKeys = new Dictionary<int, byte[]>();
        private readonly List<int> _encryptingKeyIds = new List<int>();

        public IdentifiedKey GetAnEncryptingKey()
        {
            var keyId = _encryptingKeyIds[new Random().Next(0, _encryptingKeyIds.Count - 1)];
            var key = _encryptingKeys[keyId];
            return new IdentifiedKey(keyId, key);
        }
        
        public byte[] GetEncryptingKey(int id) => _encryptingKeys[id];

        public byte[] GetDecryptingKey(int id) => _decryptingKeys[id];
        
        public void Register(int id, byte[] key)
        {
            _encryptingKeyIds.Add(id);
            _encryptingKeys[id] = key;
            _decryptingKeys[id] = key;
        }

        public void RegisterAsDecryptingKeyOnly(int id, byte[] key) => _decryptingKeys[id] = key;
    }
}
