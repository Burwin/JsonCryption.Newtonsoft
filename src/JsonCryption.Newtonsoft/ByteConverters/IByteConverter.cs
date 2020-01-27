namespace JsonCryption.Newtonsoft.ByteConverters
{
    internal interface IByteConverter<T>
    {
        byte[] ToBytes(T value);
        T FromBytes(byte[] bytes);
    }
}
