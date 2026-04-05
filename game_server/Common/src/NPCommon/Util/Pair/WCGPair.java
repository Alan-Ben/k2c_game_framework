package NPCommon.Util.Pair;

public class WCGPair<T1, T2>
{
    public T1 first;
    public T2 second;

    public WCGPair(T1 _first, T2 _second)
    {
        first = _first;
        second = _second;
    }

    @Override
    public String toString()
    {
        return String.format("%s:%s", first, second);
    }
}