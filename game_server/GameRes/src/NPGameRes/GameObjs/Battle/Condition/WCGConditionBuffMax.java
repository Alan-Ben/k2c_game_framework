package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionBuffMax extends _AWCGBasicSingleCondition
{
    private long _m_lBuffID;

    public long BuffID()
    {
        return _m_lBuffID;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_STK;
    }

    public static WCGConditionBuffMax readCond(String _str)
    {
        WCGConditionBuffMax cond = new WCGConditionBuffMax();

        try
        {
            cond._m_lBuffID = Long.parseLong(_str.trim());

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - actor_type example: enum:actor_type... Error Str: " + _str, e);
            return null;
        }
    }
}