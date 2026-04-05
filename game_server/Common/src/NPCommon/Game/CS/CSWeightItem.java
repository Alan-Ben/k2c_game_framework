package NPCommon.Game.CS;

/**
 * 单权重对象
 */
public class CSWeightItem
{
    private long _m_dataId;
    private int _m_weight;

    public CSWeightItem(long _dataId, int _weight)
    {
        this._m_dataId = _dataId;
        this._m_weight = _weight;
    }

    public long getDataId()
    {
        return _m_dataId;
    }

    public int getWeight()
    {
        return _m_weight;
    }
}
