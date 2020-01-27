using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal abstract class EncryptedValueProvider<T> : EncryptedValueProvider
    {
        private readonly IEncryptedConverter<T> _encryptedConverter;
        private readonly IValueProvider _innerProvider;
        private readonly T _emptyValue;

        public EncryptedValueProvider(IEncryptedConverter<T> encryptedConverter, IValueProvider innerProvider, T emptyValue = default)
        {
            _encryptedConverter = encryptedConverter;
            _innerProvider = innerProvider;
            _emptyValue = emptyValue;
        }

        public override object GetValue(object target)
        {
            var value = (T)_innerProvider.GetValue(target) ?? _emptyValue;
            return _encryptedConverter.ToCipherText(value);
        }

        public override void SetValue(object target, object value)
        {
            _innerProvider.SetValue(target, value);
        }
    }

    internal abstract class EncryptedValueProvider : IValueProvider
    {
        public abstract object GetValue(object target);
        public abstract void SetValue(object target, object value);
    }
}
