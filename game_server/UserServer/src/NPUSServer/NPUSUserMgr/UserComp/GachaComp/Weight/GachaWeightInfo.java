package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Weight;

import NPEnum.EQuality;
import NPGameRes.Refs.AvatarGacha.RefGachaPoolWeight;

/**
 * 抽卡权重信息，包含三个字段：品质id、当前的抽数
 * ["int", "quality", "品质"],
 * ["int", "roll_index", "当前抽数"],
 */
public class GachaWeightInfo
{
    private RefGachaPoolWeight _m_refWeight;
    private EQuality _m_quality;
    private int _m_rollIndex;

    public GachaWeightInfo(RefGachaPoolWeight _refWeight)
    {
        _m_refWeight = _refWeight;
        _m_quality = _refWeight.quality;
    }

    public void _initRollIndex(int _rollIndex)
    {
        _m_rollIndex = _rollIndex;
    }

    public EQuality getQuality()
    {
        return _m_quality;
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

    public void incRollIndex()
    {
        _m_rollIndex++;
    }

    public void resetRollIndex()
    {
        _m_rollIndex = 0;
    }

    @Override
    public String toString()
    {
        return _m_quality + ":" + _m_rollIndex;
    }
}
