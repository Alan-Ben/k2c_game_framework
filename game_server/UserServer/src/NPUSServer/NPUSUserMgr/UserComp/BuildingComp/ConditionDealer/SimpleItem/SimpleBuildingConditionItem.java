package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.SimpleItem;

import CommonEnum.ESpecAttrType;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp._IBuildingConditionProxy;

public class SimpleBuildingConditionItem implements _IBuildingConditionProxy
{
    private long _m_buildingId;
    private ESpecAttrType _m_attrType;

    public SimpleBuildingConditionItem(long _buildingId, ESpecAttrType _attrType)
    {
        _m_buildingId = _buildingId;
        _m_attrType = _attrType;
    }

    @Override
    public long getBuildingId()
    {
        return _m_buildingId;
    }

    @Override
    public ESpecAttrType getAttrType()
    {
        return _m_attrType;
    }
}
