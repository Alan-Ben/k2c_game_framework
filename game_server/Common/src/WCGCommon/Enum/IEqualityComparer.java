package WCGCommon.Enum;


public abstract class IEqualityComparer<T>
{
    public abstract boolean Equals(T x, T y);

    public abstract int GetHashCode(T obj);
}
