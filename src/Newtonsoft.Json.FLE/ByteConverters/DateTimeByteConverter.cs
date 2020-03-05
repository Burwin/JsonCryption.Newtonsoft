﻿using System;

namespace Newtonsoft.Json.FLE.ByteConverters
{
    internal sealed class DateTimeByteConverter : IByteConverter<DateTime>
    {
        public DateTime FromBytes(byte[] bytes) => DateTime.FromBinary(BitConverter.ToInt64(bytes, 0));
        public byte[] ToBytes(DateTime value) => BitConverter.GetBytes(value.ToBinary());
    }
}
