namespace JsonCryption
{
    internal interface IByteConverter<T>
    {
        byte[] ToBytes(T value);
        T FromBytes(byte[] bytes);
    }
}
