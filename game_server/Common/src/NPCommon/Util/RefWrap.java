package NPCommon.Util;

public class RefWrap<T>
{
    public T v;

    public RefWrap(T value)
    {
        v = value;
    }

    public T get()
    {
        return v;
    }

    public void set(T _v)
    {
        v = _v;
    }
}