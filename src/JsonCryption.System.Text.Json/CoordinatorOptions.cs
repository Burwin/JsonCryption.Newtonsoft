using Microsoft.AspNetCore.DataProtection;
using System;
using System.Text.Json;

namespace JsonCryption
{
    /// <summary>
    /// Options for configuring the encryption/decryption and serialization/deserialization
    /// </summary>
    public sealed class CoordinatorOptions
    {
        /// <summary>
        /// Get or set the <see cref="JsonSerializerOptions"/> for the <see cref="JsonSerializer"/>
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; set; }

        /// <summary>
        /// Get or set the root <see cref="IDataProtectionProvider"/> used to create instances of <see cref="IDataProtector"/>
        /// to use when Protecting/Unprotecting data
        /// </summary>
        public IDataProtectionProvider DataProtectionProvider { get; set; }

        /// <summary>
        /// An empty <see cref="CoordinatorOptions"/> with null <see cref="JsonSerializerOptions"/> and null <see cref="IDataProtectionProvider"/>
        /// </summary>
        public static CoordinatorOptions Empty => new CoordinatorOptions();

        private CoordinatorOptions()
        {

        }

        /// <summary>
        /// Ensures the options are properly setup, otherwise throws a <see cref="NullReferenceException"/>
        /// </summary>
        public void Validate()
        {
            if (DataProtectionProvider is null)
                throw new NullReferenceException(nameof(DataProtectionProvider));
        }
    }
}