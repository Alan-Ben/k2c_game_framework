package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

import java.util.ArrayList;
import java.util.List;


public class WCGConditionActorSid extends _AWCGBasicSingleCondition
{
    private List<Long> _m_lActorSid = new ArrayList<>();

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.ACT_SID;
    }

    public boolean isSidInRange(long _id)
    {
        return _m_lActorSid.indexOf(_id) != -1;
    }

    public static WCGConditionActorSid readCond(String _str)
    {
        WCGConditionActorSid cond = new WCGConditionActorSid();

        String[] strs = CommonFunc.charSplit(_str, ':');
        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                cond._m_lActorSid.add(Long.parseLong(strs[i]));
            }
            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - actor_sid example: enum:sid:sid... Error Str: " + _str + ";", e);
            return null;
        }
    }
}