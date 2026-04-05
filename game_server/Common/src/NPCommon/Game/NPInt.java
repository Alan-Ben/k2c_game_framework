package NPCommon.Game;

public class NPInt
{
    public NPInt()
    {
    }

    public NPInt(int _value)
    {
        _m_value = _value;
    }

    public int _m_value;

    public void setValue(int _value)
    {
        _m_value = _value;
    }

    public int getValue()
    {
        return _m_value;
    }

    public int v()
    {
        return _m_value;
    }

    @Override
    public String toString()
    {
        return Integer.toString(_m_value);
    }

    ;

    @Override
    public int hashCode()
    {
        return _m_value;
    }
}
