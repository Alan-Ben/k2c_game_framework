package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGTeamTriggerType;
import WCGCommon.Enum.NPEnum.EWCGUnitType;

public class WCGTeamTriggerSummon extends _AWCGBasicTeamTrigger
{
    private EWCGUnitType _m_lUnitType;

    private long _m_lActorId;

    private int _m_iLevel;

    public EWCGUnitType UnitType()
    {
        return _m_lUnitType;
    }

    public long ActorId()
    {
        return _m_lActorId;
    }

    public int Level()
    {
        return _m_iLevel;
    }

    @Override
    public EWCGTeamTriggerType triggerType()
    {
        return EWCGTeamTriggerType.SUMMON;
    }


    public static WCGTeamTriggerSummon read(String _infoStr)
    {
        WCGTeamTriggerSummon obj = new WCGTeamTriggerSummon();

        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 3)
        {
            CommLog.error("ai 指挥官技能SUMMON配置错误 正确配置: 枚举:单位类型:id:等级");
            return null;
        }
        try
        {
            obj._m_lUnitType = EWCGUnitType.valueOf(strs[0].toUpperCase().trim());
            obj._m_lActorId = Long.parseLong(strs[1].trim());
            obj._m_iLevel = Integer.parseInt(strs[2].trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai 指挥官技能SUMMON配置错误 正确配置: 枚举:单位类型:id:等级", e);

            return null;
        }
    }
}
