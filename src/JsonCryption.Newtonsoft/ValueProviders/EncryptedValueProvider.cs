using JsonCryption.Newtonsoft.Encrypters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal abstract class EncryptedValueProvider<T> : EncryptedValueProvider
    {
        private readonly IValueProvider _innerProvider;

        public EncryptedValueProvider(Encrypter encrypter, IValueProvider innerProvider)
            : base(encrypter)
        {
            _innerProvider = innerProvider;
        }

        public override object GetValue(object target)
        {
            var raw = (T)_innerProvider.GetValue(target);
            var bytes = ToBytes(raw);
            var encrypted = _encrypter.Encrypt(bytes);
            return encrypted;
        }

        public override void SetValue(object target, object value)
        {
            var encrypted = (string)value;
            var bytes = _encrypter.DecryptToByteArray(encrypted);
            _innerProvider.SetValue(target, FromBytes(bytes));
        }

        public abstract T FromBytes(byte[] bytes);
        public abstract byte[] ToBytes(T value);
    }

    internal abstract class EncryptedValueProvider : IValueProvider
    {
        protected readonly Encrypter _encrypter;

        public EncryptedValueProvider(Encrypter encrypter)
        {
            _encrypter = encrypter;
        }
        
        public abstract object GetValue(object target);
        public abstract void SetValue(object target, object value);
    }
}
