using JsonCryption.System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using System.Text.Json;

namespace JsonCryption.Converters
{
    internal sealed class KeyValuePairConverter<TKey, TValue> : EncryptedConverter<KeyValuePair<TKey, TValue>>
    {
        private static readonly string _purpose = typeof(KeyValuePairConverter<TKey, TValue>).FullName;

        public KeyValuePairConverter(IDataProtectionProvider dataProtectionProvider, JsonSerializerOptions options)
            : base(dataProtectionProvider.CreateProtector(_purpose), options, Coordinator.GetByteConverter<KeyValuePair<TKey, TValue>>())
        {
        }
    }
}
