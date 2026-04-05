package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

//范围内是否有争夺战斗发生
public class WCGConditionRangeFighting extends _AWCGBasicSingleCondition
{
    private int _m_iRange;//范围  厘米

    private int _m_iEnableValue;//有效标记   0代表没有争夺状态  1代表有争夺状态

    public int Range()
    {
        return _m_iRange;
    }

    public int EnableValue()
    {
        return _m_iEnableValue;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RNG_FT;
    }

    protected WCGConditionRangeFighting()
    {
        _m_iEnableValue = 1;
    }


    public static WCGConditionRangeFighting readCond(String _infoStr)
    {
        WCGConditionRangeFighting obj = new WCGConditionRangeFighting();

        String[] strs = CommonFunc.charSplit(_infoStr, ':');

        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            if (strs.length > 1)
                obj._m_iEnableValue = Integer.parseInt(strs[1].trim());
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - RNG_EMY_GRP example: enum:range:count_range Error Str: " + _infoStr);
            return null;
        }

    }
}
