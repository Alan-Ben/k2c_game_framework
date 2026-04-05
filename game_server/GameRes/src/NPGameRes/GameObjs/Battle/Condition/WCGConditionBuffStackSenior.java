package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


public class WCGConditionBuffStackSenior extends _AWCGBasicSingleCondition
{
    private WCGVariableGroupObj _m_voBufIdVar;

    private WCGVariableGroupObj _m_voBufStkMin;
    private WCGVariableGroupObj _m_voBufStkMax;

    public WCGVariableGroupObj BuffID()
    {
        return _m_voBufIdVar;
    }

    public WCGVariableGroupObj min()
    {
        return _m_voBufStkMin;
    }

    public WCGVariableGroupObj max()
    {
        return _m_voBufStkMax;
    }


    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_STK_S;
    }

    public static WCGConditionBuffStackSenior readCond(String _str)
    {
        WCGConditionBuffStackSenior cond = new WCGConditionBuffStackSenior();

        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 - BUF_STK_S example: BUF_STK_S:bufidS:minS:maxS... Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_voBufIdVar = WCGVariableGroupObj.readVariableGroup(strs[0], "buf_stk_s1");
            cond._m_voBufStkMin = WCGVariableGroupObj.readVariableGroup(strs[1], "buf_stk_s2");
            cond._m_voBufStkMax = WCGVariableGroupObj.readVariableGroup(strs[2], "buf_stk_s3");

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - BUF_STK_S example: BUF_STK_S:bufidS:minS:maxS... Error Str: " + _str);
            return null;
        }
    }

    public static boolean inRange(int _value, long _min, long _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }
}
