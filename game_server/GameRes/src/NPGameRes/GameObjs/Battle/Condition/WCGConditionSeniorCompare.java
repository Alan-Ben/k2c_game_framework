package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGCompareResult;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

/// <summary>
/// P_COMP - 属性比较   格式如下：枚举:高级公式1:高级公式2:比较类型
/// </summary>
public class WCGConditionSeniorCompare extends _AWCGBasicSingleCondition
{
    /*目标对象高级公式*/
    private WCGVariableGroupObj _m_eCompareDealer;

    /*比较目标对象高级公式*/
    private WCGVariableGroupObj _m_eCompareTargetDealer;

    /*比较结果类型*/
    private EWCGCompareResult _m_eCompareResult;

    public WCGVariableGroupObj CompareDealer()
    {
        return _m_eCompareDealer;
    }

    public WCGVariableGroupObj CompareTargetDealer()
    {
        return _m_eCompareTargetDealer;
    }

    public EWCGCompareResult CompareResult()
    {
        return _m_eCompareResult;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.S_COMP;
    }

    public static WCGConditionSeniorCompare readCond(String _str)
    {
        WCGConditionSeniorCompare cond = new WCGConditionSeniorCompare();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("条件配置错误 - Pro_Compare example: enum:target_a:target_b:compare_result Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_eCompareDealer = WCGVariableGroupObj.readVariableGroup(strs[0], "获取队伍资源数效果高级公式错误： ");
            cond._m_eCompareTargetDealer = WCGVariableGroupObj.readVariableGroup(strs[1], "获取队伍资源数效果高级公式错误： ");
            cond._m_eCompareResult = EWCGCompareResult.valueOf(strs[2].toUpperCase().trim());

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - Pro_Compare example: enum:target_a:target_b:compare_result Error Str: " + _str, e);
            return null;
        }
    }
}