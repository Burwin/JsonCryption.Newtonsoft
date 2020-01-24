using System;
using System.Security.Cryptography;

namespace JsonCryption.Newtonsoft.Encryption
{
    public interface IAlgorithmProvider
    {
        /// <summary>
        /// Gets a pseudo-random <see cref="AlgorithmBuilder"/> of an encryption-suitable <see cref="SymmetricAlgorithm"/>
        /// </summary>
        AlgorithmBuilder GetAnEncrypterBuilder();

        /// <summary>
        /// Gets the <see cref="AlgorithmBuilder"/> of an encryption-suitable <see cref="SymmetricAlgorithm"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        SymmetricAlgorithm GetEncrypter(int id);

        /// <summary>
        /// Gets the <see cref="AlgorithmBuilder"/> of a decryption-suitable <see cref="SymmetricAlgorithm"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        SymmetricAlgorithm GetDecrypter(int id);

        /// <summary>
        /// Registers a <see cref="SymmetricAlgorithm"/> for dual use as an encrypter and decrypter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="algorithm"></param>
        void Register(int id, Func<SymmetricAlgorithm> algorithmBuilder);

        /// <summary>
        /// Registers a <see cref="SymmetricAlgorithm"/> for use as a decrypter only.
        /// </summary>
        /// <remarks>
        /// Use for algorithms that are being retired, which may still be needed to decrypt data,
        /// but shouldn't be used to encrypt new data.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="algorithm"></param>
        void RegisterDecrypterOnly(int id, Func<SymmetricAlgorithm> algorithmBuilder);
    }
}