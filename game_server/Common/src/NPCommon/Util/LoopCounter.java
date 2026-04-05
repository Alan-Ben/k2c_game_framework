package NPCommon.Util;

/*****
 * 循环计数器，指定最大循环次数，检验循环是否死循环，
 */
public class LoopCounter
{
    private int _m_iMaxCount = 100000;
    private int _m_iCurCount = 0;

    public LoopCounter(int _maxCount)
    {
        _m_iMaxCount = _maxCount;
    }

    public LoopCounter()
    {
    }

    /*****
     * 检测循环是否已经超出上限，
     * @return
     */
    public boolean incCount()
    {
        _m_iCurCount++;
        return _m_iCurCount <= _m_iMaxCount;
    }

    public int getCount()
    {
        return _m_iCurCount;
    }
}
