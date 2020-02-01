namespace JsonCryption
{
    public interface IByteConverter<T>
    {
        byte[] ToBytes(T value);
        T FromBytes(byte[] bytes);
    }
}
