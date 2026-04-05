package NPCommon.CommonObj;

import NPEnum.EQuality;

public class NPCommonWeightCostItem
{
    //对应物品
    private NPCommonCostItem _m_ciCostItem;
    //物品品质
    private EQuality _m_eQuality = EQuality.NONE;
    //配置初始权重
    private long _m_lInitWeight;
    //配置额外权重
    private long _m_lExtWeight;
    //当前权重
    private long _m_lWeight;

    public NPCommonWeightCostItem(NPCommonCostItem _costItem, EQuality _quality, long _initWeight, long _extWeight)
    {
        _m_ciCostItem = _costItem.duplicate();
        _m_eQuality = _quality;

        _m_lInitWeight = _initWeight;
        _m_lExtWeight = _extWeight;

        _m_lWeight = _m_lInitWeight;
    }

    public NPCommonCostItem getCostItem()
    {
        return _m_ciCostItem;
    }

    public EQuality getQuality()
    {
        return _m_eQuality;
    }

    public long getInitWeight()
    {
        return _m_lInitWeight;
    }

    public long getExtWeight()
    {
        return _m_lExtWeight;
    }

    public long getWeight()
    {
        return _m_lWeight;
    }

    /**********************
     * 重新调整权重
     * 品质相等-》回到初始权重，品质不等-》增加额外权重
     * @param _quality
     */
    public void adjustWeight(EQuality _quality)
    {
        if (_m_eQuality == _quality)
        {
            _m_lWeight = _m_lInitWeight;
        } else
        {
            _m_lWeight += _m_lExtWeight;
        }
    }
}
