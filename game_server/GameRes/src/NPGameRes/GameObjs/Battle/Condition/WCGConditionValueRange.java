package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGFloatRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;
import WCGCommon.Enum.NPEnum.EWCGValueType;

/**************
 * 满足状态值范围
 **/
public class WCGConditionValueRange extends _AWCGBasicSingleCondition
{
    private EWCGValueType _m_eValueType;

    private WCGFloatRange _m_rValueRange;

    public EWCGValueType ValueType()
    {
        return _m_eValueType;
    }

    public WCGFloatRange ValueRange()
    {
        return _m_rValueRange;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.V_RNG;
    }

    /**************
     * value_range:value_type:min:max - 状态值的范围
     **/
    public static WCGConditionValueRange read(String _infoStr)
    {
        WCGConditionValueRange obj = new WCGConditionValueRange();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 3)
        {
            CommLog.error("ai value_range:value_type:min:max - 状态值的范围 配置错误，_infoStr:" + _infoStr);
            return null;
        }
        try
        {
            obj._m_eValueType = EWCGValueType.valueOf(strs[0].toUpperCase().trim());
            obj._m_rValueRange = new WCGFloatRange(strs[1], strs[2]);
        } catch (Exception e)
        {
            CommLog.error("ai value_range:value_type:min:max - 状态值的范围 配置错误，_infoStr:" + _infoStr);
            return null;
        }
        return obj;
    }
}
