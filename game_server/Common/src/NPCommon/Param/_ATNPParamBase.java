package NPCommon.Param;

public abstract class _ATNPParamBase<D>
{
    //对应枚举
    protected int _m_iIndex;
    //存储的数据对象
    protected D _m_dData;

    public _ATNPParamBase(int _index, D _data)
    {
        _m_iIndex = _index;
        _m_dData = _data;
    }

    public int getIndex()
    {
        return _m_iIndex;
    }

    public D getData()
    {
        return _m_dData;
    }

    /*********
     * 获取值的处理
     * @return
     */
    public abstract long GetValue();

    /****************
     * 保存值的操作
     * @param value
     */
    public abstract void saveValue(long value);
}