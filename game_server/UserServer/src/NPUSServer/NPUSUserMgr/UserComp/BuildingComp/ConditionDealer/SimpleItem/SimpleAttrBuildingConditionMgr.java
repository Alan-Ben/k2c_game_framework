package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.SimpleItem;

import CommonEnum.ESpecAttrType;

public class SimpleAttrBuildingConditionMgr
{
    private static final SimpleAttrBuildingConditionMgr _g_instance = new SimpleAttrBuildingConditionMgr();

    public static SimpleAttrBuildingConditionMgr getInstance()
    {
        return _g_instance;
    }

    private SimpleBuildingConditionItem[] _m_itemList;

    public SimpleAttrBuildingConditionMgr()
    {
        _m_itemList = new SimpleBuildingConditionItem[ESpecAttrType.ESpecAttrType_Length];
        for (int i = 0; i < ESpecAttrType.ESpecAttrType_Length; i++)
        {
            _m_itemList[i] = new SimpleBuildingConditionItem(0, ESpecAttrType.values()[i]);
        }
    }

    public SimpleBuildingConditionItem getItem(ESpecAttrType _attrType)
    {
        return _m_itemList[_attrType.ordinal()];
    }
}
