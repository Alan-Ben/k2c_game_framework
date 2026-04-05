package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionSkillCD extends _AWCGBasicSingleCondition
{

    private long _m_lSkillId;

    private WCGIntRange _m_irLvlRange;

    public long SkillID()
    {
        return _m_lSkillId;
    }

    public WCGIntRange lvlRange()
    {
        return _m_irLvlRange;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SK_OK;
    }

    public static WCGConditionSkillCD read(String _infoStr)
    {
        WCGConditionSkillCD obj = new WCGConditionSkillCD();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        try
        {
            obj._m_lSkillId = Long.parseLong(strs[0].trim());

            int min = -1;
            int max = -1;

            if (strs.length > 1)
                min = Integer.parseInt(strs[1].trim());
            if (strs.length > 2)
                max = Integer.parseInt(strs[2].trim());
            obj._m_irLvlRange = new WCGIntRange(min, max);

            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - SKILL_CD_OK example: enum:skillId:skillLevel Error Str: " + _infoStr);
            return null;
        }
    }
}
