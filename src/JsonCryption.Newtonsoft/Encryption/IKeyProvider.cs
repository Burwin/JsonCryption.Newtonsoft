using System.Collections.Generic;

namespace JsonCryption.Newtonsoft.Encryption
{
    public interface IKeyProvider
    {
        /// <summary>
        /// Gets a pseudo-random encryption-suitable key
        /// </summary>
        IdentifiedKey GetAnEncryptingKey();

        /// <summary>
        /// Gets an encryption-suitable key by ID
        /// </summary>
        /// <remarks>
        /// Throws a <see cref="KeyNotFoundException"/> if the requested ID doesn't exist.
        /// </remarks>
        /// <param name="id"></param>
        byte[] GetEncryptingKey(int id);

        /// <summary>
        /// Gets a decryption-suitable key by ID
        /// </summary>
        /// <remarks>
        /// Throws a <see cref="KeyNotFoundException"/> if the requested ID doesn't exist.
        /// </remarks>
        /// <param name="id"></param>
        byte[] GetDecryptingKey(int id);

        /// <summary>
        /// Registers a key suitable for both encryption and decryption
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void Register(int id, byte[] key);

        /// <summary>
        /// Registers a key suitable for only decryption
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        void RegisterAsDecryptingKeyOnly(int id, byte[] key);
    }
}