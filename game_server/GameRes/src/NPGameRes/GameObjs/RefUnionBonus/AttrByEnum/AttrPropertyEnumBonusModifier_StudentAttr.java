package NPGameRes.GameObjs.RefUnionBonus.AttrByEnum;


import CommonEnum.EBonusFilterType;
import CommonEnum.ESpecAttrType;


/**
 * 属性加成容器,可以单独使用，单独使用时认为是不分类型的属性加成
 */
public class AttrPropertyEnumBonusModifier_StudentAttr extends _AAttrPropertyBonusModifier
{
    private ESpecAttrType _m_eAttrType;

    /************
     * 获取加成类型
     * @return
     */
    @Override
    public EBonusFilterType getFilterType() {return EBonusFilterType.STUDENT_ATTR;}

    /**
     * 获取对应的子加成id筛选数据，如无筛选则返回0
     * @return ENPPropBonusType
     */
    @Override
    public long getBonusId(){return _m_eAttrType.ordinal();}

    /***************
     * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
     * @param _subIdInfo
     */
    @Override
    protected void _readStr(String _subIdInfo)
    {
        //读取类型
        if(_subIdInfo.isEmpty())
            _m_eAttrType = ESpecAttrType.NONE;
        else
            _m_eAttrType = ESpecAttrType.valueOf(_subIdInfo.toUpperCase());
    }
}