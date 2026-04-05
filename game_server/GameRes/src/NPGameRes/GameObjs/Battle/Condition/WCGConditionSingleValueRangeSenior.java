package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;
import WCGCommon.Enum.NPEnum.EWCGValueType;

/**************
 * 状态值范围
 格式如下：枚举:状态值类型:最小值（高级计算公式）:最大值（高级计算公式）

 示例：当前血量小于等于10点 - v_rng:hp:num@-1:num@10
 示例：当前攻击大于等于hp点 - v_rng:attack:value@ins@hp:num@-1
 **/
public class WCGConditionSingleValueRangeSenior extends _AWCGBasicSingleCondition
{
    private EWCGValueType _m_eValueType;

    private WCGVariableGroupObj _m_sValueVariableMin;
    private WCGVariableGroupObj _m_sValueVariableMax;
//    private WCGFloatRange _m_rValueRange;

    public EWCGValueType ValueType()
    {
        return _m_eValueType;
    }

    public WCGVariableGroupObj variableMin()
    {
        return _m_sValueVariableMin;
    }

    public WCGVariableGroupObj variableMax()
    {
        return _m_sValueVariableMax;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.V_RNG_S;
    }

    /**************
     * v_rng_s:value_type:min(高级公式):max(高级公式) - 状态值的范围
     **/
    public static WCGConditionSingleValueRangeSenior read(String _infoStr)
    {
        WCGConditionSingleValueRangeSenior obj = new WCGConditionSingleValueRangeSenior();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 3)
        {
            CommLog.error("v_rng_s:value_type:min(高级公式):max(高级公式) - 状态值的范围 配置错误，_infoStr:" + _infoStr);
            return null;
        }
        try
        {
            obj._m_eValueType = EWCGValueType.valueOf(strs[0].toUpperCase().trim());
            obj._m_sValueVariableMin = WCGVariableGroupObj.readVariableGroup(strs[1], "v_rng_s:value_type:min(高级公式):max(高级公式) - 状态值的范围 min高级公式错误　");
            obj._m_sValueVariableMax = WCGVariableGroupObj.readVariableGroup(strs[2], "v_rng_s:value_type:min(高级公式):max(高级公式) - 状态值的范围 max高级公式错误　");
        } catch (Exception e)
        {
            CommLog.error("v_rng_s:value_type:min(高级公式):max(高级公式) - 状态值的范围 配置错误，_infoStr:" + _infoStr, e);
            return null;
        }
        return obj;
    }
}
