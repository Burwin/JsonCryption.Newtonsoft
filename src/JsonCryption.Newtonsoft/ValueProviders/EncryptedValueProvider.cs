using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Serialization;
using System;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal abstract class EncryptedValueProvider<T> : EncryptedValueProvider
    {
        private readonly IValueProvider _innerProvider;

        public EncryptedValueProvider(IDataProtector dataProtector, IValueProvider innerProvider)
            : base(dataProtector)
        {
            _innerProvider = innerProvider;
        }

        public override object GetValue(object target)
        {
            var raw = (T)_innerProvider.GetValue(target);
            var unprotectedBytes = ToBytes(raw);
            var protectedBytes = _dataProtector.Protect(unprotectedBytes);
            var cipherText = Convert.ToBase64String(protectedBytes);
            return cipherText;
        }

        public override void SetValue(object target, object value)
        {
            var cipherText = (string)value;
            var protectedBytes = Convert.FromBase64String(cipherText);
            var unprotectedBytes = _dataProtector.Unprotect(protectedBytes);
            _innerProvider.SetValue(target, FromBytes(unprotectedBytes));
        }

        public abstract T FromBytes(byte[] bytes);
        public abstract byte[] ToBytes(T value);
    }

    internal abstract class EncryptedValueProvider : IValueProvider
    {
        protected readonly IDataProtector _dataProtector;

        public EncryptedValueProvider(IDataProtector dataProtector)
        {
            _dataProtector = dataProtector;
        }
        
        public abstract object GetValue(object target);
        public abstract void SetValue(object target, object value);
    }
}
