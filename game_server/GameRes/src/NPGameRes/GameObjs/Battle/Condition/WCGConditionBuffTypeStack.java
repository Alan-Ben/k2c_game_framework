package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionBuffTypeStack extends _AWCGBasicSingleCondition
{
    private long _m_lTypeID;

    private WCGIntRange _m_iRangeStack;

    public long TypeID()
    {
        return _m_lTypeID;
    }

    public WCGIntRange RangeStack()
    {
        return _m_iRangeStack;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_T_STK;
    }

    public static WCGConditionBuffTypeStack readCond(String _str)
    {
        WCGConditionBuffTypeStack cond = new WCGConditionBuffTypeStack();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 - buf_t_stk example: enum:typeid:min:max... Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_lTypeID = Long.parseLong(strs[0].trim());
            cond._m_iRangeStack = new WCGIntRange(strs[1], strs[2]);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - buf_t_stk example: enum:typeid:min:max... Error Str: " + _str);
            return null;
        }
    }
}
