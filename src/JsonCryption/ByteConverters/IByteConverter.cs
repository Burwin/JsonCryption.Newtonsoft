namespace JsonCryption.ByteConverters
{
    public interface IByteConverter<T>
    {
        byte[] ToBytes(T value);
        T FromBytes(byte[] bytes);
    }
}
