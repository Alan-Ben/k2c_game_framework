package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

import java.util.ArrayList;
import java.util.List;


public class WCGConditionActorId extends _AWCGBasicSingleCondition
{
    private List<Long> _m_lActorId = new ArrayList<Long>();

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.ACT_ID;
    }

    public boolean isIdInRange(long _id)
    {
        return _m_lActorId.indexOf(_id) != -1;
    }

    public static WCGConditionActorId readCond(String _str)
    {
        WCGConditionActorId cond = new WCGConditionActorId();

        String[] strs = CommonFunc.charSplit(_str, ':');
        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                cond._m_lActorId.add(Long.parseLong(strs[i].trim()));
            }
            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - actor_id example: enum:id:id... Error Str: " + _str + ";", e);
            return null;
        }
    }
}
