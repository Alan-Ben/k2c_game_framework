package NPCommon.Game;

public class NPLong
{
    public NPLong()
    {
    }

    public NPLong(long _value)
    {
        _m_value = _value;
    }

    public long _m_value;

    public void setValue(long _value)
    {
        _m_value = _value;
    }

    public long getValue()
    {
        return _m_value;
    }

    public long v()
    {
        return _m_value;
    }

    @Override
    public String toString()
    {
        return Long.toString(_m_value);
    }

    ;

    @Override
    public int hashCode()
    {
        return (int) (this.getValue() ^ (this.getValue() >>> 32));

    }
}
