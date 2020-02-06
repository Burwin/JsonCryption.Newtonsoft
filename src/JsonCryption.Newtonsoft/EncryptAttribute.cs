using System;

namespace JsonCryption
{
    /// <summary>
    /// Decorate fields and properties to encrypt/decrypt when serializing/deserializing
    /// 
    /// For anything but public properties, must also decorate with JsonPropertyAttribute:
    /// 
    /// class Foo
    /// {
    ///     [Encrypt]
    ///     [JsonProperty]
    ///     private string _myPrivateEncryptedField;
    /// }
    /// </summary>
    public sealed class EncryptAttribute : Attribute
    {
    }
}
