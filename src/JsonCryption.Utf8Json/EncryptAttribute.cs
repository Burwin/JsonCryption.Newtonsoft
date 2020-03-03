using System;

namespace JsonCryption.Utf8Json
{
    /// <summary>
    /// Decorate fields and properties to encrypt/decrypt when serializing/deserializing
    /// 
    /// class Foo
    /// {
    ///     [Encrypt]
    ///     private string _myPrivateEncryptedField;
    /// }
    /// </summary>
    public sealed class EncryptAttribute : Attribute
    {
    }
}
