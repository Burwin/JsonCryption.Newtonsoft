﻿using System;

namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal sealed class GuidByteConverter : IByteConverter<Guid>
    {
        public Guid FromBytes(byte[] bytes) => new Guid(bytes);
        public byte[] ToBytes(Guid value) => value.ToByteArray();
    }
}
