package NPGameRes.GameObjs.RefUnionBonus.AttrByEnum;

import CommonEnum.EBonusFilterType;

public class AttrPropertyIdBonusModifier_ConsortId extends _AAttrPropertyBonusModifier
{
    private long _m_lConsortId;

    /************
     * 获取加成类型
     * @return
     */
    @Override
    public EBonusFilterType getFilterType() {return EBonusFilterType.CONSORT_ID;}

    /**
     * 获取对应的子加成id筛选数据，如无筛选则返回0
     * @return ENPPropBonusType
     */
    @Override
    public long getBonusId(){return _m_lConsortId;}

    /***************
     * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
     * @param _subIdInfo
     */
    @Override
    protected void _readStr(String _subIdInfo)
    {
        //读取Id
        if(_subIdInfo.isEmpty())
        	_m_lConsortId = 0;
        else
        	_m_lConsortId = Long.parseLong(_subIdInfo);
    }
}
