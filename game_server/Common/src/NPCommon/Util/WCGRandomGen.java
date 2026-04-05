package NPCommon.Util;

public class WCGRandomGen
{
    private static final long _m_modulus = 1L << 48;
    ;
    private static final long _m_increment = 11;
    private static final long _m_multiplier = 25214903917L;

    private long _m_previous = 0;

    public WCGRandomGen(long _seed)
    {
        this._m_previous = _seed;
    }

    public long rand()
    {
        long random_number = (this._m_previous * _m_multiplier + _m_increment) % _m_modulus;
        this._m_previous = random_number;
        return random_number > 0 ? random_number : -random_number;
    }

    /**************
     * 获取随机种子，这个数据不可逆，只能直接返回当前的
     * @return
     */
    public long Previous()
    {
        return _m_previous;
    }

    public long randRange(int _range)
    {
        return (int) (rand() % _range);
    }

    /**
     * 取得指定范围内整數,
     */
    public int nextInt(int _min, int _max)
    {
        long randomNum = rand();
        if (_min >= _max)
        {
            return _min;
        }
        return (int) ((randomNum % (_max - _min + 1)) + _min);
    }
}