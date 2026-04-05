using System;

[Serializable]
public class WCGTriple<T1, T2, T3>
{
    public T1 first;
    public T2 second;
    public T3 third;

    public WCGTriple(T1 _first, T2 _second, T3 _third)
    {
        first = _first;
        second = _second;
        third = _third;
    }

    public override string ToString()
    {
        return $"{first}:{second}:{third}";
    }
}
