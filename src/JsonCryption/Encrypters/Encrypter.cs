using System;
using System.IO;
using System.Security.Cryptography;

namespace JsonCryption.Encrypters
{
    public abstract class Encrypter
    {
        protected byte[] Key { get; }

        protected Encrypter(byte[] key)
        {
            Key = key;
        }

        public byte[] DecryptToByteArray(string encrypted)
        {
            var buffer = Convert.FromBase64String(encrypted);

            using var inputStream = new MemoryStream(buffer, writable: false);
            using var outputStream = new MemoryStream();
            using var algorithm = GetAlgorithm();

            algorithm.Key = Key;
            var ivLength = algorithm.IV.Length;
            var iv = new byte[ivLength];
            var bytesRead = inputStream.Read(iv, 0, ivLength);
            if (bytesRead < ivLength)
                throw new CryptographicException("IV is missing or invalid.");

            var decryptor = algorithm.CreateDecryptor(algorithm.Key, iv);
            using var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(outputStream);

            return outputStream.ToArray();
        }

        internal string Encrypt(byte[] bytes)
        {
            using var inputStream = new MemoryStream(bytes, writable: false);
            using var outputStream = new MemoryStream();
            using var algorithm = GetAlgorithm();

            algorithm.Key = Key;
            var iv = algorithm.IV;

            outputStream.Write(iv, 0, iv.Length);
            outputStream.Flush();

            var encrypter = algorithm.CreateEncryptor();
            using var cryptoStream = new CryptoStream(outputStream, encrypter, CryptoStreamMode.Write);
            inputStream.CopyTo(cryptoStream);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(outputStream.ToArray());
        }

        protected abstract SymmetricAlgorithm GetAlgorithm();
    }
}
