package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家已放置建筑 CS_HAS_BUILDING:building_id
 */
public class NPPlayerCondition_CS_HAS_BUILDING extends _ANPBasicPlayerCondition
{
    private long _m_buildingId;//物品子ID

    public long buildingId()
    {
        return _m_buildingId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HAS_BUILDING;
    }

    public static NPPlayerCondition_CS_HAS_BUILDING readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_HAS_BUILDING cond = new NPPlayerCondition_CS_HAS_BUILDING();

        String rawBuildingId = _reader.readItem();
        if (null == rawBuildingId)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_HAS_BUILDING[" + _reader.getSrcString() + "]");
            return null;
        }
        cond._m_buildingId = Long.parseLong(rawBuildingId);

        return cond;
    }
}
