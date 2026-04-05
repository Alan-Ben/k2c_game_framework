package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


//主城等级条件
public class WCGConditionControlState extends _AWCGBasicSingleCondition
{
    //等级范围
    private long _m_iControlState;

    public long ControlStates()
    {
        return _m_iControlState;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.CTL_STAT;
    }

    public static WCGConditionControlState readCond(String _str)
    {
        WCGConditionControlState cond = new WCGConditionControlState();

        try
        {
            cond._m_iControlState = WCGResCommon.readControlType(_str);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - CTL_STAT example: enum@enum Error Str: " + _str);
            return null;
        }
    }
}
