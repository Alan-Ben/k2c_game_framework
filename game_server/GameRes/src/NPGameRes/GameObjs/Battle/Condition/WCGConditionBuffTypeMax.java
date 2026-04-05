package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


public class WCGConditionBuffTypeMax extends _AWCGBasicSingleCondition
{
    private long _m_lTypeID;

    public long TypeID()
    {
        return _m_lTypeID;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.BUF_T_STK;
    }

    public static WCGConditionBuffTypeMax readCond(String _str)
    {
        WCGConditionBuffTypeMax cond = new WCGConditionBuffTypeMax();

        try
        {
            cond._m_lTypeID = Long.parseLong(_str.trim());

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - buf_t_stk example: enum:typeid:min:max... Error Str: " + _str, e);
            return null;
        }
    }
}
