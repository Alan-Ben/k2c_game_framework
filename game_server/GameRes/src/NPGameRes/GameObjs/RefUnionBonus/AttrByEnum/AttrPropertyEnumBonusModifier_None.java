package NPGameRes.GameObjs.RefUnionBonus.AttrByEnum;


import CommonEnum.EBonusFilterType;


/**
 * 属性加成容器,可以单独使用，单独使用时认为是不分类型的属性加成
 */
public class AttrPropertyEnumBonusModifier_None extends _AAttrPropertyBonusModifier
{
    /************
     * 获取加成类型
     * @return
     */
    @Override
    public EBonusFilterType getFilterType() {return EBonusFilterType.NONE;}

    /**
     * 获取对应的子加成id筛选数据，如无筛选则返回0
     * @return ENPPropBonusType
     */
    @Override
    public long getBonusId(){return 0;}

    /***************
     * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
     * @param _subIdInfo
     */
    @Override
    protected void _readStr(String _subIdInfo)
    {

    }
}