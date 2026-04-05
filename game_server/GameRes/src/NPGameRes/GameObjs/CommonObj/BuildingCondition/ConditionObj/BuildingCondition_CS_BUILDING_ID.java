package NPGameRes.GameObjs.CommonObj.BuildingCondition.ConditionObj;

import Common.ConditionEnum.EBuildingConditionType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.BuildingCondition._ABasicBuildingCondition;

public class BuildingCondition_CS_BUILDING_ID extends _ABasicBuildingCondition
{
    private long _m_buildingId = 0;

    public long getBuildingId()
    {
        return _m_buildingId;
    }

    @Override
    public EBuildingConditionType conditionType()
    {
        return EBuildingConditionType.CS_BUILDING_ID;
    }

    public static BuildingCondition_CS_BUILDING_ID readCond(NPStringReader _reader)
    {
        BuildingCondition_CS_BUILDING_ID cond = new BuildingCondition_CS_BUILDING_ID();

        String rawBuildingId = _reader.readItem();
        if (null == rawBuildingId)
        {
            CommLog.error("Can not read str for EBuildingConditionType.CS_BUILDING_ID[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_buildingId = Long.parseLong(rawBuildingId);

        return cond;
    }
}
