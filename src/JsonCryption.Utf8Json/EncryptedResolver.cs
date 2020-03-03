using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utf8Json;

namespace JsonCryption.Utf8Json
{
    public sealed class EncryptedResolver : IJsonFormatterResolver
    {
        private readonly IJsonFormatterResolver _fallbackResolver;
        private readonly IDataProtector _dataProtector;
        private readonly FormatterCache _formatterCache = new FormatterCache();

        public EncryptedResolver(IJsonFormatterResolver fallbackResolver, IDataProtector dataProtector)
        {
            _fallbackResolver = fallbackResolver;
            _dataProtector = dataProtector;
        }
        
        public IJsonFormatter<T> GetFormatter<T>()
        {
            if (!_formatterCache.TryGetFormatter<T>(out var formatter))
            {
                formatter = CreateFormatter<T>();
                _formatterCache.Add(formatter);
            }

            return formatter;
        }

        private IJsonFormatter<T> CreateFormatter<T>() => new EncryptedFormatter<T>(_dataProtector, _fallbackResolver, ExtendedMemberInfo.GetFrom(typeof(T), _fallbackResolver).ToArray());

        class FormatterCache
        {
            private readonly ConcurrentDictionary<Type, IJsonFormatter> _formatters = new ConcurrentDictionary<Type, IJsonFormatter>();

            public bool TryGetFormatter<T>(out IJsonFormatter<T> formatter)
            {
                if (!_formatters.TryGetValue(typeof(T), out var rawFormatter))
                {
                    formatter = null;
                    return false;
                }

                formatter = rawFormatter as IJsonFormatter<T>;
                return true;
            }

            public void Add<T>(IJsonFormatter<T> formatter)
            {
                _formatters[typeof(T)] = formatter;
            }
        }
    }
}
