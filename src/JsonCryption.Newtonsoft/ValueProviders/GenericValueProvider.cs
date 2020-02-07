using JsonCryption.Newtonsoft.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JsonCryption.Newtonsoft.ValueProviders
{
    internal sealed class GenericValueProvider<T> : GenericValueProvider
    {
        public GenericValueProvider(IEncryptedConverter encryptedConverter, IValueProvider innerProvider, JsonSerializer serializer)
            : base(encryptedConverter, innerProvider, serializer)
        {
        }

        public override void SetValue(object target, object value)
        {
            var jObject = _encryptedConverter.FromCipherText((string)value, _serializer) as JObject;
            var convertedObject = jObject.ToObject<T>();
            _innerProvider.SetValue(target, convertedObject);
        }
    }

    internal abstract class GenericValueProvider : IValueProvider
    {
        protected readonly IEncryptedConverter _encryptedConverter;
        protected readonly IValueProvider _innerProvider;
        protected readonly JsonSerializer _serializer;

        public GenericValueProvider(IEncryptedConverter encryptedConverter, IValueProvider innerProvider, JsonSerializer serializer)
        {
            _encryptedConverter = encryptedConverter;
            _innerProvider = innerProvider;
            _serializer = serializer;
        }

        public object GetValue(object target)
        {
            var value = _innerProvider.GetValue(target);
            return _encryptedConverter.ToCipherText(value, _serializer);
        }

        public abstract void SetValue(object target, object value);
    }
}
