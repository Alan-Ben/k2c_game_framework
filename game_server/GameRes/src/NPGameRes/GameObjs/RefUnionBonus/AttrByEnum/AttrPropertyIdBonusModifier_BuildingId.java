package NPGameRes.GameObjs.RefUnionBonus.AttrByEnum;

import CommonEnum.EBonusFilterType;

public class AttrPropertyIdBonusModifier_BuildingId extends _AAttrPropertyBonusModifier
{
    private long _m_lBuildingId;

    /************
     * 获取加成类型
     * @return
     */
    @Override
    public EBonusFilterType getFilterType() {return EBonusFilterType.BUILDING_ID;}

    /**
     * 获取对应的子加成id筛选数据，如无筛选则返回0
     * @return ENPPropBonusType
     */
    @Override
    public long getBonusId(){return _m_lBuildingId;}

    /***************
     * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
     * @param _subIdInfo
     */
    @Override
    protected void _readStr(String _subIdInfo)
    {
        //读取Id
        if(_subIdInfo.isEmpty())
            _m_lBuildingId = 0;
        else
            _m_lBuildingId = Long.parseLong(_subIdInfo);
    }
}
