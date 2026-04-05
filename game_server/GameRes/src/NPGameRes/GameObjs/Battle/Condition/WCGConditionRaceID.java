package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionRaceID extends _AWCGBasicSingleCondition
{
    private long _m_lRaceID;

    public long RaceID()
    {
        return _m_lRaceID;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RACE_ID;
    }

    public static WCGConditionRaceID readCond(String _str)
    {
        WCGConditionRaceID cond = new WCGConditionRaceID();

        try
        {
            cond._m_lRaceID = Long.parseLong(_str.trim());
            return cond;
        } catch (Exception e)
        {
            CommLog.error("种族ID条件配置错误  示例:  race_id:ins:2000");
            return null;
        }
    }

}
