using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JsonCryption.Newtonsoft.Encryption
{
    public sealed class Encrypter
    {
        private readonly IKeyProvider _keyProvider;
        private readonly IAlgorithmProvider _algorithmProvider;
        private readonly IEncryptionBlobTranslater _encryptionInfoTranslater;

        public Encrypter(IKeyProvider keyProvider, IAlgorithmProvider algorithmProvider, IEncryptionBlobTranslater encryptionInfoTranslater)
        {
            _keyProvider = keyProvider;
            _algorithmProvider = algorithmProvider;
            _encryptionInfoTranslater = encryptionInfoTranslater;
        }

        public byte[] DecryptToByteArray(string cipher)
        {
            var encryptionInfo = _encryptionInfoTranslater.FromCipher(cipher);
            var buffer = Convert.FromBase64String(cipher);

            using var inputStream = new MemoryStream(buffer, writable: false);
            using var outputStream = new MemoryStream();
            using var algorithm = _algorithmProvider.GetDecrypter(encryptionInfo.AlgorithmId);

            var ivLength = algorithm.IV.Length;
            var iv = new byte[ivLength];
            var bytesRead = inputStream.Read(iv, 0, ivLength);
            if (bytesRead < ivLength)
                throw new CryptographicException("IV is missing or invalid.");

            var key = _keyProvider.GetDecryptingKey(encryptionInfo.KeyId);
            var decryptor = algorithm.CreateDecryptor(key, iv);
            using var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(outputStream);

            return outputStream.ToArray();
        }

        internal string Encrypt(byte[] bytes)
        {
            var algorithmBuilder = _algorithmProvider.GetAnEncrypterBuilder();
            
            using var inputStream = new MemoryStream(bytes, writable: false);
            using var outputStream = new MemoryStream();
            using var algorithm = algorithmBuilder.Build();

            var iv = algorithm.IV;

            outputStream.Write(iv, 0, iv.Length);
            outputStream.Flush();

            var identifiedKey = _keyProvider.GetAnEncryptingKey();
            algorithm.Key = identifiedKey.Key;
            var keyId = identifiedKey.ID;
            var encrypter = algorithm.CreateEncryptor();

            using var cryptoStream = new CryptoStream(outputStream, encrypter, CryptoStreamMode.Write);
            inputStream.CopyTo(cryptoStream);
            cryptoStream.FlushFinalBlock();

            var encrypted = Convert.ToBase64String(outputStream.ToArray());
            var encryptedBlob = new EncryptedBlob(keyId, algorithmBuilder.ID, encrypted);

            return _encryptionInfoTranslater.ToCipher(encryptedBlob);
        }
    }
}
