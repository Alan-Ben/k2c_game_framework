package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGActorType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionActorType extends _AWCGBasicSingleCondition
{
    private int _m_lActorType;

    public int ActorType()
    {
        return _m_lActorType;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.ACT_TYPE;
    }

    public static WCGConditionActorType readCond(String _str)
    {
        WCGConditionActorType cond = new WCGConditionActorType();

        String[] strs = CommonFunc.charSplit(_str, ':');
        try
        {
            for (int i = 0; i < strs.length; i++)
            {
                EWCGActorType actorType = EWCGActorType.valueOf(strs[i].toUpperCase().trim());
                cond._m_lActorType |= 1 << actorType.ordinal();
            }
            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - actor_type example: enum:target:actor_type... Error Str: " + _str + ";" + e.getCause().getMessage());
            return null;
        }
    }
}
