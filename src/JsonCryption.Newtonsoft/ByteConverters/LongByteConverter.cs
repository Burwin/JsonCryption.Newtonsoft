﻿using System;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class LongByteConverter : IByteConverter<long>
    {
        public long FromBytes(byte[] bytes) => BitConverter.ToInt64(bytes, 0);
        public byte[] ToBytes(long value) => BitConverter.GetBytes(value);
    }
}
