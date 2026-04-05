package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import Common.BuildingEnum.EBuildingFuncEnum;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家已放置建筑功能等级 CS_BUILDING_FUNC_LEVEL:building_id:EBuildingFuncEnum（:minLevel:maxLevel）
 */
public class NPPlayerCondition_CS_BUILDING_FUNC_LEVEL extends _ANPBasicPlayerCondition
{
    private long _m_buildingId;//物品子ID
    private EBuildingFuncEnum _m_funcType; //功能类型
    private int _m_minLvl = -1; //-1:不限制最小值
    private int _m_maxLvl = -1; //-1:不限制最大值

    public long buildingId()
    {
        return _m_buildingId;
    }

    public EBuildingFuncEnum getFuncType()
    {
        return _m_funcType;
    }

    public int minLvl()
    {
        return _m_minLvl;
    }

    public int maxLvl()
    {
        return _m_maxLvl;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL;
    }

    public static NPPlayerCondition_CS_BUILDING_FUNC_LEVEL readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_BUILDING_FUNC_LEVEL cond = new NPPlayerCondition_CS_BUILDING_FUNC_LEVEL();

        String rawBuildingId = _reader.readItem();
        String rawFuncType = _reader.readItem();
        if (null == rawBuildingId || null == rawFuncType)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL[" + _reader.getSrcString() + "]");
            return null;
        }
        cond._m_buildingId = Long.parseLong(rawBuildingId);
        cond._m_funcType = EBuildingFuncEnum.valueOf(rawFuncType);

        String rawMinLvl = _reader.readItem();
        if (null != rawMinLvl)
            cond._m_minLvl = Integer.parseInt(rawMinLvl);
        else
            cond._m_minLvl = 1;

        String rawMaxLvl = _reader.readItem();
        if (null != rawMaxLvl)
            cond._m_maxLvl = Integer.parseInt(rawMaxLvl);

        return cond;
    }
}
