package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionBuffStack extends _AWCGBasicSingleCondition
{
    private long _m_lBuffID;

    private WCGIntRange _m_iRangeStack;

    public long BuffID()
    {
        return _m_lBuffID;
    }

    public WCGIntRange RangeStack()
    {
        return _m_iRangeStack;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_STK;
    }

    public static WCGConditionBuffStack readCond(String _str)
    {
        WCGConditionBuffStack cond = new WCGConditionBuffStack();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 1- buf_stack example: enum:buffId:min:max... Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_lBuffID = Long.parseLong(strs[0].trim().trim());
            cond._m_iRangeStack = new WCGIntRange(strs[1].trim(), strs[2].trim());

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 2- actor_type example: enum:actor_type... Error Str: " + _str, e);
            return null;
        }
    }
}
