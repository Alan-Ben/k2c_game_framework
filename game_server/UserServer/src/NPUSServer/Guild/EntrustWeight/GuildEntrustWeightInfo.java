package NPUSServer.Guild.EntrustWeight;

import NPGameRes.Refs.Guild.RefGuildEntrustQuality;

public class GuildEntrustWeightInfo
{
    private RefGuildEntrustQuality _m_refWeight;
    private int _m_rollIndex;

    public GuildEntrustWeightInfo(RefGuildEntrustQuality _refWeight)
    {
        _m_refWeight = _refWeight;
    }

    public void _initIndex(int _rollIndex)
    {
        _m_rollIndex = _rollIndex;
    }

    public long getRefId()
    {
        return _m_refWeight.Id();
    }

    public RefGuildEntrustQuality getRef()
    {
        return _m_refWeight;
    }

    /**
     * 取得当前权重
     * 权重计算规则：基础权重 + (当前抽数 - 开始增加权重的抽数) * 每次增加的权重，和权重上限比较取最小值
     * @return
     */
    public int getWeight()
    {
        return Math.min(_m_refWeight.base_weight + (Math.max(0, _m_rollIndex + 1 - _m_refWeight.start_add_index)) * _m_refWeight.each_add_weight, _m_refWeight.weight_limit);
    }

    public void incIndex()
    {
        _m_rollIndex++;
    }

    public void resetIndex()
    {
        _m_rollIndex = 0;
    }

    @Override
    public String toString()
    {
        return _m_refWeight.Id() + ":" + _m_rollIndex;
    }
}
